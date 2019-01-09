using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class World : BaseEntity
    {
        [Required]
        public Account Account { get; set; }

        [Column("Name")]
        public string RawName { get; set; }

        [Required]
        [NotMapped]
        public Name Name
        {
            get => WrapName(RawName);
            set => RawName = Unwrap(value);
        }

        [Column("Identifier")]
        public string RawIdentifier { get; set; }

        [NotMapped]
        public Identifier Identifier
        {
            get => WrapIdentifier(RawIdentifier);
            set => RawIdentifier = Unwrap(value);
        }

        public WorldStatus Status { get; set; }

        public WorldPrivacy Privacy { get; set; }

        public Identity BackupUser { get; set; }

        public PairingSettings PairingSettings { get; set; }

        [Required]
        public Application Application { get; set; }

        [Required]
        public Description Description { get; set; }

        [ValidationIgnore]
        public ICollection<Member> Members { get; set; }

        [ValidationIgnore]
        public ICollection<Pair> Pairs { get; set; }

        [ValidationIgnore]
        public ICollection<Pairing> Pairings { get; set; }
    }

    public enum WorldPrivacy
    {
        ERROR,

        InviteOnly,
        VerificationRequired,
        Public,
    }

    public enum WorldStatus
    {
        ERROR,

        Applying,
        Pending,
        Rejected,
        Passed,
        Deleted
    }
}