using SmallWorld.Database.Entities;

namespace SmallWorld.Database.Model.Abstractions
{
    public interface IPairingRepository : IBaseEntityRepository<Pairing, IPairingRepository>
    {
        void Delete(Pairing pairing);

        IPairRepository Pairs(Pairing pairing);
    }
}
