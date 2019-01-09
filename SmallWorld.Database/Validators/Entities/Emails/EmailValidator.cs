using System;
using System.Linq;
using SmallWorld.Database.Entities;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.Emails
{
    [TypeValidator]
    public class EmailValidator : Validator<Email>
    {
        protected override bool Validate(IValidationTarget<Email> target)
        {
            if ((target.Value.IsSent == false) != (target.Value.Sent == null))
                return target.Error("Sent and IsSent do not match");

            if (!target.Value.Recipients.Any())
                return target.Error("No recipients");

            return true;
        }
    }
}
