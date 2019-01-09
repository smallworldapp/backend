using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SmallWorld.Auth;
using SmallWorld.Database.Entities;
using SmallWorld.Filters;
using SmallWorld.Models.Emailing;

namespace SmallWorld.Controllers
{
    [Route("manualpairings")]
    public class ManualPairingsController : MyController
    {
        private readonly EmailProvider emails;

        public ManualPairingsController(EmailProvider emails)
        {
            this.emails = emails;
        }

        [HttpPost]
        [AuthRequired]
        public IActionResult ResolveVerification([FromBody] List<ManualPairing> pairings)
        {
            if (!(Auth is AdminSession))
                return Unauthorized();

            foreach (var pairing in pairings)
            {
                emails.Send(Emails.ManualPairing, pairing.Initiator, pairing.Receiver, pairing.Subject);
            }

            return Ok();
        }

        public class ManualPairing
        {
            public Identity Initiator { get; set; }
            public Identity Receiver { get; set; }
            public string Subject { get; set; }
        }
    }
}