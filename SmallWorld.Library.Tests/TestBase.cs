using Microsoft.Extensions.DependencyInjection;

namespace SmallWorld.Library.Tests
{
    public abstract class TestBase
    {
        protected virtual void AddServices(IServiceCollection services) { }

        protected ServiceProvider CreateProvider()
        {
            var services = new ServiceCollection();

            AddServices(services);
            return services.BuildServiceProvider();
        }
    }
}
