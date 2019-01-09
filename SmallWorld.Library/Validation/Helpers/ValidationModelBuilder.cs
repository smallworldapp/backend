using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validators;

namespace SmallWorld.Library.Validation.Helpers
{
    public class ValidationModelBuilder
    {
        private readonly IDictionary<Type, object> modelCache = new ConcurrentDictionary<Type, object>();

        private readonly IServiceProvider provider;
        private readonly IEnumerable<IValidatorProvider> providers;

        public ValidationModelBuilder(IEnumerable<IValidatorProvider> providers, IServiceProvider provider)
        {
            this.providers = providers;
            this.provider = provider;
        }

        public ValidationModel<T> GetModel<T>()
        {
            if (!modelCache.TryGetValue(typeof(T), out var raw))
                raw = MakeModel<T>();

            var model = (ValidationModel<T>)raw;

            model.Trim();

            return model;
        }

        private object GetModel(Type target)
        {
            var makeModel = typeof(ValidationModelBuilder).GetMethod(nameof(MakeModel), BindingFlags.Instance | BindingFlags.NonPublic);
            var generic = makeModel.MakeGenericMethod(target);

            if (!modelCache.TryGetValue(target, out var model))
                model = generic.Invoke(this, new object[0]);

            return model;
        }

        private ValidationModel<T> MakeModel<T>()
        {
            var model = new ValidationModel<T>();
            modelCache[typeof(T)] = model;

            var typeValidators = from provider in providers
                                 from type in provider.GetValidators(typeof(T))
                                 select new TypeValidator<T>(type);

            foreach (var typeValidator in typeValidators)
                model.AddValidator(typeValidator);

            var properties = typeof(T).GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Instance);
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<ValidationIgnoreAttribute>() != null)
                    continue;

                var memberModel = GetModel(property.PropertyType);
                var validators = property.GetCustomAttributes<ValidationAttribute>()
                    .Select(a => a.Validator)
                    .ToList();

                var db = provider.GetService<DbContext>();
                var isNavigation = false;

                var entry = db?.Model.FindEntityType(typeof(T));
                if (entry != null)
                {
                    var navs = entry.GetNavigations();
                    isNavigation = navs.Any(n => n.PropertyInfo == property);
                }

                var memberType = typeof(MemberValidator<,>).MakeGenericType(typeof(T), property.PropertyType);
                var member = Activator.CreateInstance(memberType, property, memberModel, validators, isNavigation);
                model.AddValidator((MemberValidator<T>)member);
            }

            return model;
        }
    }
}
