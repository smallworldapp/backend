using System.Threading.Tasks;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SmallWorld.Database.Tests.Validation.Entities.Accounts
{
    public class CredentialsValidation : ValidationBase<Credentials>
    {
        private class CredentialsFactory : Factory<Credentials>
        {
            public ISource<byte[]> Hash = Test_Helpers.Source.OrDefault(new byte[32]);
            public ISource<byte[]> Salt = Test_Helpers.Source.OrDefault(new byte[32]);
        }

        public static ISource<Credentials> Source = new CredentialsFactory();

        public CredentialsValidation(ITestOutputHelper output) : base(output) { }

        [Theory]
        [FactoryData(typeof(CredentialsFactory), true)]
        public override Task Validate_True(Credentials value) => base.Validate_True(value);

        [Theory]
        [FactoryData(typeof(CredentialsFactory), false)]
        public override Task Validate_False(Credentials value) => base.Validate_False(value);
    }
}
