using System;
using System.Collections.Generic;
using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class Pairing : BaseEntity
    {
        public bool IsComplete { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public PairingType Type { get; set; }

        public string Message { get; set; }

        [Required]
        public World World { get; set; }

        [ValidationIgnore]
        public ICollection<Pair> Pairs { get; set; }
    }

    public enum PairingType
    {
        ERROR,
        Manual,
        Auto,
    }
}