﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models.Emailing;

namespace SmallWorld.Models.PairGenerators
{
    public class AutoGeneratedPairs : GeneratedPairs
    {
        private const bool allowRepeats = false;

        private readonly IPairingRepository pairings;
        private readonly EmailProvider emails;

        private Pairing pairing;

        private List<Pair> pairs;
        private List<Member> optOuts;
        private Member unpaired;

        public AutoGeneratedPairs(IPairingRepository pairings, EmailProvider emails)
        {
            this.pairings = pairings;
            this.emails = emails;
        }

        public override void Generate(Pairing inPairing)
        {
            pairing = inPairing;

            pairings.Entry(pairing)
                .LoadRelations(p => p.Pairs)
                .LoadRelations(p => p.World)
                .LoadRelations(p => p.World.Pairs)
                .LoadRelations(p => p.World.BackupUser)
                .LoadRelations(p => p.World.Members, m => m.Pairs1)
                .LoadRelations(p => p.World.Members, m => m.Pairs2);

            var random = new Random();

            for (var i = 0; i < 10; i++)
            {
                pairs = new List<Pair>();
                optOuts = new List<Member>();
                unpaired = null;

                var users = (from u in pairing.World.Members
                             where CanBePaired(u)
                             select u).ToList();
                if (!users.Any()) break;

                foreach (var user in users.ToArray())
                {
                    if (!user.OptOut) continue;

                    users.Remove(user);
                    optOuts.Add(user);
                }

                while (true)
                {
                    var one = users[random.Next(users.Count)];

                    var options = (from u in users
                                   where u != one && (allowRepeats || !one.Paired().Contains(u.Id))
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

                unpaired = users.SingleOrDefault();

                return;
            }

            pairs = new List<Pair>();
            optOuts = new List<Member>();
            unpaired = null;
        }

        public override async Task Submit(IContextLock access)
        {
            using (var handle = await access.Write())
            {
                var repo = pairings.Pairs(pairing);

                foreach (var pair in pairs)
                {
                    pair.Outcome = PairOutcome.Unknown;

                    repo.Add(pair);

                    emails.Send(Emails.PairingReceiver, pair);
                    emails.Send(Emails.PairingInitiator, pair);
                }

                foreach (var member in optOuts)
                {
                    member.OptOut = false;

                    emails.Send(Emails.OptOut, member);
                }

                if (unpaired != null)
                {
                    if (pairing.World.BackupUser != null)
                    {
                        emails.Send(Emails.PairingInitiator, unpaired, pairing.World.BackupUser);
                        emails.Send(Emails.PairingReceiver, unpaired, pairing.World.BackupUser);
                    }
                    else
                    {
                        emails.Send(Emails.UnpairedMember, unpaired);
                    }
                }

                pairing.IsComplete = true;

                await handle.Finish();
            }
        }

        private static bool CanBePaired(Member member)
        {
            return member.HasEmailValidation && member.HasPrivacyValidation && !member.HasLeft;
        }
    }
}