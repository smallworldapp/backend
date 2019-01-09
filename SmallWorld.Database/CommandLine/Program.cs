using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Database.CommandLine
{
    internal class Program
    {
        private static void Usage()
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("    update [directory]");
            Console.WriteLine("    backup [directory]");
        }

        private static void Main()
        {
#if DEBUG
            var services = new ServiceCollection();
            services.AddSmallworldDatabase(@"C:\Users\Max\Desktop\fucked up shit\smallworld migratoin 11-8");

            using (var provider = services.BuildServiceProvider())
                using (var context = provider.GetService<IContext>())
                    context.Initialize();
#else
            var args = new ArgQueue();

            string directory;
            switch (args.Read())
            {
                case "update":
                    directory = args.Read() ?? ".";

                    Backup(directory);
                    Migrate(directory).Wait();
                    break;

                case "backup":
                    directory = args.Read() ?? ".";

                    Backup(directory);
                    break;

                default:
                    Usage();
                    break;
            }
#endif
        }

        private static async Task Migrate(string directory)
        {
            var services = new ServiceCollection();
            services.AddSmallWorldContext(directory);

            using (var provider = services.BuildServiceProvider())
            using (var ctx = provider.GetService<IContext>())
                await ctx.Initialize();
        }

        private static void Backup(string directory)
        {
            var src = Path.Combine(directory, SmallWorldContext.File);

            if (!File.Exists(src)) return;

            var now = DateTime.UtcNow;

            var dstDir = Path.Combine(directory, "smallworld-backups", now.ToString("yyyy-M-d"));
            Directory.CreateDirectory(dstDir);

            var dst = Path.Combine(dstDir, $"{now:HH-mm-ss}.sqlite");
            File.Copy(src, dst);

            Console.WriteLine($"Backup saved to {dst}");
        }
    }
}