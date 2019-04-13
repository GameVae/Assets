
namespace Generic.Observer
{
    public interface ISubject
    {
        void Register(IObserver observer);
        void Remove(IObserver observer);
        void Notify(IObserver observer);
        void NotifyAll();
    }
}