using System;
using Newtonsoft.Json;

namespace SmallWorld.Database.Entities
{
    public class PairingSettings : BaseEntity
    {
        public DateTime Start { get; set; }

        public long Period { get; set; }

        public bool Enabled { get; set; }

        public DateTime MostRecent { get; set; }

        [JsonIgnore]
        public World World { get; set; }
    }
}