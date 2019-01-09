//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using SmallWorld.Database.Entities;
//using SmallWorld.Database.Model.Abstractions;
//using SmallWorld.Database.Tests.Validation.Entities.CustomTypes;
//using SmallWorld.Database.Tests.Validation.Entities.Worlds;
//using SmallWorld.Database.Validation.Abstractions;
//using Xunit;
//
//namespace SmallWorld.Database.Tests.Validation.Entities.Members
//{
//    public class MemberValidatorTest : TestBase
//    {
//        public static IEnumerable<Member> CreateValid()
//        {
//            var name = NameValidatorTest.CreateValid().First();
//            var email = EmailAddressValidatorTest.CreateValid().First();
//            var token = Token.Generate();
//            var world = WorldValidatorTest.CreateValid().First();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                OptOut = true,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                OptOut = false,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                HasLeft = true,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                HasLeft = false,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                HasEmailValidation = true,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                HasEmailValidation = false,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                HasPrivacyValidation = true,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                HasPrivacyValidation = false,
//
//                World = world,
//            }.Init();
//        }
//
//        public static IEnumerable<Member> CreateInvalid()
//        {
//            var name = NameValidatorTest.CreateValid().First();
//            var email = EmailAddressValidatorTest.CreateValid().First();
//            var token = Token.Generate();
//            var world = WorldValidatorTest.CreateValid().First();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                OptOut = true,
//
//                World = world,
//            };
//
//            yield return new Member {
//                FirstName = null,
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                OptOut = true,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = NameValidatorTest.CreateInvalid().First(),
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                OptOut = true,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = null,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                OptOut = true,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = NameValidatorTest.CreateInvalid().First(),
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                OptOut = true,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = null,
//                JoinToken = token,
//                LeaveToken = token,
//
//                OptOut = true,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = EmailAddressValidatorTest.CreateInvalid().First(),
//                JoinToken = token,
//                LeaveToken = token,
//
//                OptOut = true,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = email,
//                JoinToken = default(Guid),
//                LeaveToken = token,
//
//                OptOut = true,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = default(Guid),
//
//                OptOut = true,
//
//                World = world,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                OptOut = true,
//
//                World = null,
//            }.Init();
//
//            yield return new Member {
//                FirstName = name,
//                LastName = name,
//                Email = email,
//                JoinToken = token,
//                LeaveToken = token,
//
//                OptOut = true,
//
//                World = WorldValidatorTest.CreateInvalid().First(),
//            }.Init();
//        }
//
//        public static IEnumerable<object[]> ValidData => CreateValid().Select(a => new object[] { a });
//        public static IEnumerable<object[]> InvalidData => CreateInvalid().Select(a => new object[] { a });
//
//        [Theory]
//        [MemberData(nameof(ValidData))]
//        public async Task Valid(Member value)
//        {
//            using (var provider = CreateProvider())
//            using (var scope = provider.CreateScope())
//            {
//                var access = scope.ServiceProvider.GetService<IContextLock>();
//                var validation = scope.ServiceProvider.GetService<IValidationContext>();
//
//                using (await access.Read())
//                {
//                    Assert.True(validation.Validate(value, out var _));
//                }
//            }
//        }
//
//        [Theory]
//        [MemberData(nameof(InvalidData))]
//        public async Task Invalid(Member value)
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
//    }
//}
