using System;
using System.Diagnostics;
using SmallWorld.Library.CustomTypes;

namespace SmallWorld.Database.Entities
{
    public class EmailAddress : StringType<EmailAddress>
    {
        public EmailAddress(string value) : base(value, StringComparison.OrdinalIgnoreCase) { }

        protected override EmailAddress Create(string value, StringComparison comparison)
        {
            Debug.Assert(comparison == StringComparison.OrdinalIgnoreCase);
            return new EmailAddress(value);
        }
    }
}
