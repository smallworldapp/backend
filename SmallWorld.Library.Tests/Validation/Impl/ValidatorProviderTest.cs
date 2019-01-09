using System;
using System.Collections.Generic;
using System.Linq;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;
using SmallWorld.Library.Validators;
using Xunit;

namespace SmallWorld.Library.Tests.Validation.Impl
{
    public class ValidatorProviderTest
    {
        public class FakeValidator : IValidator<object>
        {
            public void Validate(IValidationTarget<object> target) { }
        }

        [Fact]
        public void GetValidators_Simple()
        {
            var provider = new ValidatorProvider();
            provider.AddTypeValidator(typeof(object), typeof(FakeValidator));

            var result = provider.GetValidators(typeof(object));

            Assert.Equal(typeof(FakeValidator), result.Single());
        }

        [Fact]
        public void GetValidators_Simple_Inheritance()
        {
            var provider = new ValidatorProvider();
            provider.AddTypeValidator(typeof(object), typeof(FakeValidator));

            var result = provider.GetValidators(typeof(ValidatorProviderTest));

            Assert.Equal(typeof(FakeValidator), result.Single());
        }

        [Theory]
        [InlineData(typeof(IEnumerable<int>))]
        [InlineData(typeof(ICollection<int>))]
        [InlineData(typeof(HashSet<int>))]
        [InlineData(typeof(List<int>))]
        public void GetValidators_Direct_IntEnumerable(Type targetType)
        {
            var provider = new ValidatorProvider();
            provider.AddTypeValidator(typeof(IEnumerable<int>), typeof(EnumerableValidator<int>));

            var result = provider.GetValidators(targetType);

            Assert.Equal(typeof(EnumerableValidator<int>), result.Single());
        }

        [Theory]
        [InlineData(typeof(IEnumerable<int>))]
        [InlineData(typeof(ICollection<int>))]
        [InlineData(typeof(HashSet<int>))]
        [InlineData(typeof(List<int>))]
        public void GetValidators_Generic_IntEnumerable(Type targetType)
        {
            var provider = new ValidatorProvider();
            provider.AddTypeValidator(typeof(IEnumerable<>), typeof(EnumerableValidator<>));

            var result = provider.GetValidators(targetType);

            Assert.Equal(typeof(EnumerableValidator<int>), result.Single());
        }

        [Theory]
        [InlineData(typeof(IEnumerable<int>))]
        [InlineData(typeof(ICollection<int>))]
        [InlineData(typeof(HashSet<int>))]
        [InlineData(typeof(List<int>))]
        public void GetValidators_Both_IntEnumerable(Type targetType)
        {
            var provider = new ValidatorProvider();
            provider.AddTypeValidator(typeof(IEnumerable<>), typeof(EnumerableValidator<>));
            provider.AddTypeValidator(typeof(IEnumerable<int>), typeof(EnumerableValidator<int>));

            var result = provider.GetValidators(targetType).ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal(typeof(EnumerableValidator<int>), result.First());
            Assert.Equal(typeof(EnumerableValidator<int>), result.Last());
        }
    }
}
