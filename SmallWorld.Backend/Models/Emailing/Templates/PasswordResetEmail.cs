using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models.Emailing.Abstractions;
using SmallWorld.Models.Providers;

namespace SmallWorld.Models.Emailing.Templates
{
    public class PasswordResetEmail : EmailTemplate, IEmailTemplate<Account>
    {
        private readonly IEntryRepository entries;
        private readonly LinkProvider links;

        public PasswordResetEmail(IEntryRepository entries, LinkProvider links)
        {
            this.entries = entries;
            this.links = links;
        }

        public Email Create(Account account)
        {
            entries.Entry(account)
                .LoadRelations(a => a.ResetToken);

            Subject = "smallworld Password Reset";
            To(account);

            Write($@"Hi {account.Name},

Click this link to reset your password: [click here]({links.PasswordReset(account)})

-The smallworld Team
");

            return Finish();
        }
    }
}