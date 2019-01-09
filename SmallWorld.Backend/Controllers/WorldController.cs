using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmallWorld.Auth;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Filters;
using SmallWorld.Library.Model;
using SmallWorld.Models.Emailing;

namespace SmallWorld.Controllers
{
    [Route("worlds/{identifier}")]
    [Route("accounts/{accountId}/worlds/{identifier}")]
    public class WorldController : MyController
    {
        private readonly IWorldRepository worlds;

        //        private IQueryable<World> Worlds => context.Worlds
        //            .Include(w => w.Account)
        //            .Where(a => a.Status != WorldStatus.Deleted);

        public WorldController(IWorldRepository worlds)
        {
            this.worlds = worlds.Include(w => w.Account);
        }

        [HttpGet]
        public IActionResult GetWorld(Guid? accountId, string identifier)
        {
            Optional<World> value;

            if (Guid.TryParse(identifier, out var guid))
                value = worlds.Find(guid);
            else
                value = worlds.Find(new Identifier(identifier));

            if (!value.Exists(out World world))
                return NotFound();

            if (accountId.HasValue && world.Account.Guid != accountId.Value)
                return NotFound();

            if (Auth != null && Permissions.AccessWorld(world))
                return Ok(world);

            worlds.Entry(world)
                .LoadRelations(w => w.Description.Faq);

            return Ok(new {
                id = world.Guid,
                identifier = world.Identifier,
                name = world.Name,
                privacy = world.Privacy,
                description = world.Description
            });
        }

        [HttpPatch]
        [AuthRequired]
        [DatabaseUpdate]
        public IActionResult UpdateWorld(Guid? accountId, string identifier, [FromBody] World update)
        {
            Optional<World> value;

            if (Guid.TryParse(identifier, out var guid))
                value = worlds.Find(guid);
            else
                value = worlds.Find(new Identifier(identifier));

            if (!value.Exists(out var world))
                return NotFound();

            if (!Permissions.ModifyWorld(world))
                return NotFound();

            if (accountId.HasValue && world.Account.Guid != accountId.Value)
                return NotFound();

            if (update.Status != WorldStatus.ERROR)
            {
                if (!(Auth is AdminSession))
                    return Unauthorized();

                world.Status = update.Status;
            }

            if (world.Status == WorldStatus.Passed)
            {
                if (update.Privacy != WorldPrivacy.ERROR)
                    world.Privacy = update.Privacy;

                if (update.BackupUser != null)
                {
                    if (update.BackupUser.Email == null)
                    {
                        world.BackupUser = null;
                    }
                    else
                    {
                        update.BackupUser.CreateIds();
                        world.BackupUser = update.BackupUser;
                    }
                }

                if (update.Identifier != null)
                {
                    if (update.Identifier.Value == "")
                        world.Identifier = null;
                    else
                        world.Identifier = update.Identifier;
                }
            }

            worlds.Update(world);

            return Ok(world);
        }

        [HttpDelete]
        [AuthRequired]
        [DatabaseUpdate]
        public IActionResult DeleteWorld(Guid? accountId, Guid identifier)
        {
            if (!worlds.Find(identifier, out var world))
                return NotFound();

            if (!Permissions.DeleteWorld(world))
                return NotFound();

            if (accountId.HasValue && world.Account.Guid != accountId.Value)
                return NotFound();

            world.Status = WorldStatus.Deleted;

            worlds.Update(world);

            return Ok();
        }

        [HttpPost("emails/{type}")]
        [AuthRequired]
        public IActionResult SendEmails(Guid? accountId, Guid identifier, string type, [FromServices] EmailProvider emails)
        {
            if (!worlds.Include(w => w.Members).Find(identifier, out var world))
                return NotFound();

            if (!Permissions.ModifyWorld(world))
                return NotFound();

            if (accountId.HasValue && world.Account.Guid != accountId.Value)
                return NotFound();

            var members = worlds.Members(world);

            switch (type)
            {
                case "confirmation":
                    var unconfirmed = members.All
                        .Where(u => !u.HasEmailValidation);

                    foreach (var member in unconfirmed)
                        emails.Send(Emails.JoinConfirmation, member);

                    break;

                default:
                    return NotFound();
            }

            return Ok();
        }
    }
}
