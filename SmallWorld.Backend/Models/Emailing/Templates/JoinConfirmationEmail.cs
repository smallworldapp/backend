using System;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Models.Emailing.Abstractions;
using SmallWorld.Models.Providers;

namespace SmallWorld.Models.Emailing.Templates
{
    public class JoinConfirmationEmail : EmailTemplate, IEmailTemplate<Member>
    {
        private readonly LinkProvider links;

        public JoinConfirmationEmail(IServiceProvider services)
        {
            links = services.GetService<LinkProvider>();
        }

        public Email Create(Member member)
        {
            Subject = "smallworld Confirmation";
            To(member);

            Write($@"Hi {member.FirstName},

Thank you for joining smallworld!

To confirm your email address please click [this link]({links.JoinConfirmation(member)}).

-The smallworld Team
");

            return Finish();
        }
    }
}
