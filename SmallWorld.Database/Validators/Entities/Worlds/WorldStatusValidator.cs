using SmallWorld.Database.Entities;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.Worlds
{
    [TypeValidator]
    public class WorldStatusValidator : Validator<WorldStatus>
    {
        protected override bool Validate(IValidationTarget<WorldStatus> target)
        {
            if (target.Value == WorldStatus.ERROR)
                return target.Error("Invalid pair outcome");

            return true;
        }
    }
}
