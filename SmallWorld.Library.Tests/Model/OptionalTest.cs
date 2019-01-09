using SmallWorld.Library.Model;
using Xunit;

namespace SmallWorld.Library.Tests.Model
{
    public class OptionalTest : TestBase
    {
        [Theory]
        [InlineData(null)]
        [InlineData("123")]
        public void NotPresent(string value)
        {
            var opt = new Optional<string>(false, value);

            Assert.False(opt.HasValue);
            Assert.False(opt.Exists(out var value2));

            Assert.Same(value, value2);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("123")]
        public void Present(string value)
        {
            var opt = new Optional<string>(true, value);

            Assert.True(opt.HasValue);
            Assert.True(opt.Exists(out var value2));

            Assert.Same(value, value2);
        }
    }
}
