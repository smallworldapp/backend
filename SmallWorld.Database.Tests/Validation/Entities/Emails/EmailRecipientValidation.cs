using System.Threading.Tasks;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Entities.CustomTypes;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SmallWorld.Database.Tests.Validation.Entities.Emails
{
    public class EmailRecipientValidation : ValidationBase<EmailRecipient>
    {
        public static ISource<EmailRecipient> Source = new ResetTokenFactory();

        private class ResetTokenFactory : Factory<EmailRecipient>
        {
            public Source<Name> Name = NameValidation.Source;
            public Source<EmailAddress> Address = EmailAddressValidation.Source;
        }

        public EmailRecipientValidation(ITestOutputHelper output) : base(output) { }

        [Theory]
        [FactoryData(typeof(ResetTokenFactory), true)]
        public override Task Validate_True(EmailRecipient value) => base.Validate_True(value);

        [Theory]
        [FactoryData(typeof(ResetTokenFactory), false)]
        public override Task Validate_False(EmailRecipient value) => base.Validate_False(value);
    }
}
