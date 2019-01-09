using System.ComponentModel.DataAnnotations.Schema;
using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class Identity : BaseEntity
    {
        [Column("FirstName")]
        public string RawFirstName { get; set; }
        [Column("LastName")]
        public string RawLastName { get; set; }
        [Column("Email")]
        public string RawEmail { get; set; }

        [Required]
        [NotMapped]
        public Name FirstName
        {
            get => WrapName(RawFirstName);
            set => RawFirstName = Unwrap(value);
        }

        [Required]
        [NotMapped]
        public Name LastName
        {
            get => WrapName(RawLastName);
            set => RawLastName = Unwrap(value);
        }

        [Required]
        [NotMapped]
        public EmailAddress Email
        {
            get => WrapEmailAddress(RawEmail);
            set => RawEmail = Unwrap(value);
        }

        public Name FullName()
        {
            return new Name($"{FirstName} {LastName}");
        }
    }
}