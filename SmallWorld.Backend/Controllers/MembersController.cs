using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SmallWorld.Converters.Members;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Filters;
using SmallWorld.Models.Emailing;

namespace SmallWorld.Controllers
{
    [Route("worlds/{worldId}/members")]
    public class MembersController : MyController
    {
        private readonly IWorldRepository worlds;
        private readonly EmailProvider emails;

        public MembersController(IWorldRepository worlds, EmailProvider emails)
        {
            this.worlds = worlds;
            this.emails = emails;
        }

        [HttpGet]
        [AuthRequired]
        public IActionResult GetMembers(Guid worldId)
        {
            if (!worlds.Find(worldId, out var world))
                return NotFound();

            if (!Permissions.AccessWorld(world))
                return NotFound();

            var members = worlds.Members(world);

            return Ok(members.All);
        }

        [HttpPost]
        [DatabaseUpdate]
        public IActionResult CreateMembers(Guid worldId, [FromQuery] bool confirm, [FromBody] List<Member> body)
        {
            var value = worlds
                .Include(w => w.Account)
                .Find(worldId);

            if (!value.Exists(out var world))
                return NotFound();

            var members = worlds.Members(world);

            var isConfirmed = true;

            if (Auth == null || !Permissions.ModifyWorld(world))
            {
                if (world.Privacy == WorldPrivacy.InviteOnly)
                    return Unauthorized();

                if (world.Privacy == WorldPrivacy.VerificationRequired)
                    isConfirmed = false;

                confirm = true;
            }

            var list = new List<Member>();

            foreach (var id in body)
            {
                id.FirstName = id.FirstName.Trim();
                id.LastName = id.LastName.Trim();
                id.Email = id.Email.Trim();

                var isNew = false;
                if (!members.Find(id.Email, out var member))
                {
                    member = new Member();
                    isNew = true;
                }

                member.FirstName = id.FirstName;
                member.LastName = id.LastName;
                member.Email = id.Email;

                member.JoinToken = Database.Token.Generate();
                member.LeaveToken = Database.Token.Generate();

                if (isNew)
                    members.Add(member);
                else
                    members.Update(member);

                list.Add(member);

                switch (world.Account.Type)
                {
                    case AccountType.Standard:
                        member.HasLeft = false;
                        member.HasEmailValidation = !confirm;
                        member.HasPrivacyValidation = isConfirmed;

                        if (Auth == null)
                            emails.Send(Emails.JoinConfirmation, member);
                        else
                            emails.Send(Emails.MemberAdded, member);
                        break;

                    case AccountType.Research:
                        member.HasLeft = false;
                        member.HasEmailValidation = true;
                        member.HasPrivacyValidation = true;
                        break;
                }
            }

            return Ok(list);
        }

        [HttpPut("{id}/status")]
        [AuthRequired]
        [DatabaseUpdate]
        public IActionResult UpdateMember(Guid worldId, Guid id, [FromBody] MemberStatus update)
        {
            if (!worlds.Find(worldId, out var world))
                return NotFound();

            if (!Permissions.ModifyWorld(world))
                return NotFound();

            var members = worlds.Members(world);

            if (!members.Find(id, out var member))
                return NotFound();

            member.HasLeft = update.left;
            member.HasEmailValidation = update.email;
            member.HasPrivacyValidation = update.privacy;

            members.Update(member);

            return Ok(member);
        }

        [HttpDelete]
        [DatabaseUpdate]
        public IActionResult DeleteMember(Guid worldId, [FromQuery] string email)
        {
            if (!worlds.Find(worldId, out var world))
                return NotFound();

            if (email == null)
                return NotFound();

            var members = worlds.Members(world);

            if (!members.Find(new EmailAddress(email), out var member))
                return NotFound();

            emails.Send(Emails.LeaveConfirmation, member);

            return Ok();
        }
    }
}
