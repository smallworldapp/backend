using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Filters;

namespace SmallWorld.Controllers
{
    [Route("worlds/{worldId}/pairings")]
    public class PairingsController : MyController
    {
        private readonly IWorldRepository worlds;

        public PairingsController(IWorldRepository worlds)
        {
            this.worlds = worlds;
        }

        [HttpGet]
        [AuthRequired]
        public IActionResult GetPairings(Guid worldId)
        {
            if (!worlds.Find(worldId, out var world))
                return NotFound();

            if (!Permissions.AccessWorld(world))
                return NotFound();

            var pairings = worlds.Pairings(world);

            var list = pairings.All.Where(p => p.Date > DateTime.UtcNow);
            return Ok(list);
        }

        [HttpPost]
        [AuthRequired]
        [DatabaseUpdate]
        public IActionResult CreatePairing(Guid worldId, [FromBody] PairingRequest request)
        {
            if (!worlds.Find(worldId, out var world))
                return NotFound();

            if (!Permissions.ModifyWorld(world))
                return NotFound();

            var pairings = worlds.Pairings(world);

            var pairing = new Pairing {
                IsComplete = false,
                Type = PairingType.Auto,
                Date = request.Date.ToUniversalTime(),
                Message = request.Message,
            };

            pairings.Add(pairing);

            return Ok(pairing);
        }

        [HttpPost("manual")]
        [AuthRequired]
        [DatabaseUpdate]
        public IActionResult CreateManualPairings(Guid worldId, [FromBody] ManualPairingRequest request)
        {
            var value = worlds
                .Include(w => w.Members)
                .Include(w => w.Pairs)
                .Find(worldId);

            if (!value.Exists(out var world))
                return NotFound();

            if (!Permissions.ModifyWorld(world))
                return NotFound();

            var pairings = worlds.Pairings(world);

            if (request.Pairs == null || !request.Pairs.Any())
                return BadRequest();

            var pairing = new Pairing {
                IsComplete = false,
                Type = PairingType.Manual,
                Date = request.Date.ToUniversalTime(),
                Message = request.Message,
                Pairs = new HashSet<Pair>()
            };

            pairings.Add(pairing);

            var pairs = pairings.Pairs(pairing);

            foreach (var src in request.Pairs)
            {
                var pair = new Pair {
                    Initiator = world.Members.FirstOrDefault(m => m.Guid == src.Initiator),
                    Receiver = world.Members.FirstOrDefault(m => m.Guid == src.Receiver),
                    Outcome = PairOutcome.Unknown
                };

                if (pair.Initiator == null || pair.Receiver == null || pair.Initiator == pair.Receiver)
                    return BadRequest();

                pairs.Add(pair);
            }

            return Ok(pairing);
        }

        [HttpDelete("{id}")]
        [AuthRequired]
        [DatabaseUpdate]
        public IActionResult DeletePairing(Guid worldId, Guid id)
        {
            if (!worlds.Find(worldId, out var world))
                return NotFound();

            if (!Permissions.ModifyWorld(world))
                return NotFound();

            var pairings = worlds.Pairings(world);

            if (!pairings.Find(id, out var pairing))
                return NotFound();

            if (pairing.IsComplete)
                return BadRequest();

            pairings.Delete(pairing);

            return Ok();
        }

        public class PairingRequest
        {
            public DateTime Date { get; set; }
            public string Message { get; set; }
        }

        public class ManualPairingRequest
        {
            public DateTime Date { get; set; }
            public string Message { get; set; }
            public List<ManualPair> Pairs { get; set; }
        }

        public class ManualPair
        {
            public Guid Initiator { get; set; }
            public Guid Receiver { get; set; }
        }
    }
}