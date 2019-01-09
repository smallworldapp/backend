using System.ComponentModel.DataAnnotations.Schema;
using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class EmailRecipient : BaseEntity
    {
        [Column("Name")]
        public string RawName { get; set; }

        [Required]
        [NotMapped]
        public Name Name
        {
            get => WrapName(RawName);
            set => RawName = Unwrap(value);
        }

        [Column("Address")]
        public string RawAddress { get; set; }

        [Required]
        [NotMapped]
        public EmailAddress Address
        {
            get => WrapEmailAddress(RawAddress);
            set => RawAddress = Unwrap(value);
        }
    }
}
