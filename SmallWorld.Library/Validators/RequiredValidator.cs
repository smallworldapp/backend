using System;
using System.Collections.Generic;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Library.Validators
{
    public class RequiredAttribute : ValidationAttribute
    {
        public override Type Validator => typeof(RequiredValidator<>);
    }

    public class RequiredValidator<T> : Validator<T>
    {
        protected override bool Validate(IValidationTarget<T> target)
        {
            if (EqualityComparer<T>.Default.Equals(target.Value, default(T)))
                return target.Error("Object is null");

            return true;
        }
    }
}
