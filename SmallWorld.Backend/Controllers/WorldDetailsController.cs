using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Filters;

namespace SmallWorld.Controllers
{
    [Route("worlds/{id}")]
    [Route("accounts/{accountId}/worlds/{id}")]
    public class WorldDetailsController : MyController
    {
        private readonly IWorldRepository worlds;

        public WorldDetailsController(IWorldRepository worlds)
        {
            this.worlds = worlds.Include(w => w.Account);
        }

        [HttpGet("application")]
        [AuthRequired]
        public IActionResult GetWorldApplication(Guid? accountId, Guid id)
        {
            if (!worlds.Include(w => w.Application).Find(id, out var world))
                return NotFound();

            if (!Permissions.AccessWorld(world))
                return NotFound();

            if (accountId.HasValue && world.Account.Guid != accountId.Value)
                return NotFound();

            return Ok(world.Application);
        }

        [HttpPatch("application")]
        [AuthRequired]
        [DatabaseUpdate]
        public IActionResult UpdateApplication(Guid? accountId, Guid id, [FromBody] Application update)
        {
            if (!worlds.Find(id, out var world))
                return NotFound();

            if (!Permissions.ModifyWorld(world))
                return NotFound();

            if (accountId.HasValue && world.Account.Guid != accountId.Value)
                return NotFound();

            worlds.Entry(world)
                .LoadRelations(w => w.Application.Questions);

            foreach (var que in update.Questions)
            {
                if (que.Answer == null)
                    continue;

                var old = world.Application.Questions.FirstOrDefault(q => q.Guid == que.Guid);
                if (old == null)
                    continue;

                old.Answer = que.Answer;
            }

            worlds.Update(world);

            return Ok(world.Application);
        }

        [HttpGet("description")]
        [AuthRequired]
        public IActionResult GetWorldDescription(Guid? accountId, Guid id)
        {
            if (!worlds.Include(w => w.Description).Find(id, out var world))
                return NotFound();

            if (!Permissions.AccessWorld(world))
                return NotFound();

            if (accountId.HasValue && world.Account.Guid != accountId.Value)
                return NotFound();

            return Ok(world.Description);
        }

        [HttpPatch("description")]
        [AuthRequired]
        [DatabaseUpdate]
        public IActionResult UpdateDescription(Guid? accountId, Guid id, [FromBody] Description update)
        {
            if (!worlds.Find(id, out var world))
                return NotFound();

            if (!Permissions.ModifyWorld(world))
                return NotFound();

            if (accountId.HasValue && world.Account.Guid != accountId.Value)
                return NotFound();

            if (world.Status != WorldStatus.Passed)
                return Unauthorized();

            worlds.Entry(world)
                .LoadRelations(w => w.Description.Faq);

            if (update.Introduction != null)
                world.Description.Introduction = update.Introduction;

            if (update.Goals != null)
                world.Description.Goals = update.Goals;

            world.Description.Faq.Clear();
            foreach (var item in update.Faq)
            {
                item.CreateIds();
                world.Description.Faq.Add(item);
            }

            worlds.Update(world);

            return Ok(world.Description);
        }
    }
}