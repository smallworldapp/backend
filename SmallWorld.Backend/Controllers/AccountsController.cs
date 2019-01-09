using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Filters;
using SmallWorld.Models.Emailing;
using SmallWorld.Models.Providers;

namespace SmallWorld.Controllers
{
    [Route("accounts")]
    public class AccountsController : MyController
    {
        private readonly IAccountRepository accounts;
        private readonly CryptoProvider crypto;
        private readonly EmailProvider emails;

        public AccountsController(IAccountRepository accounts, CryptoProvider crypto, EmailProvider emails)
        {
            this.accounts = accounts;
            this.crypto = crypto;
            this.emails = emails;
        }

        [HttpGet]
        [AuthRequired]
        public IActionResult GetAccounts()
        {
            var list = from account in accounts.NotDeleted
                       where Permissions.AccessAccount(account)
                       select account;

            return Ok(list);
        }

        [HttpPost]
        [AuthRequired]
        [DatabaseUpdate]
        public IActionResult CreateAccount([FromBody] Account account)
        {
            if (!Permissions.CreateAccount())
                return Unauthorized();

            if (account.Type == AccountType.ERROR)
                return BadRequest();

            bool isCreating;
            account.Email = new EmailAddress(account.Email.Value.Trim());

            if (accounts.Find(account.Email, out var old))
            {
                if (old.Status != AccountStatus.Deleted)
                    return BadRequest();

                old.Name = account.Name;
                account = old;
                isCreating = false;
            }
            else
            {
                isCreating = true;
            }

            account.Status = AccountStatus.New;
            account.ResetToken = null;

#if DEBUG
            if (account.Email.Value == "test-auto-email")
            {
                account.Credentials = new Credentials();
                account.Credentials.CreateIds();
                account.Credentials.Salt = crypto.GenerateSalt();
                account.Credentials.Hash = crypto.GenerateHash(account.Credentials.Salt, "test-auto-password");
            }
            else
            {
#endif
                var password = crypto.CreatePassword();

                account.Credentials = new Credentials();
                account.Credentials.CreateIds();
                account.Credentials.Salt = crypto.GenerateSalt();
                account.Credentials.Hash = crypto.GenerateHash(account.Credentials.Salt, password);

                emails.Send(Emails.AccountCreation, account, password);
#if DEBUG
            }
#endif

            if (isCreating)
                accounts.Add(account);
            else
                accounts.Update(account);

            return Ok(account);
        }
    }
}
