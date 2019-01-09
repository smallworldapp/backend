using System;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models;

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable InconsistentNaming

namespace SmallWorld.Converters.Members
{
    public class PairConverter : EntityConverter<Pair>
    {
        public override void Load(IEntry<Pair> entry, IEntryRepository context)
        {
            entry.LoadRelations(p => p.Initiator);
            entry.LoadRelations(p => p.Receiver);

            entry.LoadRelations(p => p.Pairing);
            entry.LoadRelations(p => p.World);
        }

        public Guid initiatorId
        {
            get => Value.Initiator.Guid;
        }

        public Guid receiverId
        {
            get => Value.Receiver.Guid;
        }

        public PairOutcome outcome
        {
            get => Value.Outcome;
        }

        public DateTime date
        {
            get => Value.Pairing.Date;
        }

        public string message
        {
            get => Value.Pairing.Message;
        }

        public Guid pairingId
        {
            get => Value.Pairing.Guid;
        }

        public Guid worldId
        {
            get => Value.World.Guid;
        }
    }
}
