using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Library.Model;
using SmallWorld.Library.Model.Abstractions;
using Xunit;

namespace SmallWorld.Library.Tests.Model
{
    public class ModelsExtensionsTest : TestBase
    {
        protected override void AddServices(IServiceCollection services)
        {
            services.AddScoped<IContext, FakeContext>();
            services.AddModel();
        }

        [Fact]
        public void AddModels()
        {
            using (var provider = CreateProvider())
            using (var scope1 = provider.CreateScope())
            using (var scope2 = provider.CreateScope())
            {
                var access1A = scope1.ServiceProvider.GetService<IContextLock>();
                var entries1A = scope1.ServiceProvider.GetService<IEntryRepository>();

                var access1B = scope1.ServiceProvider.GetService<IContextLock>();
                var entries1B = scope1.ServiceProvider.GetService<IEntryRepository>();

                var access2 = scope2.ServiceProvider.GetService<IContextLock>();
                var entries2 = scope2.ServiceProvider.GetService<IEntryRepository>();

                Assert.NotNull(access1A);
                Assert.NotNull(entries1A);

                Assert.NotNull(access1B);
                Assert.NotNull(entries1B);

                Assert.Same(access1A, access1B);
                Assert.Same(entries1A, entries1B);

                Assert.NotNull(access2);
                Assert.NotNull(entries2);

                Assert.NotSame(access1A, access2);
                Assert.NotSame(entries1A, entries2);
            }
        }
    }
}
