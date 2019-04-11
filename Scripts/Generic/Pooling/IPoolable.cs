using System;

namespace Generic.Pooling
{
    public interface IPoolable : IDisposable
    {
        int ManagedId { get; }
        void FirstSetup(int insId);
    }
}