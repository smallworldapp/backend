using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Helpers;
using SmallWorld.Library.Validation.Impl;
using SmallWorld.Library.Validators;

namespace SmallWorld.Library.Validation
{
    public static class ValidationExtensions
    {
        public static IServiceCollection AddValidation(this IServiceCollection services)
        {
            services.AddSingleton<ValidationModelBuilder>();
            services.AddScoped<IValidationProvider, ValidationProvider>();

            services.AddScoped(typeof(RequiredValidator<>));

            var provider = new ValidatorProvider();
            provider.AddTypeValidator(typeof(IEnumerable<>), typeof(EnumerableValidator<>));
            services.AddSingleton<IValidatorProvider>(provider);

            return services;
        }
//        public static IValidationResult Validate<T, TValidator>(this IValidationProvider provider, T value) where TValidator : IValidator<T>
//        {
//            return provider.Validate(value, typeof(TValidator));
//        }
//
//        public static IValidationResult Validate<T, TChild, TValidator>(this IValidationProvider provider, IValidationTarget<T> context, TChild value) where TValidator : IValidator<TChild>
//        {
//            return provider.Validate(context, value, typeof(TValidator));
//        }
//
//        public static IValidationResult Validate<T, TChild, TValidator>(this IValidationProvider provider, IValidationTarget<T> context, string name, TChild value) where TValidator : IValidator<TChild>
//        {
//            return provider.Validate(context, name, value, typeof(TValidator));
//        }
    }
}
