using System;
using System.Threading.Tasks;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SmallWorld.Database.Tests.Validation.Entities.Accounts
{
    public class ResetTokenValidation : ValidationBase<ResetToken>
    {
        public static ISource<ResetToken> Source = new ResetTokenFactory();

        private class ResetTokenFactory : Factory<ResetToken>
        {
            public Source<DateTime> Expiration = Test_Helpers.Source.OrDefault(DateTime.Today);
        }

        public ResetTokenValidation(ITestOutputHelper output) : base(output) { }

        [Theory]
        [FactoryData(typeof(ResetTokenFactory), true)]
        public override Task Validate_True(ResetToken value) => base.Validate_True(value);

        [Theory]
        [FactoryData(typeof(ResetTokenFactory), false)]
        public override Task Validate_False(ResetToken value) => base.Validate_False(value);
    }
}
