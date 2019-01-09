using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Library.Validation.Abstractions;

namespace SmallWorld.Library.Validation.Helpers
{
    public abstract class MemberValidator<T>
    {
        public PropertyInfo Property { get; }
        public IEnumerable<Type> Validators { get; }

        protected bool IsNavigation { get; }

        protected MemberValidator(PropertyInfo property, IEnumerable<Type> validators, bool isNavigation)
        {
            Property = property;
            Validators = validators.ToList();
            IsNavigation = isNavigation;
        }

        public bool HasValidators() => Validators.Any();

        public abstract void Trim();
        public abstract bool IsEffective { get; }

        public abstract void Validate(IValidationProvider validation, IValidationTarget<T> instance);
    }

    public class MemberValidator<T, TProperty> : MemberValidator<T>
    {
        private ValidationModel<TProperty> model;

        public MemberValidator(PropertyInfo property, ValidationModel<TProperty> model, IEnumerable<Type> validators, bool isNavigation)
            : base(property, validators, isNavigation)
        {
            this.model = model;
        }

        private bool? isEffective;

        public override bool IsEffective => isEffective ?? (isEffective = GetEffectiveness()).Value;

        private bool GetEffectiveness()
        {
            if (Validators.Any())
                return true;

            if (model == null)
                return false;

            return model.IsEffective;
        }

        public override void Trim()
        {
            if (model == null) return;

            if (model.IsEffective)
                model.Trim();
            else
                model = null;
        }

        public override void Validate(IValidationProvider validation, IValidationTarget<T> instance)
        {
            if (IsNavigation && instance.Value is object entity)
            {
                var db = validation.ServiceProvider.GetService<DbContext>();
                var entry = db?.Entry(entity);
                if (entry != null && entry.State != EntityState.Detached)
                    entry.Navigation(Property.Name)?.Load();
            }

            var value = GetValue(instance.Value);
            var target = instance.Child(Property.Name, value);

            foreach (var validatorType in Validators)
            {
                var type = validatorType.IsGenericType ? validatorType.MakeGenericType(typeof(TProperty)) : validatorType;
                var validator = (IValidator<TProperty>)validation.ServiceProvider.GetRequiredService(type);

                validator.Validate(target);
                if (!target.Continue || !instance.Continue) return;
            }

            if (model != null)
            {
                model.Validate(validation, target);
            }
        }

        private TProperty GetValue(T instance)
        {
            var isIndex = Property.GetIndexParameters().Length != 0;
            return (TProperty)Property.GetValue(instance);
        }
    }
}