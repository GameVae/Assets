using System;

namespace Generic.Observer
{
    public interface IObserver : IDisposable
    {
        void SubjectUpdated(object dataPacked);
    }
}