using System;
using System.Linq;
using SmallWorld.Database.Entities;
using SmallWorld.Library.Model;

namespace SmallWorld.Database.Model.Abstractions
{
    public interface IWorldRepository : IBaseEntityRepository<World, IWorldRepository>
    {
        IQueryable<World> NotDeleted { get; }

        bool Find(Identifier identifier, out World acc);
        Optional<World> Find(Identifier identifier);

        bool Find(Guid id, bool includeDeleted, out World acc);
        Optional<World> Find(Guid id, bool includeDeleted);

        bool Find(Func<World, bool> filter, bool includeDeleted, out World acc);
        Optional<World> Find(Func<World, bool> filter, bool includeDeleted);

        IPairRepository Pairs(World world);

        IMemberRepository Members(World world);
        IPairingRepository Pairings(World world);
    }
}
