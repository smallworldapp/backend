using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using SmallWorld.Database.Validators;
using SmallWorld.Library.Validation;
using SmallWorld.Library.Validation.Abstractions;
using Xunit;
using SmallWorld.Library;
using SmallWorld.Library.Validation.Helpers;
using SmallWorld.Library.Validation.Impl;

namespace SmallWorld.Database.Tests.Validators.Entities
{
    public class EnumValidatorTest : TestBase
    {
        private static IEnumerable<object[]> GetTypes()
        {
            return from type in typeof(SmallWorldContext).Assembly.GetLoadableDefinedTypes()
                   where type.Namespace?.StartsWith("SmallWorld.Database.Entities") ?? false
                   where type.IsEnum
                   where type.GetCustomAttribute<ObsoleteAttribute>() == null
                   select new object[] { type };
        }

        protected override void AddServices(IServiceCollection services)
        {
            services.AddValidators();
        }

        [Theory]
        [MemberData(nameof(GetTypes))]
        public async Task Validate_NotError(Type type)
        {
            using (var provider = await CreateProvider())
            {
                var sources = provider.GetServices<IValidatorProvider>();
                var validators = from src in sources
                                 from arg in src.GetValidators(type)
                                 select arg;

                var genericType = typeof(IValidator<>).MakeGenericType(type);
                var method = genericType.GetMethod(nameof(IValidator<object>.Validate));

                var value = Activator.CreateInstance(type);
                var targetType = typeof(ValidationTarget<>).MakeGenericType(type);
                var target = (IValidationTarget) Activator.CreateInstance(targetType, value);

                foreach (var validatorType in validators)
                {
                    var validator = Activator.CreateInstance(validatorType);

                    method.Invoke(validator, new object[] { target });
                }

                Assert.True(target.GetResult().HasErrors, target.GetResult().ToString());
            }
        }
    }
}
