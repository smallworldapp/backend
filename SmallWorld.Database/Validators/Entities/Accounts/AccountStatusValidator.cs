using SmallWorld.Database.Entities;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.Accounts
{
    [TypeValidator]
    public class AccountStatusValidator : Validator<AccountStatus>
    {
        protected override bool Validate(IValidationTarget<AccountStatus> target)
        {
            if (target.Value == AccountStatus.ERROR)
                return target.Error("Invalid account status");

            return true;
        }
    }
}
