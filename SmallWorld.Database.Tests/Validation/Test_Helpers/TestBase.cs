using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace SmallWorld.Database.Tests.Validation.Test_Helpers
{
    public abstract class TestBase
    {
        protected virtual void AddServices(IServiceCollection services) { }
        protected virtual Task Initialize(IServiceProvider provider) => Task.CompletedTask;

        protected async Task<ServiceProvider> CreateProvider()
        {
            var services = new ServiceCollection();

            AddServices(services);

            var provider = services.BuildServiceProvider();

            await Initialize(provider);

            return provider;
        }
    }

    //    public class ValidatorTestBase2<T> : TestBase
    //    {
    //        [Fact]
    //        public async Task Valid()
    //        {
    //            using (var provider = CreateProvider())
    //            using (var scope = provider.CreateScope())
    //            {
    //                var access = scope.ServiceProvider.GetService<IContextLock>();
    //                var validation = scope.ServiceProvider.GetService<IValidationContext>();
    //
    //                using (await access.Read())
    //                {
    //                    foreach (var value in Factory.Valid())
    //                    {
    //                        Assert.True(validation.Validate(value, out var _));
    //                    }
    //                }
    //            }
    //        }
    //
    //        [Fact]
    //        public async Task Invalid()
    //        {
    //            using (var provider = CreateProvider())
    //            using (var scope = provider.CreateScope())
    //            {
    //                var access = scope.ServiceProvider.GetService<IContextLock>();
    //                var validation = scope.ServiceProvider.GetService<IValidationContext>();
    //
    //                using (await access.Read())
    //                {
    //                    foreach (var value in Factory.Invalid())
    //                    {
    //                        Assert.False(validation.Validate(value, out var _));
    //                    }
    //                }
    //            }
    //        }
    //
    //
    //        protected virtual Factory<T> Factory { get; }
    //    }
    //
    //    public abstract class EntityValidatorTestBase : TestBase
    //    {
    //        [Fact]
    //        public abstract Task InvalidBaseEntity();
    //    }
}
