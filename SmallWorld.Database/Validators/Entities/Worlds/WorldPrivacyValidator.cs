using SmallWorld.Database.Entities;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.Worlds
{
    [TypeValidator]
    public class WorldPrivacyValidator : Validator<WorldPrivacy>
    {
        protected override bool Validate(IValidationTarget<WorldPrivacy> target)
        {
            if (target.Value == WorldPrivacy.ERROR)
                return target.Error("Invalid pair outcome");

            return true;
        }
    }
}
