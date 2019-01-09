using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SmallWorld.Library.Validation.Abstractions;

namespace SmallWorld.Library.Validation.Impl
{
    public class AssemblyValidatorProvider : ValidatorProvider
    {
        public AssemblyValidatorProvider(Assembly assembly)
        {
            foreach (var tuple in GetTypes(assembly))
            {
                AddTypeValidator(tuple.type, tuple.validator);
            }
        }

        private static readonly IDictionary<Assembly, List<(Type, Type)>> cache = new ConcurrentDictionary<Assembly, List<(Type, Type)>>();

        private static List<(Type type, Type validator)> GetTypes(Assembly ass)
        {
            if (!cache.TryGetValue(ass, out var list))
            {
                list = new List<(Type, Type)>();

                var specific = from type in ass.GetLoadableDefinedTypes()
                               where !type.IsAbstract && !type.IsInterface
                               where type.GetCustomAttribute<TypeValidatorAttribute>() != null
                               select type;

                foreach (var type in specific)
                {
                    var validatorType = type.GetGenericSuperType(typeof(IValidator<>));
                    if (validatorType == null) continue;

                    var t = validatorType.GetGenericArguments()[0];

                    if (t.IsGenericType)
                        t = t.GetGenericTypeDefinition();

                    list.Add((t, type));
                }

                cache[ass] = list;
            }

            return list;
        }
    }
}
