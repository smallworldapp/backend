using System.Threading.Tasks;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SmallWorld.Database.Tests.Validation.Entities.CustomTypes
{
    public class IdentifierValidation : ValidationBase<Identifier>
    {
        public static MemberData<Identifier> Valid = new MemberData<string> {
            "test-world",
            "test-account",
            "not-much-wiggle-room",
        }.Cast(v => new Identifier(v));

        public static MemberData<Identifier> Invalid = new MemberData<string> {
            null,
            "",
            " test",
            "Test",
            "this-identifier-is-sooooo-looong-its-crazy-how-long-it-is-like-really-crazy-whos-idea-was-this-it-still",
            "-almost",
            "almost-",
        }.Cast(v => new Identifier(v));

        public static Source<Identifier> Source = new Source<Identifier>(Valid, Invalid);

        public IdentifierValidation(ITestOutputHelper output) : base(output) { }

        [Theory]
        [MemberData(nameof(Valid))]
        public override Task Validate_True(Identifier value) => base.Validate_True(value);

        [Theory]
        [MemberData(nameof(Invalid))]
        public override Task Validate_False(Identifier value) => base.Validate_False(value);
    }
}
