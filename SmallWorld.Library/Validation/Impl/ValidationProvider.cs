using System;
using System.Collections.Generic;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Helpers;

namespace SmallWorld.Library.Validation.Impl
{
    public class ValidationProvider : IValidationProvider
    {
        public IServiceProvider ServiceProvider { get; }

        private readonly ValidationModelBuilder modelBuilder;
        private readonly HashSet<object> validated = new HashSet<object>();

        public ValidationProvider(IServiceProvider provider, ValidationModelBuilder builder)
        {
            ServiceProvider = provider;
            modelBuilder = builder;
        }

        protected virtual IValidationResult ValidateImpl<T>(IValidationTarget<T> target)
        {
            if (target.Value == null)
                return target.GetResult();

            if (validated.Contains(target.Value))
                return target.GetResult();

            validated.Add(target.Value);

            var model = modelBuilder.GetModel<T>();
            model.Validate(this, target);

            return target.GetResult();
        }

        public IValidationResult Validate<T>(T value)
        {
            var target = new ValidationTarget<T>(value);

            return ValidateImpl(target);
        }

        public IValidationResult Validate<T, TChild>(IValidationTarget<T> context, TChild value)
        {
            var target = context.Child(value);

            return ValidateImpl(target);
        }

        public IValidationResult Validate<T, TChild>(IValidationTarget<T> context, string name, TChild value)
        {
            var target = context.Child(name, value);

            return ValidateImpl(target);
        }
    }
}
