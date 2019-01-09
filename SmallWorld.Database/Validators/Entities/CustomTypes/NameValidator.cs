using System.Linq;
using SmallWorld.Database.Entities;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.CustomTypes
{
    [TypeValidator]
    public class NameValidator : Validator<Name>
    {
        protected override bool Validate(IValidationTarget<Name> target)
        {
            if (string.IsNullOrWhiteSpace(target.Value.Value))
                return target.Error("Invalid name");

            if (target.Value.Trim() != target.Value)
                return target.Error("Invalid name");

            if (target.Value.Length > 100)
                return target.Error("Name is too long");

            if (target.Value.Value.Any(c => char.IsWhiteSpace(c) && c != ' '))
                return target.Error("Invalid characters in name");

            return true;
        }
    }
}
