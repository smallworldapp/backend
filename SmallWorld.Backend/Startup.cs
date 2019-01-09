using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SharpRaven;
using SmallWorld.Converters.ModelBased;
using SmallWorld.Database;
using SmallWorld.Database.Entities;
using SmallWorld.Filters;
using SmallWorld.Formatters;
using SmallWorld.Library;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models.Abstractions;
using SmallWorld.Models.Emailing;
using SmallWorld.Models.Emailing.Abstractions;
using SmallWorld.Models.Impl;
using SmallWorld.Models.PairGenerators;
using SmallWorld.Models.Providers;

namespace SmallWorld
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            const string dir = ".";

            services.AddSmallWorldLibrary();
            services.AddSmallworldDatabase(dir);

            services.AddSingleton(new RavenClient("https://80d5d1f738a144019a763f795076f64a:a398de24654e4f84b2b20f84f7d40d63@sentry.io/235133"));

            services.AddSingleton<UpdateLoop>();
            services.AddSingleton<AuthProvider>();

            services.AddTransient<LinkProvider>();
            services.AddTransient<CryptoProvider>();

            services.AddTransient<PairGenerator>();
            services.AddTransient<ITelemetryProvider, TelemetryProvider>();

            services.AddScoped<EmailProvider>();
            var includes = typeof(IEmailInclude).FindTypes();
            foreach (var type in includes)
                services.AddTransient(type);

            var templates = typeof(IEmailTemplateBase).FindTypes();
            foreach (var type in templates)
                services.AddTransient(type);

            var converters = typeof(JsonConverter).FindTypes(typeof(JsonModel).Assembly);

            foreach (var type in converters)
                services.AddTransient(typeof(JsonConverter), type);

            services.AddCors();
            services.AddMvc(options => {
                options.Filters.Add<AccessLockFilter>();
                options.Filters.Add<ValidationExceptionFilter>();
                options.Filters.Add<TelemetryExceptionFilter>();

                options.InputFormatters.Insert(0, new MyInputFormatter());
                options.OutputFormatters.Insert(0, new MyOutputFormatter());
            });
        }

        public void Configure(IApplicationBuilder app, ILoggerFactory factory, IServiceProvider provider, SmallWorldOptions options)
        {
            var context = provider.GetService<IContext>();
            context.Initialize();

#if DEBUG
            var crypto = provider.GetService<CryptoProvider>();

            context.AcquireLock(true);

            var test = context.Set<Account>()
                .Include(a => a.Credentials)
                .Where(a => a.Email.Value.Contains("test") && a.Email.Value.EndsWith("@mfro.me"));

            foreach (var acc in test)
            {
                acc.Credentials.Salt = crypto.GenerateSalt();
                acc.Credentials.Hash = crypto.GenerateHash(acc.Credentials.Salt, "password");
            }

            context.Finish();
#endif

            factory.AddFile("logs/info.log");
            factory.AddFile("logs/errors.log", LogLevel.Warning);

            factory.AddDebug();
            factory.AddConsole(LogLevel.Warning);

            app.UseCors(cors => {
                cors.AllowAnyHeader();
                cors.AllowAnyMethod();
                cors.WithOrigins(options.Origins);
            });

            app.UseMvc();

            provider.GetService<UpdateLoop>().Start();
        }
    }
}