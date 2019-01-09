//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using SmallWorld.Database.Entities;
//using SmallWorld.Database.Model.Abstractions;
//using SmallWorld.Database.Tests.Validation.Entities.Worlds;
//using SmallWorld.Database.Validation.Abstractions;
//using Xunit;
//
//namespace SmallWorld.Database.Tests.Validation.Entities.Members
//{
//    public class PairingValidatorTest : TestBase
//    {
//        public static IEnumerable<Pairing> CreateValid()
//        {
//            var world = WorldValidatorTest.CreateValid().First();
//            var type = PairingTypeValidatorTest.CreateValid().First();
//
//            yield return new Pairing {
//                IsComplete = true,
//
//                Date = DateTime.UtcNow,
//
//                Type = type,
//
//                Message = null,
//
//                World = world
//            }.Init();
//
//            yield return new Pairing {
//                IsComplete = false,
//
//                Date = DateTime.UtcNow,
//
//                Type = type,
//
//                Message = null,
//
//                World = world
//            }.Init();
//
//            yield return new Pairing {
//                IsComplete = true,
//
//                Date = DateTime.UtcNow,
//
//                Type = type,
//
//                Message = "Test message",
//
//                World = world
//            }.Init();
//        }
//
//        public static IEnumerable<Pairing> CreateInvalid()
//        {
//            var world = WorldValidatorTest.CreateValid().First();
//            var type = PairingTypeValidatorTest.CreateValid().First();
//
//            yield return new Pairing {
//                IsComplete = true,
//
//                Date = DateTime.UtcNow,
//
//                Type = type,
//
//                Message = null,
//
//                World = world
//            };
//
//            yield return new Pairing {
//                IsComplete = true,
//
//                Date = default(DateTime),
//
//                Type = type,
//
//                Message = null,
//
//                World = world
//            }.Init();
//
//            yield return new Pairing {
//                IsComplete = true,
//
//                Date = DateTime.UtcNow,
//
//                Type = PairingType.ERROR,
//
//                Message = null,
//
//                World = world
//            }.Init();
//
//            yield return new Pairing {
//                IsComplete = true,
//
//                Date = DateTime.UtcNow,
//
//                Type = type,
//
//                Message = null,
//
//                World = null
//            }.Init();
//
//            yield return new Pairing {
//                IsComplete = true,
//
//                Date = DateTime.UtcNow,
//
//                Type = type,
//
//                Message = null,
//
//                World = WorldValidatorTest.CreateInvalid().First()
//            }.Init();
//        }
//
//        public static IEnumerable<object[]> ValidData => CreateValid().Select(a => new object[] { a });
//        public static IEnumerable<object[]> InvalidData => CreateInvalid().Select(a => new object[] { a });
//
//        [Theory]
//        [MemberData(nameof(ValidData))]
//        public async Task Valid(Pairing value)
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
//        public async Task Invalid(Pairing value)
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
