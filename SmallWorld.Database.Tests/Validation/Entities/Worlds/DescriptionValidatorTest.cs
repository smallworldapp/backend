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
//    public class DescriptionValidatorTest : TestBase
//    {
//        public static IEnumerable<Description> CreateValid()
//        {
//            yield return new Description {
//                Introduction = "intro",
//                Goals = "goals",
//
//                Faq = new HashSet<FaqItem>()
//            }.Init();
//
//            yield return new Description {
//                Introduction = "intro",
//                Goals = "goals",
//
//                Faq = FaqItemValidatorTest.CreateValid().ToList()
//            }.Init();
//        }
//
//        public static IEnumerable<Description> CreateInvalid()
//        {
//            yield return new Description {
//                Introduction = "intro",
//                Goals = "goals"
//            };
//
//            yield return new Description {
//                Introduction = null,
//                Goals = "goals"
//            }.Init();
//
//            yield return new Description {
//                Introduction = "intro",
//                Goals = null
//            }.Init();
//
//            yield return new Description {
//                Introduction = "intro",
//                Goals = "goals",
//
//                Faq = FaqItemValidatorTest.CreateInvalid().ToList()
//            }.Init();
//
//            yield return new Description {
//                Introduction = "intro",
//                Goals = "goals",
//
//                Faq = FaqItemValidatorTest.CreateValid().ToList()
//            };
//
//            yield return new Description {
//                Introduction = null,
//                Goals = "goals",
//
//                Faq = FaqItemValidatorTest.CreateValid().ToList()
//            }.Init();
//
//            yield return new Description {
//                Introduction = "intro",
//                Goals = null,
//
//                Faq = FaqItemValidatorTest.CreateValid().ToList()
//            }.Init();
//
//            yield return new Description {
//                Introduction = "intro",
//                Goals = "goals",
//
//                Faq = FaqItemValidatorTest.CreateInvalid().ToList()
//            }.Init();
//        }
//
//        public static IEnumerable<object[]> ValidData => CreateValid().Select(a => new object[] { a });
//        public static IEnumerable<object[]> InvalidData => CreateInvalid().Select(a => new object[] { a });
//
//        [Theory]
//        [MemberData(nameof(ValidData))]
//        public async Task Valid(Description value)
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
//        public async Task Invalid(Description value)
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
