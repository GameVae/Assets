using System.Collections;
using UnityGameTask;

public class SyncServerData : IGameTask
{
    private bool autoEmit;
    private string eventString;
    private Listener listener;

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

    public SyncServerData(Listener _listener, string evt, bool autoEmitEvent = false)
    {
        listener = _listener;
        eventString = evt;
        autoEmit = autoEmitEvent;

        _listener.On(evt, delegate { Received(); });
    }

    public IEnumerator Action()
    {
        Progress = 0.0f;
        IsDone = false;

        if (autoEmit)
            Emit();    
        yield break;
    }

    private void Emit()
    {
        listener.Emit(eventString);
    }

    private void Received()
    {
        Progress = 1.0f;
        IsDone = true;
    }
}
