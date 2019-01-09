using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Database.Model.Impl.Scoped
{
    public class PairingPairRepository : PairRepository
    {
        public Pairing Pairing { get; }

        public PairingPairRepository(IServiceProvider provider, Pairing pairing, IQueryable<Pair> src = null) : base(provider, src ?? GetQueryable(provider, pairing))
        {
            Pairing = pairing;

            Context.Entry(pairing)
                .LoadRelations(w => w.Pairs)
                .LoadRelations(w => w.World);
        }

        public override void Add(Pair pair)
        {
            pair.World = Pairing.World;
            Pairing.World.Pairs.Add(pair);

            pair.Pairing = Pairing;
            Pairing.Pairs.Add(pair);

            base.Add(pair);
        }

        protected override IPairRepository Create(IQueryable<Pair> chain) => new PairingPairRepository(Provider, Pairing, chain);

        private static IQueryable<Pair> GetQueryable(IServiceProvider provider, Pairing pairing)
        {
            var context = provider.GetRequiredService<IEntryRepository>();
            var entry = context.Entry(pairing);
            return entry.QueryRelation(w => w.Pairs);
        }
    }
}
