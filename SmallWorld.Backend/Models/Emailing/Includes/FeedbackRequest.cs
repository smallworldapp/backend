using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Models.Emailing.Abstractions;
using SmallWorld.Models.Providers;

namespace SmallWorld.Models.Emailing.Includes
{
    public class FeedbackRequest : IEmailInclude
    {
        private readonly IPairRepository pairs;
        private readonly LinkProvider links;

        public FeedbackRequest(IPairRepository pairs, LinkProvider links)
        {
            this.pairs = pairs.Include(p => p.Receiver).Include(p => p.Initiator);
            this.links = links;
        }

        public string Create(Member member, Pairing current)
        {
            if (member == null)
                return "";

            IList<Pair> list;
            if (current == null)
            {
                list = (from pair in pairs.All
                        where (pair.Initiator == member || pair.Receiver == member) &&
                              pair.Outcome == PairOutcome.Unknown &&
                              pair.Pairing.IsComplete
                        select pair).ToList();
            }
            else
            {
                list = (from pair in pairs.All
                        where (pair.Initiator == member || pair.Receiver == member) &&
                              pair.Outcome == PairOutcome.Unknown &&
                              pair.Pairing != current &&
                              pair.Pairing.IsComplete
                        select pair).ToList();
            }

            if (!list.Any())
                return "";

            var str = "Please click the links below to help us understand which smallworld connections were successful.";

            foreach (var pair in list)
            {
                var other = pair.Initiator.Id == member.Id ? pair.Receiver : pair.Initiator;

                str += $"  \n{other.FullName()}: [Met]({links.Feedback(pair, "yes")}) | [Did not meet]({links.Feedback(pair, "no")}) | [Planning to meet]({links.Feedback(pair, "planning")})";
            }

            return str;
        }
    }
}
