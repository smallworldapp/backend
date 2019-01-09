using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace SmallWorld.Library.CustomTypes
{
    public static class CustomTypeExtensions
    {
        public static IServiceCollection AddCustomTypeConverters(this IServiceCollection services)
        {
            services.AddTransient<JsonConverter, StringTypeConverter>();

            return services;
        }
    }
}
