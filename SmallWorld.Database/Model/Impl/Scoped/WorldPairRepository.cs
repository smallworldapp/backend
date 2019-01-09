using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Database.Model.Impl.Scoped
{
    public class WorldPairRepository : PairRepository
    {
        public World World { get; }

        public WorldPairRepository(IServiceProvider provider, World world, IQueryable<Pair> src = null) : base(provider, src ?? GetQueryable(provider, world))
        {
            World = world;

            Context.Entry(world)
                .LoadRelations(w => w.Pairs);
        }

        public override void Add(Pair pair)
        {
            throw new InvalidOperationException();
        }

        protected override IPairRepository Create(IQueryable<Pair> chain) => new WorldPairRepository(Provider, World, chain);

        private static IQueryable<Pair> GetQueryable(IServiceProvider provider, World world)
        {
            var context = provider.GetRequiredService<IEntryRepository>();
            var entry = context.Entry(world);
            return entry.QueryRelation(w => w.Pairs);
        }
    }
}
