using System;
using System.Collections.Generic;
using SmallWorld.Library.Validators;

namespace SmallWorld.Database.Entities
{
    public class Email : BaseEntity
    {
        [Required]
        public DateTime Created { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        [Required]
        public ICollection<EmailRecipient> Recipients { get; set; }

        public bool IsSent { get; set; }
        public DateTime? Sent { get; set; }
    }
}
