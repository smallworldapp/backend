using System;
using System.Linq;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;

namespace SmallWorld.Database.Model.Impl
{
    public class PairRepository : BaseEntityRepository<Pair, IPairRepository>, IPairRepository
    {
        public PairRepository(IServiceProvider provider) : this(provider, null) { }
        public PairRepository(IServiceProvider provider, IQueryable<Pair> src) : base(provider, src) { }

        protected override IPairRepository Create(IQueryable<Pair> chain) => new PairRepository(Provider, chain);
    }
}
