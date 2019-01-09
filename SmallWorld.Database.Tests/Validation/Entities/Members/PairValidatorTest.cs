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
//    public class PairValidatorTest : TestBase
//    {
//        public static IEnumerable<Pair> CreateValid()
//        {
//            var pairing = PairingValidatorTest.CreateValid().First();
//            var world = pairing.World;
//
//            var init = MemberValidatorTest.CreateValid().First();
//            var recv = MemberValidatorTest.CreateValid().First();
//
//            var outcome = PairOutcomeValidatorTest.CreateValid().First();
//
//            yield return new Pair {
//                Outcome = outcome,
//
//                World = world,
//                Pairing = pairing,
//
//                Initiator = init,
//                Receiver = recv
//            }.Init();
//        }
//
//        public static IEnumerable<Pair> CreateInvalid()
//        {
//            var pairing = PairingValidatorTest.CreateValid().First();
//            var world = pairing.World;
//
//            var init = MemberValidatorTest.CreateValid().First();
//            var recv = MemberValidatorTest.CreateValid().First();
//
//            var outcome = PairOutcomeValidatorTest.CreateValid().First();
//
//            yield return new Pair {
//                Outcome = outcome,
//
//                World = world,
//                Pairing = pairing,
//
//                Initiator = init,
//                Receiver = recv
//            };
//
//            yield return new Pair {
//                Outcome = PairOutcomeValidatorTest.CreateInvalid().First(),
//
//                World = world,
//                Pairing = pairing,
//
//                Initiator = init,
//                Receiver = recv
//            }.Init();
//
//            yield return new Pair {
//                Outcome = outcome,
//
//                World = null,
//                Pairing = pairing,
//
//                Initiator = init,
//                Receiver = recv
//            }.Init();
//
//            yield return new Pair {
//                Outcome = outcome,
//
//                World = WorldValidatorTest.CreateValid().First(),
//                Pairing = pairing,
//
//                Initiator = init,
//                Receiver = recv
//            }.Init();
//
//            yield return new Pair {
//                Outcome = outcome,
//
//                World = WorldValidatorTest.CreateInvalid().First(),
//                Pairing = pairing,
//
//                Initiator = init,
//                Receiver = recv
//            }.Init();
//
//            yield return new Pair {
//                Outcome = outcome,
//
//                World = world,
//                Pairing = null,
//
//                Initiator = init,
//                Receiver = recv
//            }.Init();
//
//            yield return new Pair {
//                Outcome = outcome,
//
//                World = world,
//                Pairing = PairingValidatorTest.CreateInvalid().First(),
//
//                Initiator = init,
//                Receiver = recv
//            }.Init();
//
//            yield return new Pair {
//                Outcome = outcome,
//
//                World = world,
//                Pairing = pairing,
//
//                Initiator = null,
//                Receiver = recv
//            }.Init();
//
//            yield return new Pair {
//                Outcome = outcome,
//
//                World = world,
//                Pairing = pairing,
//
//                Initiator = MemberValidatorTest.CreateInvalid().First(),
//                Receiver = recv
//            }.Init();
//
//            yield return new Pair {
//                Outcome = outcome,
//
//                World = world,
//                Pairing = pairing,
//
//                Initiator = init,
//                Receiver = null
//            }.Init();
//
//            yield return new Pair {
//                Outcome = outcome,
//
//                World = world,
//                Pairing = pairing,
//
//                Initiator = init,
//                Receiver = MemberValidatorTest.CreateInvalid().First(),
//            }.Init();
//        }
//
//        public static IEnumerable<object[]> ValidData => CreateValid().Select(a => new object[] { a });
//        public static IEnumerable<object[]> InvalidData => CreateInvalid().Select(a => new object[] { a });
//
//        [Theory]
//        [MemberData(nameof(ValidData))]
//        public async Task Valid(Pair value)
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
//        public async Task Invalid(Pair value)
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
