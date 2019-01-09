using System.Threading.Tasks;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SmallWorld.Database.Tests.Validation.Entities.Accounts
{
    public class AccountTypeValidation : ValidationBase<AccountType>
    {
        public static MemberData<AccountType> Valid = new MemberData<AccountType> {
            AccountType.Standard,
            AccountType.Conference,
        };

        public static MemberData<AccountType> Invalid = new MemberData<AccountType> {
            AccountType.ERROR
        };

        public static Source<AccountType> Source = new Source<AccountType>(Valid, Invalid);

        public AccountTypeValidation(ITestOutputHelper output) : base(output) { }

        [Theory]
        [MemberData(nameof(Valid))]
        public override Task Validate_True(AccountType value) => base.Validate_True(value);

        [Theory]
        [MemberData(nameof(Invalid))]
        public override Task Validate_False(AccountType value) => base.Validate_False(value);
    }
}
