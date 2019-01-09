using System;
using System.Collections.Generic;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using SmallWorld.Database.Validators.Entities.Emails;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;
using Xunit;

namespace SmallWorld.Database.Tests.Validators.Entities.Emails
{
    public class EmailValidatorTest : TestBase
    {
        public static IEnumerable<object[]> Validate_Sent_IsSentArgs = new MemberData {
            { false, true, null },
            { false, false, DateTime.UtcNow },
            { true, true, DateTime.UtcNow },
            { true, false, null },
        };

        [Theory]
        [MemberData(nameof(Validate_Sent_IsSentArgs))]
        public void Validate_Sent_IsSent(bool valid, bool isSent, DateTime? sent)
        {
            IValidator<Email> validator = new EmailValidator();

            var email = new Email {
                IsSent = isSent,
                Sent = sent,
                Recipients = new HashSet<EmailRecipient> { new EmailRecipient() },
            };

            var target = new ValidationTarget<Email>(email);
            validator.Validate(target);
            Assert.Equal(valid, !target.GetResult().HasErrors);
        }

        public static IEnumerable<object[]> Validate_RecipientsArgs = new MemberData {
            { false, new EmailRecipient[0] },
            { true, new [] { new EmailRecipient(), } },
        };

        [Theory]
        [MemberData(nameof(Validate_RecipientsArgs))]
        public void Validate_Recipients(bool valid, EmailRecipient[] recipients)
        {
            IValidator<Email> validator = new EmailValidator();

            var email = new Email {
                IsSent = false,
                Sent = null,
                Recipients = new HashSet<EmailRecipient>(recipients)
            };

            var target = new ValidationTarget<Email>(email);
            validator.Validate(target);
            Assert.Equal(valid, !target.GetResult().HasErrors);
        }
    }
}
