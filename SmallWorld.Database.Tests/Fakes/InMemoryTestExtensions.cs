using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Library.Model.Impl;

namespace SmallWorld.Database.Tests.Fakes
{
    public static class InMemoryTestExtensions
    {
        public static IServiceCollection AddInMemoryContext(this IServiceCollection services)
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<SmallWorldContext>()
                .UseSqlite(connection)
                .Options;

            services.AddSmallWorldContext(options);
            services.AddScoped<IContextLock, ContextLock>();
            services.AddScoped<IEntryRepository, EntryRepository>();

            return services;
        }
    }
}
