using System.Diagnostics;
using System.Threading.Tasks;
using SmallWorld.Library.Model.Abstractions;

namespace SmallWorld.Library.Model.Impl
{
    public class ContextLock : IContextLock
    {
        private readonly IContext context;

        private bool? isWriting;

        public ContextLock(IContext context)
        {
            this.context = context;
        }

        public void CheckWritable()
        {
            Debug.Assert(isWriting == true, "ContextLock should be writable");
        }

        public async Task<IContextWritableHandle> Write()
        {
            isWriting = true;
            await context.AcquireLock(true);
            return new Handle(this);
        }

        public async Task<IContextReadableHandle> Read()
        {
            isWriting = false;
            await context.AcquireLock(false);
            return new Handle(this);
        }

        private class Handle : IContextWritableHandle
        {
            private readonly ContextLock access;

            public Handle(ContextLock access)
            {
                this.access = access;
            }

            public void Dispose()
            {
                if (!access.isWriting.HasValue) return;

                access.isWriting = null;
                access.context.Release();
            }

            public async Task Finish()
            {
                await access.context.Finish();
                access.isWriting = null;
            }
        }
    }
}
