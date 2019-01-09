using System.Linq;
using SmallWorld.Database.Entities;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.CustomTypes
{
    [TypeValidator]
    public class IdentifierValidator : Validator<Identifier>
    {
        protected override bool Validate(IValidationTarget<Identifier> target)
        {
            if (string.IsNullOrWhiteSpace(target.Value.Value))
                return target.Error("Invalid identifer");

            if (target.Value.Trim() != target.Value)
                return target.Error("Invalid identifier");

            if (target.Value.Trim('-') != target.Value)
                return target.Error("Invalid identifier");

            if (target.Value.Length > 100)
                return target.Error("Identifier is too long");

            if (!target.Value.Value.All(c => c == '-' || char.IsLower(c) || char.IsDigit(c)))
                return target.Error("Invalid identifier");

            return true;
        }
    }
}
