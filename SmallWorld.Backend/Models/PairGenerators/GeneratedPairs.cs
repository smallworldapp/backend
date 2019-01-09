using System.Threading.Tasks;
using SmallWorld.Database.Entities;
using SmallWorld.Database.Model.Abstractions;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Models.PairGenerators
{
    public abstract class GeneratedPairs
    {
        public abstract void Generate(Pairing pairing);

        public abstract Task Submit(IContextLock access);
    }
}
