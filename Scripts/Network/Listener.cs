using Generic.Singleton;
using Network.Data;
using SocketIO;
using System.Collections.Generic;
using UnityEngine;

public abstract class Listener : MonoBehaviour, Network.Interface.IListener
{
    public abstract void RegisterCallback();

    private EventListenersController evCtrl;

    private HashSet<string> listenningEvents;
    private HashSet<string> abilityEmitEvents;

    protected Network.Sync.Sync SyncData;

    public EventListenersController EvCtrl
    {
        get
        {
            return evCtrl ?? (evCtrl = Singleton.Instance<EventListenersController>());
        }
    }

    protected HashSet<string> ListenningEvents
    {
        get
        {
            return listenningEvents ?? (listenningEvents = new HashSet<string>());
        }
    }
    protected HashSet<string> AbilityEmitEvents
    {
        get
        {
            return abilityEmitEvents ?? (abilityEmitEvents = new HashSet<string>());
        }
    }

    protected virtual void Start()
    {
        SyncData = EvCtrl.SyncData;
        RegisterCallback();
    }

    public void AddEmiter(string ev, System.Func<JSONObject> getData)
    {
        EvCtrl.AddEmiter(ev, getData);
        AbilityEmitEvents.Add(ev);
    }

    public void Emit(string ev)
    {
        EvCtrl.Emit(ev);
        //Debugger.Log("Emit: " + ev);
    }

    public void Emit(string ev, JSONObject data)
    {
        EvCtrl.Emit(ev, data);
    }

    public void On(string ev, params System.Action<SocketIOEvent>[] callbacks)
    {
        for (int i = 0; i < callbacks.Length; i++)
        {
            EvCtrl.On(ev, callbacks[i]);
        }
        ListenningEvents.Add(ev);
        // Debugger.Log("On " + ev);
    }
}
