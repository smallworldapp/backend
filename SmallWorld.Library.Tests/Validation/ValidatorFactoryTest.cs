//using System.Collections.Generic;
//using System.Linq;
//using Microsoft.Extensions.DependencyInjection;
//using SmallWorld.Library.Validation;
//using SmallWorld.Library.Validation.Abstractions;
//using SmallWorld.Library.Validation.Impl;
//using SmallWorld.Library.Validators;
//using Xunit;
//
//namespace SmallWorld.Library.Tests.Validation
//{
//    public class ValidatorFactoryTest : TestBase
//    {
//        protected override void AddServices(IServiceCollection services)
//        {
//            services.AddScoped<ValidatorFactory>();
//
//            services.AddScoped(typeof(RequiredValidator<>));
//
//            var provider = new ValidatorProvider();
//            provider.AddTypeValidator(typeof(IEnumerable<>), typeof(EnumerableValidator<>));
//            services.AddSingleton<IValidatorProvider>(provider);
//        }
//
//        [Fact]
//        public void GetRequiredValidator()
//        {
//            using (var provider = CreateProvider())
//            using (var scope = provider.CreateScope())
//            {
//                var factory = scope.ServiceProvider.GetRequiredService<ValidatorFactory>();
//
//                var intValidator = factory.CreateValidator<int>(typeof(RequiredValidator<>));
//                Assert.NotNull(intValidator);
//
//                var objValidator = factory.CreateValidator<object>(typeof(RequiredValidator<>));
//                Assert.NotNull(objValidator);
//            }
//        }
//
//        [Fact]
//        public void GetCollectionValidator()
//        {
//            using (var provider = CreateProvider())
//            using (var scope = provider.CreateScope())
//            {
//                var factory = scope.ServiceProvider.GetRequiredService<ValidatorFactory>();
//
//                var intValidator = factory.GetTypeValidator<ICollection<int>>();
//                Assert.Equal(typeof(EnumerableValidator<int>), intValidator.ValidatorTypes.Single());
//
//                var objValidator = factory.GetTypeValidator<ICollection<object>>();
//                Assert.Equal(typeof(EnumerableValidator<object>), objValidator.ValidatorTypes.Single());
//            }
//        }
//    }
//}
