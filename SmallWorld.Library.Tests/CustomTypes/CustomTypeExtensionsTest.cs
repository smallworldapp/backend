using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SmallWorld.Library.CustomTypes;
using Xunit;

namespace SmallWorld.Library.Tests.CustomTypes
{
    public class CustomTypeExtensionsTest : TestBase
    {
        protected override void AddServices(IServiceCollection services)
        {
            services.AddCustomTypeConverters();
        }

        [Fact]
        public void AddCustomTypeConverters()
        {
            using (var provider = CreateProvider())
            {
                var one = provider.GetServices<JsonConverter>();
                var two = provider.GetServices<JsonConverter>();

                Assert.Single(one);
                Assert.Single(two);

                Assert.NotSame(one.Single(), two.Single());
            }
        }
    }
}
