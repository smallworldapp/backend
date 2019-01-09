using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Library.Model.Impl;
using Xunit;

namespace SmallWorld.Library.Tests.Model.Impl
{
    public class ContextLockTest : TestBase
    {
        protected override void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IContext, FakeContext>();
            services.AddSingleton<IContextLock, ContextLock>();
        }

        [Fact]
        public async Task Write_Finish()
        {
            using (var provider = CreateProvider())
            using (var iContext = provider.GetService<IContext>())
            {
                var context = (FakeContext)iContext;
                var access = provider.GetService<IContextLock>();
                var handle = await access.Write();

                Assert.True(context.IsWriting);

                await handle.Finish();

                Assert.Null(context.IsWriting);
                Assert.False(context.IsInvalidated);
            }
        }

        [Fact]
        public async Task Write_Dispose()
        {
            using (var provider = CreateProvider())
            using (var iContext = provider.GetService<IContext>())
            {
                var context = (FakeContext)iContext;
                var access = provider.GetService<IContextLock>();
                var handle = await access.Write();

                Assert.True(context.IsWriting);

                handle.Dispose();

                Assert.Null(context.IsWriting);
                Assert.True(context.IsInvalidated);
            }
        }

        [Fact]
        public async Task Read()
        {
            using (var provider = CreateProvider())
            using (var iContext = provider.GetService<IContext>())
            {
                var context = (FakeContext)iContext;
                var access = provider.GetService<IContextLock>();
                var handle = await access.Read();

                Assert.False(context.IsWriting);

                handle.Dispose();

                Assert.Null(context.IsWriting);
                Assert.False(context.IsInvalidated);
            }
        }
    }
}
