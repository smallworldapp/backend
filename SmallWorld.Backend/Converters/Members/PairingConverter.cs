using System;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models;

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable InconsistentNaming

namespace SmallWorld.Converters.Members
{
    public class PairingConverter : EntityConverter<Pairing>
    {
        public override void Load(IEntry<Pairing> entry, IEntryRepository context)
        {
            entry.LoadRelations(p => p.World);
        }

        public Guid worldId
        {
            get => Value.World.Guid;
        }

        public DateTime date
        {
            get => Value.Date;
            set => Value.Date = value;
        }

        public string message
        {
            get => Value.Message;
            set => Value.Message = value;
        }

        public PairingType type
        {
            get => Value.Type;
            set => Value.Type= value;
        }
    }
}
