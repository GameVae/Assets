using System.Collections;

namespace UnityGameTask
{
    public interface IGameTask
    {
        bool IsDone { get; }
        float Progress { get; }
        IEnumerator Action();
    }

    public interface IGameTaskCollection : IGameTask
    {
        void AddGameTask(IGameTask task);
    }
}