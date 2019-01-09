using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace SmallWorld.Database.Design
{
    public class DesignTimeFactory : IDesignTimeDbContextFactory<SmallWorldContext>
    {
        public SmallWorldContext CreateDbContext(string[] args)
        {
            var file = Path.GetFullPath(SmallWorldContext.File);

            var options = new DbContextOptionsBuilder<SmallWorldContext>()
                .UseSqlite($"Filename={file}")
                .Options;

            return new SmallWorldContext(options);
        }
    }
}
