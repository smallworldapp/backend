using System;
using Microsoft.AspNetCore.Mvc;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Filters;

namespace SmallWorld.Controllers
{
    [Route("worlds/{worldId}/pairs")]
    public class PairsController : MyController
    {
        private readonly IWorldRepository worlds;

        public PairsController(IWorldRepository worlds)
        {
            this.worlds = worlds;
        }

        [HttpGet]
        [AuthRequired]
        public IActionResult GetPairs(Guid worldId)
        {
            if (!worlds.Find(worldId, out var world))
                return NotFound();

            if (!Permissions.AccessWorld(world))
                return NotFound();

            var pairs = worlds.Pairs(world);

            return Ok(pairs.All);
        }
    }
}