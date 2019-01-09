using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Database.Model.Impl;
using SmallWorld.Database.Tests.Validation.Test_Fake;
using SmallWorld.Database.Tests.Validation.Test_Helpers;
using SmallWorld.Database.Validators.Entities.Accounts;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Library.Model.Impl;
using SmallWorld.Library.Validation.Abstractions;
using SmallWorld.Library.Validation.Impl;
using Xunit;

namespace SmallWorld.Database.Tests.Validators.Entities.Accounts
{
    public class AccountValidatorTest : TestBase
    {
        protected override void AddServices(IServiceCollection services)
        {
            services.AddSingleton<IContext, EmulatedContext>();
            services.AddSingleton<IContextLock, ContextLock>();
            services.AddSingleton<IAccountRepository, AccountRepository>();
        }

        [Fact]
        public async Task Validate_OneAccount()
        {
            using (var provider = await CreateProvider())
            {
                var access = provider.GetRequiredService<IContextLock>();
                var accounts = provider.GetRequiredService<IAccountRepository>();
                IValidator<Account> validator = new AccountValidator(accounts);
                var account = new Account();

                using (await access.Read())
                {
                    var target = new ValidationTarget<Account>(account);
                    validator.Validate(target);
                    Assert.False(target.GetResult().HasErrors);
                }
            }
        }

        [Theory]
        [InlineData(false, "test@example.com", "test@example.com")]
        [InlineData(false, "test@ExAmPlE.com", "test@example.com")]
        [InlineData(false, "TeSt@example.com", "test@example.com")]
        [InlineData(true, "test-2@example.com", "test@example.com")]
        public async Task Validate_TwoAccounts(bool valid, string email1, string email2)
        {
            using (var provider = await CreateProvider())
            {
                var access = provider.GetRequiredService<IContextLock>();
                var context = provider.GetRequiredService<IContext>();
                var accounts = provider.GetRequiredService<IAccountRepository>();
                IValidator<Account> validator = new AccountValidator(accounts);

                using (var handle = await access.Write())
                {
                    var one = new Account { Email = new EmailAddress(email1) };
                    context.Add(one);
                    await handle.Finish();
                }

                using (await access.Read())
                {
                    var two = new Account { Email = new EmailAddress(email2) };
                    var target = new ValidationTarget<Account>(two);
                    validator.Validate(target);
                    Assert.Equal(valid, !target.GetResult().HasErrors);
                }
            }
        }
    }
}
