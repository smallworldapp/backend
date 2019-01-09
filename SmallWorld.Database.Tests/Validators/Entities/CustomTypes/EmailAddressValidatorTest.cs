using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using SmallWorld.Database.Validators.Entities.CustomTypes;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;
using Xunit;

namespace SmallWorld.Database.Tests.Validators.Entities.CustomTypes
{
    public class EmailAddressValidatorTest : TestBase
    {
        [Theory]
        [InlineData("test@mfro.me")]
        [InlineData("test@gmail.com")]
        [InlineData("this-email@smallworldapp.org")]
        public void Valid(string value)
        {
            IValidator<EmailAddress> validator = new EmailAddressValidator();

            var target = new ValidationTarget<EmailAddress>(new EmailAddress(value));
            validator.Validate(target);
            Assert.False(target.GetResult().HasErrors);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("not-an-email")]
        [InlineData(" test@mfro.me")]
        [InlineData("test@mfro.me ")]
        [InlineData("test\nemail@mfro.me")]
        [InlineData("thisisaverylongemailthatnoonewouldeverusebecauseitstoolong@mfro.me")]
        public void Invalid(string value)
        {
            IValidator<EmailAddress> validator = new EmailAddressValidator();

            var target = new ValidationTarget<EmailAddress>(new EmailAddress(value));
            validator.Validate(target);
            Assert.True(target.GetResult().HasErrors);
        }
    }
}
