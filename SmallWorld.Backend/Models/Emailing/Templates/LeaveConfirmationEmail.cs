using System;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Models.Emailing.Abstractions;
using SmallWorld.Models.Providers;

namespace SmallWorld.Models.Emailing.Templates
{
    public class LeaveConfirmationEmail : EmailTemplate, IEmailTemplate<Member>
    {
        private readonly LinkProvider links;

        public LeaveConfirmationEmail(IServiceProvider services)
        {
            links = services.GetService<LinkProvider>();
        }

        public Email Create(Member member)
        {
            Subject = "smallworld Confirmation";
            To(member);

            Write($@"Hi {member.FirstName},

To leave smallworld please click click [this link]({links.LeaveConfirmation(member)}).

-The SmallWorld Team
");
            return Finish();
        }
    }
}
