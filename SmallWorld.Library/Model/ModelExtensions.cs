using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Library.Model.Impl;

namespace SmallWorld.Library.Model
{
    public static class ModelExtensions
    {
        public static IServiceCollection AddModel(this IServiceCollection services)
        {
            services.AddScoped<IContextLock, ContextLock>();
            services.AddScoped<IEntryRepository, EntryRepository>();

            return services;
        }
    }
}
