using System;
using System.Linq;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Database.Model.Impl.Scoped;
using SmallWorld.Library.Model;

namespace SmallWorld.Database.Model.Impl
{
    public class WorldRepository : BaseEntityRepository<World, IWorldRepository>, IWorldRepository
    {
        public IQueryable<World> NotDeleted => All
            .Where(a => a.Status != WorldStatus.Deleted);

        public WorldRepository(IServiceProvider provider) : this(provider, null) { }
        public WorldRepository(IServiceProvider provider, IQueryable<World> src) : base(provider, src) { }

        public override Optional<World> Find(Func<World, bool> filter) => Find(filter, false);

        public bool Find(Identifier identifier, out World acc) => Find(identifier).Exists(out acc);
        public Optional<World> Find(Identifier identifier) => Find(a => a.Identifier == identifier);

        public bool Find(Guid id, bool includeDeleted, out World acc) => Find(id, includeDeleted).Exists(out acc);
        public Optional<World> Find(Guid id, bool includeDeleted) => Find(a => a.Guid == id, includeDeleted);

        public bool Find(Func<World, bool> filter, bool includeDeleted, out World acc) => Find(filter, includeDeleted).Exists(out acc);
        public Optional<World> Find(Func<World, bool> filter, bool includeDeleted)
        {
            var src = includeDeleted ? All : NotDeleted;

            var value = src.SingleOrDefault(filter);
            return new Optional<World>(value != null, value);
        }

        public IPairRepository Pairs(World world)
        {
            return new WorldPairRepository(Provider, world);
        }

        public IMemberRepository Members(World world)
        {
            return new WorldMemberRepository(Provider, world);
        }

        public IPairingRepository Pairings(World world)
        {
            return new WorldPairingRepository(Provider, world);
        }

        protected override IWorldRepository Create(IQueryable<World> chain) => new WorldRepository(Provider, chain);
    }
}