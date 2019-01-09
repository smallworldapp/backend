using System;
using System.Diagnostics;
using SmallWorld.Library.CustomTypes;

namespace SmallWorld.Database.Entities
{
    public class Identifier : StringType<Identifier>
    {
        public Identifier(string value) : base(value, StringComparison.OrdinalIgnoreCase) { }

        protected override Identifier Create(string value, StringComparison comparison)
        {
            Debug.Assert(comparison == StringComparison.OrdinalIgnoreCase);
            return new Identifier(value);
        }
    }
}
