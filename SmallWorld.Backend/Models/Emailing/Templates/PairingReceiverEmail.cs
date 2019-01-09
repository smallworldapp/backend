using System;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models.Emailing.Includes;
using SmallWorld.Models.Emailing.Abstractions;
using SmallWorld.Models.Providers;

namespace SmallWorld.Models.Emailing.Templates
{
    public class PairingReceiverEmail : EmailTemplate, IEmailTemplate<Pair>, IEmailTemplate<Member, Identity>
    {
        private readonly IEntryRepository entries;
        private readonly LinkProvider links;

        private readonly FeedbackRequest feedbackRequest;
        private readonly PersonalMessage personalMessage;

        private readonly OptOut optOut;

        public PairingReceiverEmail(IServiceProvider services)
        {
            entries = services.GetService<IEntryRepository>();
            links = services.GetService<LinkProvider>();

            feedbackRequest = services.GetService<FeedbackRequest>();
            personalMessage = services.GetService<PersonalMessage>();
            optOut = services.GetService<OptOut>();
        }

        private void Construct(Identity init, Identity recv, World world, Pairing pairing)
        {
            Subject = $"{world.Name} - smallworld Connection";
            To(recv);

            var member = recv as Member;

            Write($@"
{feedbackRequest.Create(member, pairing)}

Hi {recv.FirstName},

You have been paired with {init.FullName()} for your next smallworld
connection. {init.FirstName} has been selected as the initiator. Since
everyone is busy from time to time, we suggest contacting your partner
if you have not heard from them within 48 hours of the initial pairing email.

Your partner’s contact information is below. We suggest providing three or
more times to connect in your initial email.

{init.FirstName} {init.LastName}  
{init.Email}

{personalMessage.Create(pairing)}

-The smallworld Team

{optOut.Create(member)}

Use the following link to leave/join your smallworld group at any time: [click here]({links.InvitePage(world)})
");
        }

        public Email Create(Pair pair)
        {
            var entry = entries.Entry(pair);

            entry.LoadRelations(p => p.World);
            entry.LoadRelations(p => p.Pairing);
            entry.LoadRelations(p => p.Initiator);
            entry.LoadRelations(p => p.Receiver);

            Construct(pair.Initiator, pair.Receiver, pair.Receiver.World, pair.Pairing);

            return Finish();
        }

        public Email Create(Member init, Identity recv)
        {
            var entry = entries.Entry(init);

            entry.LoadRelations(p => p.World);

            Construct(init, recv, init.World, null);

            return Finish();
        }
    }
}