using SmallWorld.Database.Entities;
using SmallWorld.Models.Emailing.Abstractions;

namespace SmallWorld.Models.Emailing.Includes
{
    public class OptOut : IEmailInclude
    {
        public OptOut() { }

        public string Create(Member t1)
        {
            return "";
        }
    }
}
