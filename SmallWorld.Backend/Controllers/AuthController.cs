using System;
using Microsoft.AspNetCore.Mvc;
using SmallWorld.Auth;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Filters;
using SmallWorld.Models.Emailing;
using SmallWorld.Models.Providers;

namespace SmallWorld.Controllers
{
    [Route("auth")]
    public class AuthController : MyController
    {
        private readonly IAccountRepository accounts;
        private readonly CryptoProvider crypto;
        private readonly AuthProvider auth;

        public AuthController(IAccountRepository accounts, CryptoProvider crypto, AuthProvider auth)
        {
            this.accounts = accounts;
            this.crypto = crypto;
            this.auth = auth;
        }

        [HttpGet]
        [AuthRequired]
        public IActionResult GetAuthStatus()
        {
            return Ok(Auth.Serialize(HttpContext.RequestServices));
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginCredentials creds)
        {
            if (!accounts.Include(a => a.Credentials).Find(new EmailAddress(creds.Email), out var acc))
                return NotFound();

            if (!crypto.Authenticate(acc, creds.Password))
                return NotFound();

            Auth = new AccountSession(acc.Guid);

            return Ok(Auth.Serialize(HttpContext.RequestServices));
        }

        [HttpPost("admin")]
        public IActionResult LoginAdmin([FromBody] LoginCredentials request)
        {
            if (!auth.AuthorizeAdmin(request.Email, request.Password))
                return Unauthorized();

            Auth = new AdminSession();

            return Ok(Auth.Serialize(HttpContext.RequestServices));
        }

        [HttpPost("resets")]
        [DatabaseUpdate]
        public IActionResult CreateReset([FromBody] LoginCredentials creds, [FromServices] EmailProvider email)
        {
            if (accounts.Include(a => a.ResetToken).Find(new EmailAddress(creds.Email), out var account))
            {
                account.ResetToken = new ResetToken();
                account.ResetToken.CreateIds();
                account.ResetToken.Expiration = DateTime.UtcNow.AddHours(24);

                accounts.Update(account);

                email.Send(Emails.PasswordReset, account);
            }

            return Ok();
        }

        [HttpPost("resets/{id}")]
        [DatabaseUpdate]
        public IActionResult ResolveReset(Guid id, [FromBody] LoginCredentials creds)
        {
            var value = accounts
                .Include(a => a.ResetToken)
                .Include(a => a.Credentials)
                .Find(a => a.ResetToken != null && a.ResetToken.Guid == id);

            if (value.Exists(out var account))
            {
                var reset = account.ResetToken;
                account.ResetToken = null;

                if (reset.Expiration < DateTime.UtcNow)
                    return NotFound();

                account.Credentials.Salt = crypto.GenerateSalt();
                account.Credentials.Hash = crypto.GenerateHash(account.Credentials.Salt, creds.Password);

                accounts.Update(account);
            }

            return Ok();
        }

        public class LoginCredentials
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
    }
}
