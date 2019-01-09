using System;
using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class ResetToken : BaseEntity
    {
        [Required]
        public DateTime Expiration { get; set; }
    }
}