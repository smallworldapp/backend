using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using SmallWorld.Database.Validators.Entities.CustomTypes;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;
using Xunit;

namespace SmallWorld.Database.Tests.Validators.Entities.CustomTypes
{
    public class IdentifierValidatorTest : TestBase
    {
        [Theory]
        [InlineData("test-world")]
        [InlineData("test-account")]
        [InlineData("not-much-wiggle-room")]
        public void Valid(string value)
        {
            IValidator<Identifier> validator = new IdentifierValidator();

            var target = new ValidationTarget<Identifier>(new Identifier(value));
            validator.Validate(target);
            Assert.False(target.GetResult().HasErrors);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" test")]
        [InlineData("Test")]
        [InlineData("this-identifier-is-sooooo-looong-its-crazy-how-long-it-is-like-really-crazy-whos-idea-was-this-it-still")]
        [InlineData("-almost")]
        [InlineData("almost-")]
        public void Invalid(string value)
        {
            IValidator<Identifier> validator = new IdentifierValidator();

            var target = new ValidationTarget<Identifier>(new Identifier(value));
            validator.Validate(target);
            Assert.True(target.GetResult().HasErrors);
        }
    }
}
