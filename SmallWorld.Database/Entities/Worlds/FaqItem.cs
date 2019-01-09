using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class FaqItem : BaseEntity
    {
        [Required]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }
    }
}