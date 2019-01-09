using System;
using System.Diagnostics;
using SmallWorld.Library.CustomTypes;

namespace SmallWorld.Database.Entities
{
    public class Name : StringType<Name>
    {
        public Name(string value) : base(value, StringComparison.OrdinalIgnoreCase) { }

        protected override Name Create(string value, StringComparison comparison)
        {
            Debug.Assert(comparison == StringComparison.OrdinalIgnoreCase);
            return new Name(value);
        }
    }
}
