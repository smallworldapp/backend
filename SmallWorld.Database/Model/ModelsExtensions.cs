using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Database.Model.Impl;

namespace SmallWorld.Database.Model
{
    public static class ModelsExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IPairingRepository, PairingRepository>();
            services.AddScoped<IPairRepository, PairRepository>();
            services.AddScoped<IWorldRepository, WorldRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();

            return services;
        }
    }
}
