using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.Accounts
{
    [TypeValidator]
    public class AccountValidator : Validator<Account>
    {
        private readonly IAccountRepository accounts;

        public AccountValidator(IAccountRepository accounts)
        {
            this.accounts = accounts;
        }

        protected override bool Validate(IValidationTarget<Account> target)
        {
            if (accounts.Find(target.Value.Email, out var same) && same != target.Value)
                return target.Error("An account already exists with that email");

            return true;
        }
    }
}
