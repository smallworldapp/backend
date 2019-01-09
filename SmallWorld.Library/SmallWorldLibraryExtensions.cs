using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Library.CustomTypes;
using SmallWorld.Library.Model;
using SmallWorld.Library.Validation;

namespace SmallWorld.Library
{
    public static class SmallWorldLibraryExtensions
    {
        public static IServiceCollection AddSmallWorldLibrary(this IServiceCollection services)
        {
            services.AddModel();
            services.AddValidation();
            services.AddCustomTypeConverters();

            return services;
        }
    }
}
