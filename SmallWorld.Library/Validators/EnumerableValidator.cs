using System.Collections.Generic;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Library.Validators
{
    [TypeValidator]
    public class EnumerableValidator<T> : Validator<IEnumerable<T>>
    {
        private readonly IValidationProvider validation;

        public EnumerableValidator(IValidationProvider validation)
        {
            this.validation = validation;
        }

        protected override bool Validate(IValidationTarget<IEnumerable<T>> target)
        {
            foreach (var item in target.Value)
            {
                validation.Validate(target, item);
                if (!target.Continue)
                    return true;
            }

            return true;
        }
    }
}