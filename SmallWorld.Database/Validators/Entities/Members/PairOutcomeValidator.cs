using SmallWorld.Database.Entities;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.Members
{
    [TypeValidator]
    public class PairOutcomeValidator : Validator<PairOutcome>
    {
        protected override bool Validate(IValidationTarget<PairOutcome> target)
        {
            if (target.Value == PairOutcome.ERROR)
                return target.Error("Invalid pair outcome");

            return true;
        }
    }
}
