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
//    public class ApplicationQuestionValidatorTest : TestBase
//    {
//        public static IEnumerable<ApplicationQuestion> CreateValid()
//        {
//            yield return new ApplicationQuestion {
//                Question = "What is the answer to life, the universe, and everything?",
//                Answer = "42"
//            }.Init();
//
//            yield return new ApplicationQuestion {
//                Question = "What is the answer to life, the universe, and everything?",
//                Subtext = "This time with subtext",
//                Answer = "42"
//            }.Init();
//        }
//
//        public static IEnumerable<ApplicationQuestion> CreateInvalid()
//        {
//            yield return new ApplicationQuestion {
//                Question = "What is the answer to life, the universe, and everything?",
//                Answer = "42"
//            };
//
//            yield return new ApplicationQuestion {
//                Question = null,
//                Answer = "42"
//            }.Init();
//
//            yield return new ApplicationQuestion {
//                Question = "What is the answer to life, the universe, and everything?",
//                Answer = null
//            }.Init();
//
//            yield return new ApplicationQuestion {
//                Question = "What is the answer to life, the universe, and everything?",
//                Subtext = "This time with subtext",
//                Answer = "42"
//            };
//
//            yield return new ApplicationQuestion {
//                Question = null,
//                Subtext = "This time with subtext",
//                Answer = "42"
//            }.Init();
//
//            yield return new ApplicationQuestion {
//                Question = "What is the answer to life, the universe, and everything?",
//                Subtext = "This time with subtext",
//                Answer = null
//            }.Init();
//        }
//
//        public static IEnumerable<object[]> ValidData => CreateValid().Select(a => new object[] { a });
//        public static IEnumerable<object[]> InvalidData => CreateInvalid().Select(a => new object[] { a });
//
//        [Theory]
//        [MemberData(nameof(ValidData))]
//        public async Task Valid(ApplicationQuestion value)
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
//        public async Task Invalid(ApplicationQuestion value)
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
