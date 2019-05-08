using System.Collections;
using UnityEngine.Events;
using UnityGameTask;

public class ActionTask : IGameTask
{
    public bool IsDone
    {
        get;
        private set;
    }
    public float Progress
    {
        get;
        private set;
    }

    private UnityAction action;
    public ActionTask(UnityAction _action)
    {
        action = _action;
    }

    public IEnumerator Action()
    {
        action?.Invoke();

        IsDone = true;
        Progress = 1.0f;
        yield break;
    }
}
