using System.Threading.Tasks;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SmallWorld.Database.Tests.Validation.Entities.CustomTypes
{
    public class NameValidation : ValidationBase<Name>
    {
        public static MemberData<Name> Valid = new MemberData<string> {
            "Test",
            "Test Account",
            "This is a Test Account",
            "99 Test Accounts"
        }.Cast(v => new Name(v));

        public static MemberData<Name> Invalid = new MemberData<string> {
            null,
            "",
            " Test",
            "Test Account ",
            "Test Account\nTwo Lines",
            "Test Account\tWith a Tab",
            "99 Problems and an invalid Test Account is one because its way wayy waaaaayy waaaaaaay waaaaaaaaay too long"
        }.Cast(v => new Name(v));

        public static Source<Name> Source = new Source<Name>(Valid, Invalid);

        public NameValidation(ITestOutputHelper output) : base(output) { }

        [Theory]
        [MemberData(nameof(Valid))]
        public override Task Validate_True(Name value) => base.Validate_True(value);

        [Theory]
        [MemberData(nameof(Invalid))]
        public override Task Validate_False(Name value) => base.Validate_False(value);
    }
}
