using System;
using System.Linq;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models.Emailing.Abstractions;
using SmallWorld.Models.Providers;

namespace SmallWorld.Models.Emailing.Templates
{
    public class OptOutEmail : EmailTemplate, IEmailTemplate<Member>
    {
        private readonly IEntryRepository entries;
        private readonly IWorldRepository worlds;
        private readonly LinkProvider links;

        public OptOutEmail(IEntryRepository entries, IWorldRepository worlds, LinkProvider links)
        {
            this.entries = entries;
            this.worlds = worlds;
            this.links = links;
        }

        public Email Create(Member member)
        {
            entries.Entry(member)
                .LoadRelations(m => m.World);

            var pairings = worlds.Pairings(member.World);

            var pairing = (from p in pairings.All
                           where p.Date > DateTime.UtcNow
                           orderby p.Date ascending
                           select p).FirstOrDefault();

            Subject = "smallworld Pairing";
            To(member);

            Write($@"Hi {member.FirstName},");

            if (pairing != null)
            {
                Write($@"Your next pairing date is on {pairing.Date:M/d/yy}, if you would like to opt out of your
next pairing, [click here]({links.OptOut(member)}).");
            }
            else
            {
                Write($@" You will soon be paired again for smallworld.If you would like to opt out of your
next pairing, [click here]({links.OptOut(member)}).");
            }

            Write("-The smallworld Team");

            return Finish();
        }
    }
}