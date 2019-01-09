using System;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Library.Validation.Abstractions;

namespace SmallWorld.Library.Validation.Helpers
{
    public class TypeValidator<T> 
    {
        public Type ValidatorType { get; }

        public TypeValidator(Type validatorType)
        {
            ValidatorType = validatorType;
        }

        public void Validate(IValidationProvider validation, IValidationTarget<T> target)
        {
            var validator = (IValidator<T>)ActivatorUtilities.CreateInstance(validation.ServiceProvider, ValidatorType);

            validator.Validate(target);
        }
    }
}