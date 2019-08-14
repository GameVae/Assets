
namespace Generic.Observer
{
    public interface ISubject
    {
        void Register(IObserver observer);
        void Remove(IObserver observer);
        void NotifyAll();
    }

    public interface ISubject<T>
        where T : IObserver
    {
        void Register(T observer);
        void Remove(T observer);
        void Notify(T observer);
        void NotifyAll();
    }
}