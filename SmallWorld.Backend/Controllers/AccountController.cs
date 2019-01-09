using System;
using Microsoft.AspNetCore.Mvc;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Filters;

namespace SmallWorld.Controllers
{
    [Route("accounts/{id}")]
    public class AccountController : MyController
    {
        private readonly IAccountRepository accounts;

        public AccountController(IAccountRepository accounts)
        {
            this.accounts = accounts;
        }

        [HttpGet]
        [AuthRequired]
        public IActionResult GetAccount(Guid id)
        {
            if (!accounts.Find(id, out var acc))
                return NotFound();

            if (!Permissions.AccessAccount(acc))
                return NotFound();

            return Ok(acc);
        }

        [HttpPatch]
        [AuthRequired]
        [DatabaseUpdate]
        public IActionResult UpdateAccount(Guid id, [FromBody] Account account)
        {
            if (!accounts.Find(id, out var acc))
                return NotFound();

            if (!Permissions.ModifyAccount(acc))
                return NotFound();

            if (account.Name != null)
                acc.Name = account.Name;

            accounts.Update(acc);

            return Ok(acc);
        }

        [HttpDelete]
        [AuthRequired]
        [DatabaseUpdate]
        public IActionResult DeleteAccount(Guid id)
        {
            if (!accounts.Find(id, out var acc))
                return NotFound();

            if (!Permissions.DeleteAccount(acc))
                return NotFound();

            acc.Status = AccountStatus.Deleted;

            accounts.Update(acc);

            return Ok();
        }
    }
}
