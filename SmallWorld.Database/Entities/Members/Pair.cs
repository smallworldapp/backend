using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class Pair : BaseEntity
    {
        public PairOutcome Outcome { get; set; }

        [Required]
        public World World { get; set; }

        [Required]
        public Pairing Pairing { get; set; }

        [Required]
        public Member Initiator { get; set; }

        [Required]
        public Member Receiver { get; set; }
    }

    public enum PairOutcome
    {
        ERROR,
        Unknown,
        Success,
        Failure
    }
}