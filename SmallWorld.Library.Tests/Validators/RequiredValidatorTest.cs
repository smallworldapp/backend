using System;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;
using SmallWorld.Library.Validators;
using Xunit;

namespace SmallWorld.Library.Tests.Validators
{
    public class RequiredValidatorTest : TestBase
    {
        private void Validate<T>(bool success, T value)
        {
            IValidator<T> validator = new RequiredValidator<T>();
            var target = new ValidationTarget<T>(value);

            validator.Validate(target);

            var result = target.GetResult();
            Assert.Equal(success, !result.HasErrors);
        }

        [Fact]
        public void Valid()
        {
            Validate(true, "test");
            Validate(true, new object());
            Validate(true, new RequiredValidatorTest());
        }

        [Fact]
        public void NullValue()
        {
            Validate<string>(false, null);
            Validate<object>(false, null);
            Validate<RequiredValidatorTest>(false, null);
        }

        [Fact]
        public void DefaultStructs()
        {
            Validate(false, default(DateTime));
            Validate(false, default(Guid));
        }
    }
}
