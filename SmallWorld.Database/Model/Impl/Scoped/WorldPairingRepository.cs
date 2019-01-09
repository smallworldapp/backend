using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Database.Model.Impl.Scoped
{
    public class WorldPairingRepository : PairingRepository
    {
        public World World { get; }

        public WorldPairingRepository(IServiceProvider provider, World world, IQueryable<Pairing> src = null) : base(provider, src ?? GetQueryable(provider, world))
        {
            World = world;

            Context.Entry(world).LoadRelations(w => w.Members);
        }

        public override void Add(Pairing pairing)
        {
            pairing.World = World;
            World.Pairings.Add(pairing);

            base.Add(pairing);
        }

        public override void Delete(Pairing pairing)
        {
            Entry(pairing)
                .LoadRelations(p => p.World.Pairings);

            if (pairing.World != World)
                throw new ArgumentException("Pairing is not in this repository");

            base.Delete(pairing);
        }

        protected override IPairingRepository Create(IQueryable<Pairing> chain) => new WorldPairingRepository(Provider, World, chain);

        private static IQueryable<Pairing> GetQueryable(IServiceProvider provider, World world)
        {
            var context = provider.GetRequiredService<IEntryRepository>();
            var entry = context.Entry(world);
            return entry.QueryRelation(w => w.Pairings);
        }
    }
}
