using System;
using Microsoft.AspNetCore.Mvc;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Filters;

namespace SmallWorld.Controllers
{
    [Route("verifications/{token}")]
    public class VerificationsController : MyController
    {
        private readonly IMemberRepository members;

        public VerificationsController(IMemberRepository members)
        {
            this.members = members;
        }

        [HttpPost]
        [DatabaseUpdate]
        public IActionResult ResolveVerification(Guid token)
        {
            if (members.Find(m => m.JoinToken == token, out var joining))
            {
                joining.HasEmailValidation = true;
                members.Update(joining);

                return Ok(new { type = "join" });
            }

            if (members.Find(m => m.LeaveToken == token, out var leaving))
            {
                leaving.HasLeft = true;
                members.Update(leaving);

                return Ok(new { type = "leave" });
            }

            return NotFound();
        }
    }
}