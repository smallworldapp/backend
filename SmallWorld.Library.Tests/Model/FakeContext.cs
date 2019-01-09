using System;
using System.Linq;
using System.Threading.Tasks;
using SmallWorld.Library.Model.Abstractions;
using Xunit;

namespace SmallWorld.Library.Tests.Model
{
    public class FakeContext : IContext
    {
        public bool? IsWriting { get; private set; }
        public bool IsInvalidated { get; private set; }

        public Task Initialize() => throw new NotImplementedException();

        public IEntry<T> Entry<T>(T entity) where T : class => throw new NotImplementedException();

        public IQueryable<T> Set<T>() where T : class => throw new NotImplementedException();

        public IEntry<T> Add<T>(T entity) where T : class => throw new NotImplementedException();

        public IEntry<T> Update<T>(T entity) where T : class => throw new NotImplementedException();

        public IEntry<T> Remove<T>(T entity) where T : class => throw new NotImplementedException();

        public Task AcquireLock(bool writable)
        {
            IsWriting = writable;
            return Task.CompletedTask;
        }

        public Task Finish()
        {
            IsWriting = null;
            return Task.CompletedTask;
        }

        public void Release()
        {
            if (IsWriting == true)
                IsInvalidated = true;

            IsWriting = null;
        }

        public void Dispose()
        {
            Assert.Null(IsWriting);
        }
    }
}
