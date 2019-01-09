using System;
using System.Net.Mail;
using SmallWorld.Database.Entities;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Validators.Entities.CustomTypes
{
    [TypeValidator]
    public class EmailAddressValidator : Validator<EmailAddress>
    {
        protected override bool Validate(IValidationTarget<EmailAddress> target)
        {
            if (string.IsNullOrWhiteSpace(target.Value.Value))
                return target.Error("Invalid email address");

            if (target.Value.Trim() != target.Value)
                return target.Error("Invalid email address");

            if (target.Value.Length > 50)
                return target.Error("Email address is too long");

            try
            {
                if (new MailAddress(target.Value.Value).Address != target.Value.Value)
                    return target.Error("Invalid email address");

                return true;
            }
            catch (FormatException)
            {
                return target.Error("Invalid email address");
            }
        }
    }
}
