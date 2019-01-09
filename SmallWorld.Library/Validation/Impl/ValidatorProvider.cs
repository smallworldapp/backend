using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SmallWorld.Library.Validation.Abstractions;

namespace SmallWorld.Library.Validation.Impl
{
    public class ValidatorProvider : IValidatorProvider
    {
        private readonly IDictionary<Type, Type> lookup = new ConcurrentDictionary<Type, Type>();

        public void AddTypeValidator(Type target, Type validatorType)
        {
            Debug.Assert(target.IsGenericType || typeof(IValidator<>).MakeGenericType(target).IsAssignableFrom(validatorType));

            lookup.Add(target, validatorType);
        }

        public IEnumerable<Type> GetValidators(Type targetType)
        {
            var validators = from pair in lookup
                             where pair.Key.IsAssignableFrom(targetType)
                             select pair.Value;

            if (targetType.IsGenericType)
            {
                var gen = targetType.GetGenericTypeDefinition();
                var args = targetType.GetGenericArguments();

                var generic = from pair in lookup
                              where pair.Key.IsGenericTypeDefinition && pair.Key.IsGenericAssignableFrom(gen)
                              select pair.Value.MakeGenericType(args);

                validators = validators.Concat(generic);
            }

            return validators;
        }
    }
}
