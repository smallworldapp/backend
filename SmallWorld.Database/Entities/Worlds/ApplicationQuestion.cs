using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class ApplicationQuestion : BaseEntity
    {
        [Required]
        public string Question { get; set; }

        public string Subtext { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}