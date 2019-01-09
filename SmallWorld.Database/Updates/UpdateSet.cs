using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Internal;

namespace SmallWorld.Database.Updates
{
    public interface IUpdateSet
    {
        bool IsReady(SmallWorldContext context);
        IEnumerable<IUpdate> Updates();
    }

    public abstract class UpdateSet<T> : IUpdateSet where T : Migration, new()
    {
        public bool IsReady(SmallWorldContext context)
        {
            var migration = new T();
            var id = migration.GetId();

            var migrations = context.Database.GetAppliedMigrations();

            return migrations.Contains(id);
        }

        public abstract IEnumerable<IUpdate> Updates();
    }
}