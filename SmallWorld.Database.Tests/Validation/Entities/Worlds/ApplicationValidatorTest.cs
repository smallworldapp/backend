//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.Extensions.DependencyInjection;
//using SmallWorld.Database.Entities;
//using SmallWorld.Database.Model.Abstractions;
//using SmallWorld.Database.Validation.Abstractions;
//using Xunit;
//
//namespace SmallWorld.Database.Tests.Validation.Entities.Worlds
//{
//    public class ApplicationValidatorTest : TestBase
//    {
//        public static IEnumerable<Application> CreateValid()
//        {
//            yield return new Application {
//                Questions = new HashSet<ApplicationQuestion>()
//            }.Init();
//
//            yield return new Application {
//                Questions = ApplicationQuestionValidatorTest.CreateValid().ToList()
//            }.Init();
//        }
//
//        public static IEnumerable<Application> CreateInvalid()
//        {
//            yield return new Application {
//                Questions = new HashSet<ApplicationQuestion>()
//            };
//
//            yield return new Application {
//                Questions = ApplicationQuestionValidatorTest.CreateInvalid().ToList()
//            }.Init();
//
//            yield return new Application {
//                Questions = null
//            }.Init();
//        }
//
//        public static IEnumerable<object[]> ValidData => CreateValid().Select(a => new object[] { a });
//        public static IEnumerable<object[]> InvalidData => CreateInvalid().Select(a => new object[] { a });
//
//        [Theory]
//        [MemberData(nameof(ValidData))]
//        public async Task Valid(Application value)
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
//        public async Task Invalid(Application value)
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
