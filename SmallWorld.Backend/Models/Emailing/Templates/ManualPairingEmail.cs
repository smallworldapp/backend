using SmallWorld.Database.Entities;
using SmallWorld.Models.Emailing.Abstractions;

namespace SmallWorld.Models.Emailing.Templates
{
    public class ManualPairingEmail : EmailTemplate, IEmailTemplate<Identity, Identity, string>
    {
        public Email Create(Identity init, Identity recv, string subject)
        {
            Subject = subject;
            To(init);
            To(recv);

            Write($@"Hi {init.FirstName} and {recv.FirstName},

You have been paired together for your next smallworld connection.

{init.FirstName} {init.LastName} is the initiator, as the initiator it
is {init.FirstName}'s responsibility to send the first email to set up your
meeting. We suggest providing three or more times to connect in your initial email.

-The smallworld Team
");
            return Finish();
        }
    }
}