using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Model;
using SmallWorld.Database.Model.Impl;
using SmallWorld.Database.Validators;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Database
{
    public static class SmallWorldDatabaseExtensions
    {
        public static IServiceCollection AddSmallworldDatabase(this IServiceCollection services, string directory)
        {
            services.AddValidators();
            services.AddRepositories();

            services.AddSmallWorldContext(directory);

            return services;
        }

        public static IServiceCollection AddSmallWorldContext(this IServiceCollection services, string directory)
        {
            var file = Path.Combine(directory ?? ".", SmallWorldContext.File);
            file = Path.GetFullPath(file);

            var options = new DbContextOptionsBuilder<SmallWorldContext>()
                .UseSqlite($"Filename={file}")
                .Options;

            services.AddSmallWorldContext(options);

            return services;
        }

        public static IServiceCollection AddSmallWorldContext(this IServiceCollection services, DbContextOptions<SmallWorldContext> options)
        {
            services.AddSingleton(options);

            services.AddScoped<SmallWorldContext>();
            services.AddScoped<DbContext>(p => p.GetService<SmallWorldContext>());

            services.AddScoped<IContext, Context>();

            return services;
        }
    }
}
