using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class Account : BaseEntity
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

        [Column("Email")]
        public string RawEmail { get; set; }

        [Required]
        [NotMapped]
        public EmailAddress Email
        {
            get => WrapEmailAddress(RawEmail);
            set => RawEmail = Unwrap(value);
        }

        public AccountStatus Status { get; set; }

        public AccountType Type { get; set; }

        public ResetToken ResetToken { get; set; }

        [Required]
        public Credentials Credentials { get; set; }

        [ValidationIgnore]
        public ICollection<World> Worlds { get; set; }
    }

    public enum AccountType
    {
        ERROR = 0,
        Standard = 1,
        Research = 2,
        Conference = 3
    }

    public enum AccountStatus
    {
        ERROR = 0,
        Default = 1,
        New = 2,
        Deleted = 3
    }
}