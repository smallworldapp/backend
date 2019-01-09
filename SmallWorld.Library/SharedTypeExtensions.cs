using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SmallWorld.Library
{
    public static class SharedTypeExtensions
    {
        public static IEnumerable<TypeInfo> FindTypes(this Type type, Assembly ass = null)
        {
            ass = ass ?? type.GetTypeInfo().Assembly;

            if (type.IsGenericTypeDefinition)
                return from subType in ass.GetConstructableTypes()
                       where type.IsGenericAssignableFrom(subType)
                       select subType;

            return from subType in ass.GetConstructableTypes()
                   where type.IsAssignableFrom(subType)
                   select subType;
        }

        public static Type GetGenericSuperType(this Type type, Type genericType)
        {
            if (!genericType.IsGenericTypeDefinition)
                throw new ArgumentException(nameof(genericType));

            if (type == typeof(object))
                return null;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == genericType)
                return type;

            if (!genericType.IsInterface)
                return type.BaseType.GetGenericSuperType(genericType);

            foreach (var i in type.GetInterfaces())
            {
                var check = i.GetGenericSuperType(genericType);
                if (check != null) return check;
            }

            return null;
        }

        public static bool IsGenericAssignableFrom(this Type type, Type subType)
        {
            return subType.GetGenericSuperType(type) != null;
        }

        public static IEnumerable<TypeInfo> GetConstructableTypes(this Assembly assembly)
            => assembly.GetLoadableDefinedTypes().Where(
                t => !t.IsAbstract &&
                     !t.IsInterface &&
                     !t.IsGenericTypeDefinition);

        public static IEnumerable<TypeInfo> GetLoadableDefinedTypes(this Assembly assembly)
        {
            try
            {
                return assembly.DefinedTypes;
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null).Select(IntrospectionExtensions.GetTypeInfo);
            }
        }
    }
}
