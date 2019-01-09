using System;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Models.Emailing.Abstractions;
using SmallWorld.Models.Providers;

namespace SmallWorld.Models.Emailing.Templates
{
    public class AccountCreationEmail : EmailTemplate, IEmailTemplate<Account, string>
    {
        private readonly LinkProvider links;

        public AccountCreationEmail(IServiceProvider services)
        {
            links = services.GetService<LinkProvider>();
        }

        public Email Create(Account account, string password)
        {
            Subject = "smallworld Account Created";
            To(account);

            Write($@"Hi {account.Name},

A smallworld administrator account has been created for you. Your login
credentials are below:  
Email: {account.Email}  
Password: {password}

To login to your account please click [this link]({links.AdminConsole}).

-The smallworld Team
");

            return Finish();
        }
    }
}
