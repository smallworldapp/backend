using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;
using SmallWorld.Models.Abstractions;
using SmallWorld.Models.Emailing;
using SmallWorld.Models.PairGenerators;

namespace SmallWorld
{
    public class UpdateRunner
    {
        private readonly EmailProvider emails;
        private readonly PairGenerator generator;
        private readonly IWorldRepository worlds;
        private readonly IContextLock access;
        private readonly ITelemetryProvider telemetry;

        public UpdateRunner(PairGenerator generator, IWorldRepository worlds, IContextLock access, EmailProvider emails, IServiceProvider provider)
        {
            this.generator = generator;
            this.worlds = worlds;
            this.access = access;
            this.emails = emails;

            telemetry = provider.GetService<ITelemetryProvider>();
        }

        private void HandleException(string message, Exception x)
        {
            Console.WriteLine(message);
            Console.WriteLine(x);

            //            using (var handle = await access.Write())
            //            {
            //                emails.Send(Emails.Debug, x.ToString());
            //                await handle.Finish();
            //            }
            telemetry?.HandleException(x);
        }

        public async Task Run(int tickRate)
        {
            try
            {
                CreatePairings();
            }
            catch (Exception x)
            {
                HandleException("Error while creating pairings: ", x);
            }

            try
            {
                var pairs = await Generate();

                await SubmitPairings(pairs);
            }
            catch (Exception x)
            {
                HandleException("Error while creating pairs: ", x);
            }

            try
            {
                await emails.Flush(tickRate);
            }
            catch (Exception x)
            {
                HandleException("Error while sending emails: ", x);
            }
        }

        private async Task SubmitPairings(IEnumerable<GeneratedPairs> pairs)
        {
            foreach (var group in pairs)
            {
                await group.Submit(access);
            }
        }

        private async Task<IEnumerable<GeneratedPairs>> Generate()
        {
            using (await access.Read())
            {
                var ready = from world in worlds.NotDeleted
                            where world.Account.Status != AccountStatus.Deleted

                            from pairing in world.Pairings
                            where !pairing.IsComplete && pairing.Date < DateTime.UtcNow

                            select pairing;

                return ready.ToList().Select(generator.For).ToList();
            }
        }

        private void CreatePairings()
        {
            // var ready = from world in worlds.GetAll()
            //             where world.Status != WorldStatus.Deleted &&
            //                   world.Account.Status != AccountStatus.Deleted &&
            //                   world.PairingSettings.Enabled
            //             select world;

            // foreach (var world in ready) {
            //     // If the start date has not yet been reached, do nothing
            //     if (DateTime.Now < world.PairingSettings.Start)
            //         continue;

            //     // Calculate the first pairing date after right now
            //     var next = world.PairingSettings.Start;
            //     while (next < DateTime.Now) {
            //         next = next.AddMilliseconds(world.PairingSettings.Period);
            //     }
            //     next.AddMilliseconds(-world.PairingSettings.Period);

            //     // If that date has already been created, do nothing
            //     if (next == world.PairingSettings.MostRecent)
            //         continue;

            //     // Create the pairing
            //     var pairing = new Pairing {
            //         Date = next
            //     };
            //     pairings.Create(world, pairing);

            //     // Save the pairing date
            //     world.PairingSettings.MostRecent = next;
            // }
        }
    }
}
