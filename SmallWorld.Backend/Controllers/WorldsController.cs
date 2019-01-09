using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmallWorld.Auth;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Filters;

namespace SmallWorld.Controllers
{
    [Route("worlds")]
    [Route("accounts/{accountId}/worlds")]
    public class WorldsController : MyController
    {
        private readonly IWorldRepository worlds;

        public WorldsController(IWorldRepository worlds)
        {
            this.worlds = worlds.Include(w => w.Account);
        }

        [HttpGet]
        [AuthRequired]
        public IActionResult GetWorlds(Guid? accountId)
        {
            var src = worlds.NotDeleted;

            if (accountId.HasValue)
            {
                src = from world in src
                      where world.Account.Guid == accountId
                      select world;
            }

            var list = from world in src.ToList()
                       where Permissions.AccessWorld(world)
                       select world;

            return Ok(list);
        }

        [HttpPost]
        [AuthRequired]
        [DatabaseUpdate]
        public IActionResult CreateWorld([FromBody] World world)
        {
            var auth = Auth as AccountSession;

            if (auth == null)
                return Unauthorized();

            var acc = auth.GetAccount(HttpContext.RequestServices);

            switch (acc.Type)
            {
                case AccountType.ERROR:
                    throw new InvalidOperationException("Invalid account type: " + acc.Guid);

                case AccountType.Standard:
                    if (world.Privacy == WorldPrivacy.ERROR)
                        return BadRequest();
                    break;

                case AccountType.Research:
                    world.Privacy = WorldPrivacy.InviteOnly;
                    break;

                default:
                    throw new NotImplementedException();
            }

            world.Status = WorldStatus.Passed;

            world.Account = acc;
            world.BackupUser = null;
            world.Application = Application.Default();

            if (world.Description == null)
                world.Description = Description.Default();

            world.PairingSettings = null;

            worlds.Add(world);

            return Ok(world);
        }
    }
}