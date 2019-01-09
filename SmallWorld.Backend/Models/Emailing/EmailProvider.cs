using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models.Emailing.Abstractions;

namespace SmallWorld.Models.Emailing
{
    public class EmailProvider
    {
        private readonly IServiceProvider services;
        private readonly IContextLock access;
        private readonly IEmailRepository emails;
        private readonly SmallWorldOptions options;

        public EmailProvider(IServiceProvider services, IContextLock access, SmallWorldOptions options, IEmailRepository emails)
        {
            this.services = services;
            this.options = options;
            this.emails = emails;
            this.access = access;
        }

        public void Send<TEmail>(Emails.GenericReference<TEmail> handle) where TEmail : IEmailTemplate
        {
            var email = services.GetService<TEmail>();
            var msg = email.Create();

            Enqueue(msg);
        }

        public void Send<TEmail, T1>(Emails.GenericReference<TEmail> handle, T1 t1) where TEmail : IEmailTemplate<T1>
        {
            var email = services.GetService<TEmail>();
            var msg = email.Create(t1);

            Enqueue(msg);
        }

        public void Send<TEmail, T1, T2>(Emails.GenericReference<TEmail> handle, T1 t1, T2 t2) where TEmail : IEmailTemplate<T1, T2>
        {
            var email = services.GetService<TEmail>();
            var msg = email.Create(t1, t2);

            Enqueue(msg);
        }

        public void Send<TEmail, T1, T2, T3>(Emails.GenericReference<TEmail> handle, T1 t1, T2 t2, T3 t3) where TEmail : IEmailTemplate<T1, T2, T3>
        {
            var email = services.GetService<TEmail>();
            var msg = email.Create(t1, t2, t3);

            Enqueue(msg);
        }

        private void Enqueue(Email email)
        {
            access.CheckWritable();
            emails.Add(email);
        }

        public async Task Flush(int maxMillis)
        {
            var timer = new Stopwatch();
            timer.Start();

            while (timer.ElapsedMilliseconds < maxMillis)
            {
                var email = await Next();
                if (email == null)
                    break;

                var mime = new MailMessage {
                    Subject = email.Subject,
                    IsBodyHtml = true,
                    Body = email.Body,
                    From = new MailAddress(options.Smtp.Email, options.Smtp.Name),
                };

                foreach (var recipient in email.Recipients)
                {
                    mime.To.Add(new MailAddress(recipient.Address.Value, recipient.Name.Value));
                }

                Send(mime);

                using (var handle = await access.Write())
                {
                    email.IsSent = true;
                    email.Sent = DateTime.UtcNow;
                    await handle.Finish();
                }
            }
        }

        private async Task<Email> Next()
        {
            using (await access.Read())
            {
                var unsent = from mail in emails.Include(e => e.Recipients).All
                             where !mail.IsSent
                             orderby mail.Created ascending
                             select mail;

                return unsent.FirstOrDefault();
            }
        }

        private void Send(MailMessage mail)
        {
            Console.WriteLine($"Sending email to {string.Join(", ", mail.To.Select(u => u.Address))}");

#if DEBUG
            var _ = new NetworkCredential();
            Console.WriteLine($"Ignoring email: {mail.Subject} to {string.Join(", ", mail.To.Select(u => u.Address))}");
#else
            using (var client = new SmtpClient())
            {
                client.Credentials = new NetworkCredential(options.Smtp.Username, options.Smtp.Password);
                client.Host = options.Smtp.Host;
                client.Port = options.Smtp.Port;
                client.EnableSsl = true;

                client.Send(mail);
            }
#endif
        }
    }
}
