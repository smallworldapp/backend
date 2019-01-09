using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace SmallWorld
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var options = JObject.Parse(File.ReadAllText("config.json")).ToObject<SmallWorldOptions>();

            var host = new WebHostBuilder()
                .ConfigureServices(services => services.AddSingleton(options))
                .UseStartup<Startup>()
                .UseKestrel()
                .UseUrls($"http://+:{options.Port}/")
                .Build();

            host.Run();
        }
    }
}