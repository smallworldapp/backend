//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading;
//using System.Threading.Tasks;
//using SmallWorld.Library.Model.Abstractions;
//
//namespace SmallWorld.Database.Tests.Fakes.Model
//{
//    public class FakeContext : IContext
//    {
//        private static uint counter;
//
//        private readonly uint number;
//
//        private bool? isWriting;
//        private bool isInvalidated;
//
//        public Dictionary<Type, object> Tables { get; private set; }= new Dictionary<Type, object>();
//
//        public List<object> Added { get; } = new List<object>();
//        public List<object> Updated { get; } = new List<object>();
//        public List<object> Removed { get; } = new List<object>();
//
//        public FakeContext()
//        {
//            number = counter++;
//
//            Debug.WriteLine($"Creating context {number}");
//        }
//
//        public async Task Initialize()
//        {
//            await AcquireLock(true);
//
//            await Finish();
//        }
//
//        private IList GetTable(Type t)
//        {
//            if (!Tables.TryGetValue(t, out var table))
//                table = Activator.CreateInstance(typeof(List<>).MakeGenericType(t));
//
//            return (IList)table;
//        }
//
//        private IList<T> GetTable<T>()
//        {
//            if (!Tables.TryGetValue(typeof(T), out var table))
//                table = new List<T>();
//
//            return (IList<T>)table;
//        }
//
//        private void Save()
//        {
//            foreach (var o in Added)
//                GetTable(o.GetType()).Add(o);
//
//            foreach (var o in Removed)
//            {
//                var table = GetTable(o.GetType());
//                if (!table.Contains(o))
//                    throw new InvalidOperationException("Removed untracked entity");
//                table.Remove(o);
//            }
//
//            Added.Clear();
//            Updated.Clear();
//            Removed.Clear();
//        }
//
//        public IEntry<T> Entry<T>(T entity) where T : class
//        {
//            Debug.Assert(!isInvalidated, "Using invalidated Context");
//            Debug.Assert(isWriting.HasValue, "Using unlocked Context");
//
//            return new FakeEntry<T>(entity);
//        }
//
//        public IQueryable<T> Set<T>() where T : class
//        {
//            Debug.Assert(!isInvalidated, "Using invalidated Context");
//            Debug.Assert(isWriting.HasValue, "Using unlocked Context");
//
//            return new EnumerableQuery<T>(GetTable<T>());
//        }
//
//        public IEntry<T> Add<T>(T entity) where T : class
//        {
//            Debug.Assert(!isInvalidated, "Using invalidated Context");
//            Debug.Assert(isWriting.HasValue, "Using unlocked Context");
//
//            Added.Add(entity);
//            return new FakeEntry<T>(entity);
//        }
//
//        public IEntry<T> Update<T>(T entity) where T : class
//        {
//            Debug.Assert(!isInvalidated, "Using invalidated Context");
//            Debug.Assert(isWriting.HasValue, "Using unlocked Context");
//
//            Updated.Add(entity);
//            return new FakeEntry<T>(entity);
//        }
//
//        public IEntry<T> Remove<T>(T entity) where T : class
//        {
//            Debug.Assert(!isInvalidated, "Using invalidated Context");
//            Debug.Assert(isWriting.HasValue, "Using unlocked Context");
//
//            Removed.Add(entity);
//            return new FakeEntry<T>(entity);
//        }
//
//        private TaskCompletionSource<object> read;
//
//        private static readonly SemaphoreSlim access = new SemaphoreSlim(1);
//        private static readonly List<Task> readOperations = new List<Task>();
//
//        public async Task AcquireLock(bool writable)
//        {
//            await access.WaitAsync();
//
//            if (writable)
//            {
//                readOperations.RemoveAll(t => t.IsCompleted);
//                await Task.WhenAll(readOperations);
//            }
//            else
//            {
//                read = new TaskCompletionSource<object>();
//                readOperations.Add(read.Task);
//                access.Release();
//            }
//
//            isWriting = writable;
//        }
//
//        public Task Finish()
//        {
//            if (isWriting == true)
//            {
//                Save();
//                access.Release();
//            }
//            else
//            {
//                read.SetResult(null);
//                read = null;
//            }
//
//            isWriting = null;
//            return Task.CompletedTask;
//        }
//
//        public void Release()
//        {
//            if (isWriting == true)
//            {
//                isInvalidated = true;
//                Tables = null;
//                access.Release();
//            }
//            else
//            {
//                read.SetResult(null);
//                read = null;
//            }
//
//            isWriting = null;
//        }
//
//        public void Dispose()
//        {
//            Debug.Assert(!isWriting.HasValue, "Disposing still-locked Context");
//
//            Debug.WriteLine($"Destroyed context {number}");
//        }
//    }
//}
