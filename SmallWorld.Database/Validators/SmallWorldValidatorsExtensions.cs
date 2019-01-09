using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Validators.Entities;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators
{
    public static class SmallWorldValidatorsExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            var ass = typeof(EntityValidator).GetTypeInfo().Assembly;
            services.AddSingleton<IValidatorProvider>(new AssemblyValidatorProvider(ass));

            return services;
        }
    }
}
