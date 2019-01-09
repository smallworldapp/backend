using System;
using System.Linq;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Database.Model.Impl.Scoped;

namespace SmallWorld.Database.Model.Impl
{
    public class PairingRepository : BaseEntityRepository<Pairing, IPairingRepository>, IPairingRepository
    {
        public PairingRepository(IServiceProvider provider) : this(provider, null) { }
        public PairingRepository(IServiceProvider provider, IQueryable<Pairing> src) : base(provider, src) { }

        public virtual void Delete(Pairing pairing)
        {
            Entry(pairing)
                .LoadRelations(p => p.Pairs)
                .LoadRelations(p => p.World.Pairings);

            foreach (var pair in pairing.Pairs)
            {
                Context.Remove(pair);
                pairing.World.Pairs?.Remove(pair);
            }

            Context.Remove(pairing);
            pairing.World.Pairings.Remove(pairing);
        }

        public IPairRepository Pairs(Pairing pairing)
        {
            return new PairingPairRepository(Provider, pairing);
        }

        protected override IPairingRepository Create(IQueryable<Pairing> chain) => new PairingRepository(Provider, chain);
    }
}
