using System.Threading.Tasks;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Tests.Validation.Entities.CustomTypes;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using Xunit;
using Xunit.Abstractions;

namespace SmallWorld.Database.Tests.Validation.Entities.Accounts
{
    public class AccountValidation : ValidationBase<Account>
    {
        public static ISource<Account> Source = new AccountFactory();

        private class AccountFactory : Factory<Account>
        {
            public ISource<Name> Name = NameValidation.Source;
            public ISource<EmailAddress> Email = EmailAddressValidation.Source;
            public ISource<AccountStatus> Status = AccountStatusValidation.Source;
            public ISource<AccountType> Type = AccountTypeValidation.Source;

            public ISource<ResetToken> ResetToken = ResetTokenValidation.Source;
            public ISource<Credentials> Credentials = CredentialsValidation.Source;
        }

        public AccountValidation(ITestOutputHelper output) : base(output) { }

        [Theory]
        [FactoryData(typeof(AccountFactory), true)]
        public override Task Validate_True(Account value) => base.Validate_True(value);

        [Theory]
        [FactoryData(typeof(AccountFactory), false)]
        public override Task Validate_False(Account value) => base.Validate_False(value);

        //        public static Factory<Account> Factory = new Factory<Account, Name, EmailAddress, AccountStatus, AccountType, ResetToken, Credentials>();
        //        public static MemberData<Account> Valid = new MemberData<Account> {
        //            AccountType.Standard,
        //            AccountType.Conference,
        //        };
        //
        //        public static MemberData<Account> Invalid = new MemberData<Account> {
        //            AccountType.ERROR
        //        };
        //
        //        public static Source<AccountType> Source = new Source<AccountType>(Valid, Invalid);
        //
        //        [Theory]
        //        [MemberData(nameof(Valid))]
        //        public override Task Validate_True(AccountType value) => base.Validate_True(value);
        //
        //        [Theory]
        //        [MemberData(nameof(Invalid))]
        //        public override Task Validate_False(AccountType value) => base.Validate_False(value);
        //
        //        public static IEnumerable<Account> CreateValid()
        //        {
        //            var crypto = new CryptoProvider();
        //
        //            var salt = crypto.GenerateSalt();
        //
        //            var name = NameValidation.CreateValid().First();
        //            var email = EmailAddressValidatorTest.CreateValid().First();
        //            var status = AccountStatusValidation.CreateValid().First();
        //            var type = AccountTypeValidation.CreateValid().First();
        //
        //            yield return new Account {
        //                Name = name,
        //                Email = email,
        //                Status = status,
        //                Type = type,
        //
        //                ResetToken = null,
        //
        //                Credentials = new Credentials {
        //                    Salt = salt,
        //                    Hash = crypto.GenerateHash(salt, "password"),
        //                }.Init()
        //            }.Init();
        //
        //            yield return new Account {
        //                Name = name,
        //                Email = email,
        //                Status = status,
        //                Type = type,
        //
        //                ResetToken = new ResetToken {
        //                    Expiration = DateTime.UtcNow.AddDays(1)
        //                }.Init(),
        //
        //                Credentials = new Credentials {
        //                    Salt = salt,
        //                    Hash = crypto.GenerateHash(salt, "password"),
        //                }.Init()
        //            }.Init();
        //        }
        //
        //        public static IEnumerable<Account> CreateInvalid()
        //        {
        //            var crypto = new CryptoProvider();
        //
        //            var salt = crypto.GenerateSalt();
        //
        //            yield return new Account {
        //                Name = null,
        //                Email = EmailAddressValidatorTest.CreateValid().First(),
        //                Status = AccountStatus.Default,
        //                Type = AccountType.Standard,
        //
        //                Credentials = new Credentials {
        //                    Salt = salt,
        //                    Hash = crypto.GenerateHash(salt, "password"),
        //                }.Init()
        //            };
        //
        //            yield return new Account {
        //                Name = NameValidation.CreateInvalid().First(),
        //                Email = EmailAddressValidatorTest.CreateValid().First(),
        //                Status = AccountStatus.Default,
        //                Type = AccountType.Standard,
        //
        //                Credentials = new Credentials {
        //                    Salt = salt,
        //                    Hash = crypto.GenerateHash(salt, "password"),
        //                }.Init()
        //            };
        //
        //            yield return new Account {
        //                Name = NameValidation.CreateValid().First(),
        //                Email = null,
        //                Status = AccountStatus.Default,
        //                Type = AccountType.Standard,
        //
        //                Credentials = new Credentials {
        //                    Salt = salt,
        //                    Hash = crypto.GenerateHash(salt, "password"),
        //                }.Init()
        //            };
        //
        //            yield return new Account {
        //                Name = NameValidation.CreateValid().First(),
        //                Email = EmailAddressValidatorTest.CreateInvalid().First(),
        //                Status = AccountStatus.Default,
        //                Type = AccountType.Standard,
        //
        //                Credentials = new Credentials {
        //                    Salt = salt,
        //                    Hash = crypto.GenerateHash(salt, "password"),
        //                }.Init()
        //            };
        //
        //            yield return new Account {
        //                Name = NameValidation.CreateValid().First(),
        //                Email = EmailAddressValidatorTest.CreateValid().First(),
        //                Status = AccountStatus.ERROR,
        //                Type = AccountType.Standard,
        //
        //                Credentials = new Credentials {
        //                    Salt = salt,
        //                    Hash = crypto.GenerateHash(salt, "password"),
        //                }.Init()
        //            };
        //
        //            yield return new Account {
        //                Name = NameValidation.CreateValid().First(),
        //                Email = EmailAddressValidatorTest.CreateValid().First(),
        //                Status = AccountStatus.Default,
        //                Type = AccountType.ERROR,
        //
        //                Credentials = new Credentials {
        //                    Salt = salt,
        //                    Hash = crypto.GenerateHash(salt, "password"),
        //                }.Init()
        //            };
        //
        //            yield return new Account {
        //                Name = NameValidation.CreateValid().First(),
        //                Email = EmailAddressValidatorTest.CreateValid().First(),
        //                Status = AccountStatus.Default,
        //                Type = AccountType.Standard,
        //
        //                Credentials = null
        //            };
        //        }
        //
        //        public static IEnumerable<object[]> ValidData => CreateValid().Select(a => new object[] { a });
        //        public static IEnumerable<object[]> InvalidData => CreateInvalid().Select(a => new object[] { a });
        //
        //        [Theory]
        //        [MemberData(nameof(ValidData))]
        //        public async Task Valid(Account value)
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
        //        public async Task Invalid(Account value)
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
    }
}
