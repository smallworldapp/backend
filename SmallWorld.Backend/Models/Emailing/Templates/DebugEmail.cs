using System;
using System.Collections.Generic;
using SmallWorld.Database.Entities;
using SmallWorld.Models.Emailing.Abstractions;

namespace SmallWorld.Models.Emailing.Templates
{
    public class DebugEmail : IEmailTemplate<string>
    {
        public Email Create(string contents)
        {
            var recipient = new EmailRecipient {
                Name = new Name("Max Froehlich"),
                Address = new EmailAddress("cooldudef40@gmail.com"),
            };
            recipient.CreateIds();

            var email = new Email {
                Subject = "smallworld Debug",
                Body = "<pre>" + contents + "</pre>",
                Recipients = new List<EmailRecipient> { recipient },
                Created = DateTime.UtcNow
            };

            return email;
        }
    }
}
