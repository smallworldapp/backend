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
    public class PairingInitiatorEmail : EmailTemplate, IEmailTemplate<Pair>, IEmailTemplate<Member, Identity>
    {
        private readonly IEntryRepository entries;
        private readonly LinkProvider links;

        private readonly FeedbackRequest feedbackRequest;
        private readonly PersonalMessage personalMessage;
        private readonly OptOut optOut;

        public PairingInitiatorEmail(IServiceProvider services)
        {
            entries = services.GetService<IEntryRepository>();
            links = services.GetService<LinkProvider>();

            feedbackRequest = services.GetService<FeedbackRequest>();
            personalMessage = services.GetService<PersonalMessage>();
            optOut = services.GetService<OptOut>();
        }

        private void Construct(Member init, Identity recv, World world, Pairing pairing)
        {
            Subject = $"{init.World.Name} - smallworld Connection";
            To(init);

            Write($@"
{feedbackRequest.Create(init, pairing)}

Hi {init.FirstName},

You have been paired with {recv.FirstName} for your next smallworld
connection. You have been selected as the initiator. **As the initiator,
it is your responsibility to send the first email to set up your meeting.**

Your partner’s contact information is below. We suggest providing three or
more times to connect in your initial email.  

{recv.FirstName} {recv.LastName}  
{recv.Email}

{personalMessage.Create(pairing)}

-The smallworld Team

{optOut.Create(init)}

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

            Construct(pair.Initiator, pair.Receiver, pair.World, pair.Pairing);

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