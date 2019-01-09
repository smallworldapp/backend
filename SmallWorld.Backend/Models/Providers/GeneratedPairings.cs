using System;
using System.Collections.Generic;
using System.Linq;
using SmallWorld.Database.Entities;

namespace SmallWorld.Models.Providers
{
    public class GeneratedPairings
    {
        public IEnumerable<Pair> Pairs { get; }

        public IEnumerable<Member> OptedOut { get; }
        public Member UnpairedMember { get; }

        public GeneratedPairings(IEnumerable<Pair> pairs, IEnumerable<Member> opted, Member odd)
        {
            Pairs = pairs;
            OptedOut = opted;
            UnpairedMember = odd;
        }

        public static GeneratedPairings Generate(Pairing pairing, bool allowRepeat)
        {
            var opted = new List<Member>();

            var random = new Random();

            for (var i = 0; i < 10; i++)
            {
                var users = (from u in pairing.World.Members
                             where CanBePaired(u)
                             select u).ToList();
                if (!users.Any()) break;

                foreach (var user in users.ToArray())
                {
                    if (!user.OptOut) continue;

                    users.Remove(user);
                    opted.Add(user);
                }

                var pairs = new List<Pair>();
                while (true)
                {
                    var one = users[random.Next(users.Count)];

                    var options = (from u in users
                                   where u != one && (allowRepeat || !one.Paired().Contains(u.Id))
                                   select u).ToList();
                    if (!options.Any()) break;

                    var two = options[random.Next(options.Count)];

                    users.Remove(one);
                    users.Remove(two);

                    bool rand = random.NextDouble() < .5;
                    var pair = new Pair {
                        Pairing = pairing,
                        World = pairing.World,
                        Initiator = rand ? two : one,
                        Receiver = rand ? one : two
                    };
                    pairs.Add(pair);

                    if (users.Count < 2) break;
                }

                if (users.Count >= 2) continue;
                var odd = users.SingleOrDefault();
                return new GeneratedPairings(pairs, opted, odd);
            }

            return new GeneratedPairings(new List<Pair>(), new List<Member>(), null);
        }

        private static bool CanBePaired(Member member)
        {
            return member.HasEmailValidation && member.HasPrivacyValidation && !member.HasLeft;
        }
    }
}