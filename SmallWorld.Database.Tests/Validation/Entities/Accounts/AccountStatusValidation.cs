using System.Threading.Tasks;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SmallWorld.Database.Tests.Validation.Entities.Accounts
{
    public class AccountStatusValidation : ValidationBase<AccountStatus>
    {
        public static MemberData<AccountStatus> Valid = new MemberData<AccountStatus> {
            AccountStatus.Default,
            AccountStatus.Deleted,
            AccountStatus.New
        };

        public static MemberData<AccountStatus> Invalid = new MemberData<AccountStatus> {
            AccountStatus.ERROR
        };

        public static Source<AccountStatus> Source = new Source<AccountStatus>(Valid, Invalid);

        public AccountStatusValidation(ITestOutputHelper output) : base(output) { }

        [Theory]
        [MemberData(nameof(Valid))]
        public override Task Validate_True(AccountStatus value) => base.Validate_True(value);

        [Theory]
        [MemberData(nameof(Invalid))]
        public override Task Validate_False(AccountStatus value) => base.Validate_False(value);
    }
}
