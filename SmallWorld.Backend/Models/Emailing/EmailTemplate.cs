using System;
using System.Collections.Generic;
using System.Xml.Linq;
using HeyRed.MarkdownSharp;
using SmallWorld.Database.Entities;

namespace SmallWorld.Models.Emailing
{
    public abstract class EmailTemplate
    {
        private static readonly Markdown Markdown = new Markdown();

        private readonly List<EmailRecipient> to = new List<EmailRecipient>();
        private string content;

        protected string Subject { get; set; }

        protected void To(Account acc) => To(acc.Name, acc.Email);
        protected void To(Identity acc) => To(acc.FullName(), acc.Email);
        protected void To(Name name, EmailAddress address)
        {
            var recipient = new EmailRecipient {
                Address = address,
                Name = name,
            };
            recipient.CreateIds();

            to.Add(recipient);
        }

        protected void Write(string str)
        {
            content += $"\r\n{str}\r\n";
        }

        protected Email Finish()
        {
            content = content.Trim();

            var xml = XElement.Parse("<body>" + Markdown.Transform(content) + "</body>");

            foreach (var element in xml.DescendantsAndSelf())
            {
                if (Modifiers.TryGetValue(element.Name.LocalName, out Action<XElement> modifier))
                    modifier(element);
            }

            content = xml.ToString(SaveOptions.DisableFormatting);

            var email = new Email {
                Subject = Subject,
                Body = Markdown.Transform(content),
                Recipients = to,
                Created = DateTime.UtcNow,

                IsSent = false,
                Sent = null,
            };

            return email;
        }

        private static readonly Dictionary<string, Action<XElement>> Modifiers = new Dictionary<string, Action<XElement>>(StringComparer.Ordinal) {
            ["p"] = element =>
            {
                var content = new XElement("span", element.Nodes());
                element.RemoveAll();
                element.Add(content);

                content.SetAttributeValue("style", "font-size:14.6667px;font-family:arial;background-color:transparent;vertical-align:baseline;white-space:normal");

                element.SetAttributeValue("style", "line-height:1.38;margin-top:0pt;margin-bottom:1em");
                element.SetAttributeValue("dir", "ltr");
            }
        };
    }
}
