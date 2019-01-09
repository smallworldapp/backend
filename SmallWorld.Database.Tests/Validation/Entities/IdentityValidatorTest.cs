//using System.Collections.Generic;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using SmallWorld.Database.Entities;
//using SmallWorld.Database.Model.Abstractions;
//using SmallWorld.Database.Tests.Validation.Entities.CustomTypes;
//using SmallWorld.Database.Validation.Abstractions;
//using Xunit;
//
//namespace SmallWorld.Database.Tests.Validation.Entities
//{
//    public class IdentityValidatorTest : EntityValidatorTestBase
//    {
////        private static IEnumerable<Name> ValidNames()
////        {
////            yield return null;
////            foreach (var name in NameValidation.CreateInvalid())
////                yield return name;
////        }
////        public static MemberDataUtil<Name> InvalidNames = new MemberDataUtil<Name> {
////            null,
////            NameValidation.CreateInvalid(),
////        };
////
////        public static MemberDataUtil<EmailAddress> InvalidEmails = new MemberDataUtil<EmailAddress> {
////            null,
////            EmailAddressValidation.CreateInvalid(),
////        };
////
////        public static Factory<Identity> Factory { get} = new Factory<Identity, T1, T2, T3>();
//        public static IEnumerable<Identity> CreateValid()
//        {
//            yield return new Identity {
//                FirstName = NameValidatorTest.CreateValid().First(),
//                LastName = NameValidatorTest.CreateValid().First(),
//                Email = EmailAddressValidatorTest.CreateValid().First(),
//            }.Init();
//        }
//
//        public static IEnumerable<Identity> CreateInvalid()
//        {
//            return null;
////            yield return Make(
////                InvalidNames.Value,
////                InvalidNames.Value,
////                InvalidEmails.Value
////            );
//        }
//
//        private static Identity Make(
//            Optional<Name> firstName = default(Optional<Name>),
//            Optional<Name> lastName = default(Optional<Name>),
//            Optional<EmailAddress> email = default(Optional<EmailAddress>))
//        {
//            return new Identity {
//                FirstName = firstName.Or(() => NameValidatorTest.CreateValid().First()),
//                LastName = lastName.Or(() => NameValidatorTest.CreateValid().First()),
//                Email = email.Or(() => EmailAddressValidatorTest.CreateValid().First()),
//            };
//        }
////
////        public static IEnumerable<object[]> ValidData => CreateValid().Select(a => new object[] { a });
////
////        [Theory]
////        [MemberData(nameof(ValidData))]
////        public async Task Valid(Identity value)
////        {
////            using (var provider = CreateProvider())
////            using (var scope = provider.CreateScope())
////            {
////                var access = scope.ServiceProvider.GetService<IContextLock>();
////                var validation = scope.ServiceProvider.GetService<IValidationContext>();
////
////                using (await access.Read())
////                {
////                    Assert.True(validation.Validate(value, out var _));
////                }
////            }
////        }
////
//        [Fact]
//        public override async Task InvalidBaseEntity()
//        {
//            await Invalid(new Identity {
//                FirstName = NameValidatorTest.CreateValid().First(),
//                LastName = NameValidatorTest.CreateValid().First(),
//                Email = EmailAddressValidatorTest.CreateValid().First(),
//            });
//        }
//
//        private static async Task Invalid(Identity value)
//        {
//            using (var provider = CreateProvider())
//            using (var scope = provider.CreateScope())
//            {
//                var access = scope.ServiceProvider.GetService<IContextLock>();
//                var validation = scope.ServiceProvider.GetService<IValidationContext>();
//
//                using (await access.Read())
//                {
//                    Assert.False(validation.Validate(value, out var _));
//                }
//            }
//        }
////
////        public static MemberDataUtil<Name> InvalidNames = new MemberDataUtil<Name> {
////            null,
////            NameValidation.CreateInvalid(),
////        };
////
////        public static MemberDataUtil<EmailAddress> InvalidEmails = new MemberDataUtil<EmailAddress> {
////            null,
////            EmailAddressValidation.CreateInvalid(),
////        };
////
////        [Theory]
////        [MemberData(nameof(InvalidNames))]
////        public async Task InvalidFirstName(Name name)
////        {
////            await Invalid(Make(firstName: name));
////        }
////
////        [Theory]
////        [MemberData(nameof(InvalidNames))]
////        public async Task InvalidLastName(Name name)
////        {
////            await Invalid(Make(lastName: name));
////        }
////
////        [Theory]
////        [MemberData(nameof(InvalidEmails))]
////        public async Task InvalidEmail(EmailAddress email)
////        {
////            await Invalid(Make(email: email));
////        }
//    }
//}
