using System.ComponentModel;
using System.Threading.Tasks;
using SmallWorld.Database.Entities;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models.Emailing;

namespace SmallWorld.Models.PairGenerators
{
    public class ManualGeneratedPairs : GeneratedPairs
    {
        private readonly IEntryRepository entries;
        private readonly EmailProvider emails;

        private Pairing pairing;

        public ManualGeneratedPairs(IEntryRepository entries, EmailProvider emails)
        {
            this.entries = entries;
            this.emails = emails;
        }

        public override void Generate(Pairing inPairing)
        {
            pairing = inPairing;
        }

        public override async Task Submit(IContextLock access)
        {
            if (pairing.Type != PairingType.Manual)
                throw new InvalidEnumArgumentException("CreateManualPairs: pairing");

            using (var handle = await access.Write())
            {
                entries.Entry(pairing)
                    .LoadRelations(p => p.Pairs)
                    .LoadRelations(p => p.World.Members, m => m.Pairs1)
                    .LoadRelations(p => p.World.Members, m => m.Pairs2)
                    .LoadRelations(p => p.World.Pairs)
                    .LoadRelations(p => p.World.BackupUser);

                foreach (var pair in pairing.Pairs)
                {
                    emails.Send(Emails.PairingReceiver, pair);
                    emails.Send(Emails.PairingInitiator, pair);
                }

                pairing.IsComplete = true;

                await handle.Finish();
            }
        }
    }
}

