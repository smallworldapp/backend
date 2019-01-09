using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class Member : Identity
    {
        [Required]
        public Guid JoinToken { get; set; }
        [Required]
        public Guid LeaveToken { get; set; }

        public bool OptOut { get; set; }

        public bool HasLeft { get; set; }
        public bool HasEmailValidation { get; set; }
        public bool HasPrivacyValidation { get; set; }

        [Column("Status")]
        public MembershipStatus _Deprecated_Status { get; set; }

        [ValidationIgnore]
        public ICollection<Pair> Pairs1 { get; set; }

        [ValidationIgnore]
        public ICollection<Pair> Pairs2 { get; set; }

        [Required]
        public World World { get; set; }

        public IEnumerable<Pair> AllPairs()
        {
            return Pairs1.Concat(Pairs2);
        }

        public IEnumerable<int> Paired()
        {
            return AllPairs().Select(p => p.Initiator.Id == Id ? p.Receiver.Id : p.Initiator.Id);
        }
    }

    [Obsolete]
    public enum MembershipStatus
    {
        ERROR,

        Unconfirmed,
        Confirmed,
        Left,

        _Deprecated_Removed
    }
}