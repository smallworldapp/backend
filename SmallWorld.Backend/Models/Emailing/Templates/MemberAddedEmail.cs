using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models.Emailing.Abstractions;
using SmallWorld.Models.Providers;

namespace SmallWorld.Models.Emailing.Templates
{
    public class MemberAddedEmail : EmailTemplate, IEmailTemplate<Member>
    {
        private readonly IEntryRepository entries;
        private readonly LinkProvider links;

        public MemberAddedEmail(IEntryRepository entries, LinkProvider links)
        {
            this.entries = entries;
            this.links = links;
        }

        public Email Create(Member member)
        {
            entries.Entry(member)
                .LoadRelations(m => m.World.Account);

            Subject = "Welcome to smallworld";
            To(member);

            Write($@"Hi {member.FirstName},");

            if (!member.HasEmailValidation)
            {
                Write($@"You have been added to the {member.World.Name} smallworld community by
{member.World.Account.Name}. To participate you need to confirm your email address by clicking
[this link]({links.JoinConfirmation(member)}).");
            }
            else
            {
                Write($@"You have been added to the {member.World.Name} smallworld community by
{member.World.Account.Name}. You can expect to receive your first smallworld pairing soon.

You can leave at any time with [this link]({links.InvitePage(member.World)}).");
            }

            Write("-The smallworld Team");

            return Finish();
        }
    }
}