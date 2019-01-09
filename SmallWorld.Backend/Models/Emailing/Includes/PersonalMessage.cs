using SmallWorld.Database.Entities;
using SmallWorld.Models.Emailing.Abstractions;

namespace SmallWorld.Models.Emailing.Includes
{
    public class PersonalMessage : IEmailInclude
    {
        public string Create(Pairing pairing)
        {
            if (pairing?.Message == null)
                return "";

            return $@"Below is a personalized message from your community administrator:

{pairing.Message}";
        }
    }
}
