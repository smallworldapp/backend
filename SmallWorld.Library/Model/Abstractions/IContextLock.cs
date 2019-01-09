using System;
using System.Threading.Tasks;

namespace SmallWorld.Library.Model.Abstractions
{
    public interface IContextLock
    {
        void CheckWritable();

        Task<IContextWritableHandle> Write();

        Task<IContextReadableHandle> Read();
    }

    public interface IContextReadableHandle : IDisposable
    {
//        void Release();
    }

    public interface IContextWritableHandle : IContextReadableHandle
    {
        Task Finish();
    }
}