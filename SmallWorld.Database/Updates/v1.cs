using System;
using System.Collections.Generic;
using System.Linq;
using SmallWorld.Database.Entities;

namespace SmallWorld.Database.Updates
{
    public class v1 : UpdateSet<Migrations.v1>
    {
        public override IEnumerable<IUpdate> Updates()
        {
            yield return new RemoveMembershipStatus();
            yield return new InitializeWorldIdentifiers();
        }

        private class RemoveMembershipStatus : Update
        {
            public override void Apply(SmallWorldContext context)
            {
                var members = from member in context.Members
                              where member._Deprecated_Status != MembershipStatus._Deprecated_Removed
                              select member;

                foreach (var member in members)
                {
                    var status = member._Deprecated_Status;

                    if (status == MembershipStatus.ERROR)
                        throw new Exception("Invalid member state when updating database: " + status);

                    member.HasLeft = status == MembershipStatus.Left;
                    member.HasEmailValidation = status == MembershipStatus.Confirmed;
                    member.HasPrivacyValidation = true;
                }
            }
        }

        private class InitializeWorldIdentifiers : Update
        {
            public override void Apply(SmallWorldContext context)
            {
                var groups = from world in context.Worlds
                             where world.Status == WorldStatus.Passed
                             orderby world.Members.Count descending
                             group world by world.Name.Value.ToLower().Replace(" ", "-") into similar
                             select similar;

                foreach (var group in groups)
                {
                    var largest = group.First();
                    largest.Identifier = new Identifier(group.Key);
                }
            }
        }
    }
}
