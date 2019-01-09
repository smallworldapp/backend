using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using SmallWorld.Database.Validators.Entities.CustomTypes;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;
using Xunit;

namespace SmallWorld.Database.Tests.Validators.Entities.CustomTypes
{
    public class NameValidatorTest : TestBase
    {
        [Theory]
        [InlineData("Test")]
        [InlineData("Test Account")]
        [InlineData("This is a Test Account")]
        [InlineData("99 Test Accounts")]
        public void Valid(string value)
        {
            IValidator<Name> validator = new NameValidator();

            var target = new ValidationTarget<Name>(new Name(value));
            validator.Validate(target);
            Assert.False(target.GetResult().HasErrors);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" Test")]
        [InlineData("Test Account ")]
        [InlineData("Test Account\nTwo Lines")]
        [InlineData("Test Account\tWith a Tab")]
        [InlineData("99 Problems and an invalid Test Account is one because its way wayy waaaaayy waaaaaaay waaaaaaaaay too long")]
        public void Invalid(string value)
        {
            IValidator<Name> validator = new NameValidator();

            var target = new ValidationTarget<Name>(new Name(value));
            validator.Validate(target);
            Assert.True(target.GetResult().HasErrors);
        }
    }
}
