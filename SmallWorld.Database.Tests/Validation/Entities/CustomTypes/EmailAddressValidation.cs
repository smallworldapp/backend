using System.Threading.Tasks;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SmallWorld.Database.Tests.Validation.Entities.CustomTypes
{
    public class EmailAddressValidation : ValidationBase<EmailAddress>
    {
        public static MemberData<EmailAddress> Valid = new MemberData<string> {
            "test@mfro.me",
            "test@gmail.com",
            "this-email@smallworldapp.org",
        }.Cast(v => new EmailAddress(v));

        public static MemberData<EmailAddress> Invalid = new MemberData<string> {
            null,
            "",
            "not-an-email",
            " test@mfro.me",
            "test@mfro.me ",
            "test\nemail@mfro.me",
            "thisisaverylongemailthatnoonewouldeverusebecauseitstoolong@mfro.me",
        }.Cast(v => new EmailAddress(v));

        public static Source<EmailAddress> Source = new Source<EmailAddress>(Valid, Invalid);

        public EmailAddressValidation(ITestOutputHelper output) : base(output) { }

        [Theory]
        [MemberData(nameof(Valid))]
        public override Task Validate_True(EmailAddress value) => base.Validate_True(value);

        [Theory]
        [MemberData(nameof(Invalid))]
        public override Task Validate_False(EmailAddress value) => base.Validate_False(value);
    }
}
