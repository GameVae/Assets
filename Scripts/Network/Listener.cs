using Generic.Singleton;
using Network.Data;
using SocketIO;
using System.Collections.Generic;
using UnityEngine;

public abstract class Listener : MonoBehaviour, Network.Interface.IListener
{
    public abstract void RegisterCallback();

    private EventListenersController evCtrl;

    protected HashSet<string> listenningEvents;
    protected HashSet<string> abilityEmitEvents;

    protected Network.Sync.Sync SyncData;

    protected virtual void Start()
    {
        evCtrl = Singleton.Instance<EventListenersController>();
        SyncData = evCtrl.SyncData;

        listenningEvents = new HashSet<string>();
        abilityEmitEvents = new HashSet<string>();

        RegisterCallback();
    }

    public void AddEmiter(string ev, System.Func<JSONObject> getData)
    {
        evCtrl.AddEmiter(ev, getData);
        abilityEmitEvents.Add(ev);
    }

    public void Emit(string ev)
    {
        evCtrl.Emit(ev);
        Debugger.Log("Emit: " + ev);
    }

    public void On(string ev,params System.Action<SocketIOEvent>[] callbacks)
    {
        for (int i = 0; i < callbacks.Length; i++)
        {
            evCtrl.On(ev, callbacks[i]);
        }
        listenningEvents.Add(ev);
    }
}
