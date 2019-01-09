using System;
using SmallWorld.Library.CustomTypes;

namespace SmallWorld.Library.Tests.CustomTypes
{
    public class FakeStringType : StringType<FakeStringType>
    {
        public FakeStringType(string value) : this(value, StringComparison.Ordinal) { }
        public FakeStringType(string value, StringComparison comparison) : base(value, comparison) { }

        protected override FakeStringType Create(string value, StringComparison comparison) => new FakeStringType(value, comparison);
    }
}
