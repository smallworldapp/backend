using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Library.Model.Impl
{
    public class EntryRepository : IEntryRepository
    {
        private readonly IContext context;

        public EntryRepository(IContext context)
        {
            this.context = context;
        }

        public IEntry<T> Entry<T>(T t) where T : class => context.Entry(t);
    }
}
