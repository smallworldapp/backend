using System;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models;

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable InconsistentNaming

namespace SmallWorld.Converters.Members
{
    public class MemberStatus
    {
        public bool left { get; set; }
        public bool email { get; set; }
        public bool privacy { get; set; }
    }

    public class MemberConverter : EntityConverter<Member>
    {
        public override void Load(IEntry<Member> entry, IEntryRepository context)
        {
            entry.LoadRelations(m => m.World);
        }

        public MemberStatus status
        {
            get => new MemberStatus {
                left = Value.HasLeft,
                email = Value.HasEmailValidation,
                privacy = Value.HasPrivacyValidation,
            };

            set
            {
                Value.HasLeft = value.left;
                Value.HasEmailValidation = value.email;
                Value.HasPrivacyValidation = value.privacy;
            }
        }

        public Name firstName
        {
            get => Value.FirstName;
            set => Value.FirstName = value;
        }

        public Name lastName
        {
            get => Value.LastName;
            set => Value.LastName = value;
        }

        public EmailAddress email
        {
            get => Value.Email;
            set => Value.Email = value;
        }

        public Guid worldId
        {
            get => Value.World.Guid;
        }
    }
}
