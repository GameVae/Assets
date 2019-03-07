using Generic.Singleton;
using Network.Data;
using SocketIO;
using UnityEngine;

public abstract class Listener : MonoBehaviour, Network.Interface.IListener
{
    public abstract void RegisterCallback();

    protected Network.Sync.Sync SyncData;
    protected EventListenersController evCtrl;

    protected Connection Conn { get { return evCtrl.Conn; } }
    protected virtual void Start()
    {
        evCtrl = Singleton.Instance<EventListenersController>();
        SyncData = evCtrl.SyncData;
        RegisterCallback();
    }

    public void AddEmiter(string ev, System.Func<JSONObject> getData)
    {
        evCtrl.AddEmiter(ev, getData);
    }

    public void Emit(string ev)
    {
        evCtrl.Emit(ev);
    }

    public void On(string ev,params System.Action<SocketIOEvent>[] callbacks)
    {
        for (int i = 0; i < callbacks.Length; i++)
        {
            Conn.On(ev, callbacks[i]);
        }
    }
}
