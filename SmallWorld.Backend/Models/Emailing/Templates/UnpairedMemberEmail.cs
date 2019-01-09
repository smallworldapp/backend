using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models.Emailing.Abstractions;

namespace SmallWorld.Models.Emailing.Templates
{
    public class UnpairedMemberEmail : EmailTemplate, IEmailTemplate<Member>
    {
        private readonly IEntryRepository entries;

        public UnpairedMemberEmail(IEntryRepository entries)
        {
            this.entries = entries;
        }

        public Email Create(Member member)
        {
            entries.Entry(member)
                .LoadRelations(m => m.World);

            Subject = $"{member.World.Name} - smallworld Connection";
            To(member);

            Write($@"Hi {member.FirstName},

There are an odd number of members signed up in your community and
unfortunately you were not paired with anyone this round. We encourage
you to meet someone you don’t know in your community on your own!

-The smallworld Team
");

            return Finish();
        }
    }
}