using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models.Emailing.Abstractions;
using SmallWorld.Models.Providers;

namespace SmallWorld.Models.Emailing.Templates
{
    public class WorldAcceptedEmail : EmailTemplate, IEmailTemplate<World>
    {
        private readonly IEntryRepository entries;
        private readonly LinkProvider links;

        public WorldAcceptedEmail(IEntryRepository entries, LinkProvider links)
        {
            this.entries = entries;
            this.links = links;
        }

        public Email Create(World world)
        {
            entries.Entry(world)
                .LoadRelations(w => w.Account);

            Subject = "smallworld Community Accepted";
            To(world.Account);

            Write($@"Hi {world.Account.Name},

We have reviewed the smallworld application for {world.Name} and have granted you access to proceed. Below are descriptions of the pages on the admin site.

Summary: Find information about your community and set up connections.  
Invite Page: A default Invite Page is provided and we encourage you to customize it for you community.  
Members: The members of your community.  
Settings: Change settings and download connection history.  

To login to your account please click [this link]({links.AdminConsole}).

-The smallworld Team
");

            return Finish();
        }
    }
}