using SmallWorld.Database.Entities;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.Accounts
{
    [TypeValidator]
    public class AccountTypeValidator : Validator<AccountType>
    {
        protected override bool Validate(IValidationTarget<AccountType> target)
        {
            if (target.Value == AccountType.ERROR)
                return target.Error("Invalid account type");

            return true;
        }
    }
}
