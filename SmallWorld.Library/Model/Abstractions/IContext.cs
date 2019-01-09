using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmallWorld.Library.Model.Abstractions
{
    public interface IContext : IDisposable
    {
        Task Initialize();

        IEntry<T> Entry<T>(T entity) where T : class;

        IQueryable<T> Set<T>() where T : class;

        IEntry<T> Add<T>(T entity) where T : class;

        IEntry<T> Update<T>(T entity) where T : class;

        IEntry<T> Remove<T>(T entity) where T : class;

        Task AcquireLock(bool writable);

        Task Finish();

        void Release();
    }
}