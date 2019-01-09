using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SmallWorld.Library.Model.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace SmallWorld.Database.Model.Impl
{
    public class Context : IContext
    {
        private static uint counter;

        private readonly uint number;

        private bool? isWriting;
        private bool isInvalidated;
        private SmallWorldContext context;

        public Context(SmallWorldContext context)
        {
            this.context = context;
            number = counter++;

            Debug.WriteLine($"Creating context {number}");
        }

        public async Task Initialize()
        {
            await AcquireLock(true);

            context.Migrate();

            await Finish();
        }

        public IEntry<T> Entry<T>(T entity) where T : class
        {
            Debug.Assert(!isInvalidated, "Using invalidated Context");
            Debug.Assert(isWriting.HasValue, "Using unlocked Context");

            return new Entry<T>(context.Entry(entity));
        }

        public IQueryable<T> Set<T>() where T : class
        {
            Debug.Assert(!isInvalidated, "Using invalidated Context");
            Debug.Assert(isWriting.HasValue, "Using unlocked Context");

            var set = context.Set<T>();
            return set;
        }

        public IEntry<T> Add<T>(T entity) where T : class
        {
            Debug.Assert(!isInvalidated, "Using invalidated Context");
            Debug.Assert(isWriting == true, "Modifying non-writeable Context");

            return new Entry<T>(context.Add(entity));
        }

        public IEntry<T> Update<T>(T entity) where T : class
        {
            Debug.Assert(!isInvalidated, "Using invalidated Context");
            Debug.Assert(isWriting == true, "Modifying non-writeable Context");

            return new Entry<T>(context.Update(entity));
        }

        public IEntry<T> Remove<T>(T entity) where T : class
        {
            Debug.Assert(!isInvalidated, "Using invalidated Context");
            Debug.Assert(isWriting == true, "Modifying non-writeable Context");

            return new Entry<T>(context.Remove(entity));
        }

        private TaskCompletionSource<object> read;

        private static readonly SemaphoreSlim access = new SemaphoreSlim(1);
        private static readonly List<Task> readOperations = new List<Task>();

        public async Task AcquireLock(bool writable)
        {
            await access.WaitAsync();

            if (writable)
            {
                readOperations.RemoveAll(t => t.IsCompleted);
                await Task.WhenAll(readOperations);
            }
            else
            {
                read = new TaskCompletionSource<object>();
                readOperations.Add(read.Task);
                access.Release();
            }

            isWriting = writable;
        }

        public async Task Finish()
        {
            if (isWriting == true)
            {
                await context.SaveChangesAsync();
                access.Release();
            }
            else
            {
                read.SetResult(null);
                read = null;
            }

            isWriting = null;
        }

        public void Release()
        {
            if (isWriting == true)
            {
                isInvalidated = true;
                context.Dispose();
                context = null;
                access.Release();
            }
            else
            {
                read.SetResult(null);
                read = null;
            }

            isWriting = null;
        }

        public void Dispose()
        {
            Debug.Assert(!isWriting.HasValue, "Disposing still-locked Context");

            Debug.WriteLine($"Destroyed context {number}");
        }
    }
}
