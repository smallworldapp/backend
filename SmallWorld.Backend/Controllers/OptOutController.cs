using System;
using Microsoft.AspNetCore.Mvc;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Filters;

namespace SmallWorld.Controllers
{
    [Route("worlds/{worldId}/opt-out")]
    public class OptOutController : MyController
    {
        private readonly IWorldRepository worlds;

        public OptOutController(IWorldRepository worlds)
        {
            this.worlds = worlds.Include(w => w.Members);
        }

        [HttpPost]
        [DatabaseUpdate]
        public IActionResult RequestOptOut(Guid worldId, [FromQuery] Guid memberId)
        {
            if (!worlds.Find(worldId, out var world))
                return NotFound();

            var members = worlds.Members(world);

            if (!members.Find(memberId, out var member))
                return NotFound();

            member.OptOut = true;

            members.Update(member);

            return Ok();
        }
    }
}