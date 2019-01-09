using SmallWorld.Database.Entities;

// ReSharper disable ArrangeAccessorOwnerBody
// ReSharper disable InconsistentNaming

namespace SmallWorld.Converters
{
    public class IdentityConverter : EntityConverter<Identity>
    {
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
    }
}