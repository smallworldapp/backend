using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class Credentials : BaseEntity
    {
        [Required]
        public byte[] Hash { get; set; }

        [Required]
        public byte[] Salt { get; set; }
    }
}