using System;
using System.Linq;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable InconsistentNaming

namespace SmallWorld.Converters.Worlds
{
    public class WorldConverter : EntityConverter<World>
    {
        public override void Load(IEntry<World> entry, IEntryRepository context)
        {
            entry.LoadRelations(w => w.BackupUser);
            entry.LoadRelations(w => w.Account);

            entry.LoadRelations(w => w.Members);
            entry.LoadRelations(w => w.Pairings);
        }

        public Guid accountId
        {
            get => Value.Account.Guid;
        }

        public AccountType accountType
        {
            get => Value.Account.Type;
        }

        public Name name
        {
            get => Value.Name;
            set => Value.Name = value;
        }

        public Identifier identifier
        {
            get => Value.Identifier;
            set => Value.Identifier = value;
        }

        public WorldStatus status
        {
            get => Value.Status;
            set => Value.Status = value;
        }

        public WorldPrivacy privacy
        {
            get => Value.Privacy;
            set => Value.Privacy = value;
        }

        public Identity backupUser
        {
            get => Value.BackupUser;
            set => Value.BackupUser = value;
        }

        public object memberCount
        {
            get => new {
                unconfirmed = Value.Members.Count(m => !m.HasLeft && !m.HasEmailValidation),
                confirmed = Value.Members.Count(m => !m.HasLeft && m.HasEmailValidation && m.HasPrivacyValidation),
                privacy = Value.Members.Count(m => !m.HasLeft && !m.HasPrivacyValidation),
                left = Value.Members.Count(m => m.HasLeft),
            };
        }

        public Pairing nextPairing
        {
            get => Value.Pairings.FirstOrDefault(p => p.Date > DateTime.UtcNow);
        }
    }
}
