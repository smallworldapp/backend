using Xunit;

namespace SmallWorld.Library.Tests.CustomTypes
{
    public class StringTypeTest
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("27")]
        [InlineData("long string")]
        public void EqualityOperators_True(string contents)
        {
            var str1 = new FakeStringType(contents);
            var str2 = new FakeStringType(contents);

            Assert.True(str1 == str2);
            Assert.False(str1 != str2);
            Assert.True(str2 == str1);
            Assert.False(str2 != str1);

            Assert.True(str1.Equals(str2));
            Assert.True(str2.Equals(str1));
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", null)]
        [InlineData("long string", "different string")]
        public void EqualityOperators_False(string a, string b)
        {
            var str1 = new FakeStringType(a);
            var str2 = new FakeStringType(b);

            Assert.False(str1 == str2);
            Assert.True(str1 != str2);
            Assert.False(str2 == str1);
            Assert.True(str2 != str1);

            Assert.False(str1.Equals(str2));
            Assert.False(str2.Equals(str1));
        }

        // ReSharper disable once HeuristicUnreachableCode
        [Fact]
        public void EqualityOperators_NullValues()
        {
            var str1 = new FakeStringType("");
            var str2 = new FakeStringType(null);
            var str3 = (FakeStringType)null;

            Assert.False(str1 == str2);
            Assert.True(str1 != str2);
            Assert.False(str2 == str1);
            Assert.True(str2 != str1);

            Assert.False(str1.Equals(str2));
            Assert.False(str2.Equals(str1));

            Assert.False(str1 == str3);
            Assert.True(str1 != str3);
            Assert.False(str3 == str1);
            Assert.True(str3 != str1);

            Assert.False(str1.Equals(str3));

            Assert.True(str2 == str3);
            Assert.False(str2 != str3);
            Assert.True(str3 == str2);
            Assert.False(str3 != str2);

            Assert.True(str2.Equals(str3));
        }
    }
}
