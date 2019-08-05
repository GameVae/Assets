using DataTable;
using DataTable.Row;
using Generic.Observer;
using Generic.Singleton;
using Network.Data;
using SocketIO;
using System.Collections.Generic;
using UnityEngine;

public sealed class FriendSys : MonoSingle<FriendSys>, ISubject
{
    [SerializeField] private JSONTable_Friends friends;

    private EventListenersController eventCtrl;
    private EventListenersController EventControl
    {
        get
        {
            return eventCtrl ?? (eventCtrl = Singleton.Instance<EventListenersController>());
        }
    }

    private List<IObserver> observers;
    private List<IObserver> Observers
    {
        get
        {
            return observers ?? (observers = new List<IObserver>());
        }
    }

    private void Start()
    {
        EventControl.On("R_ACCEPT_FRIEND", R_ACCEPT_FRIEND);
    }

    public bool IsMyFriend(int id)
    {
        FriendRow f = friends.GetFriendInfoById(id);
        return f != null;
    }

    public FriendRow GetFriend(int id)
    {
        return friends.GetFriendInfoById(id);
    }

    private void R_ACCEPT_FRIEND(SocketIOEvent obj)
    {
        Debugger.Log(obj.data["R_ACCEPT_FRIEND"]);
        int idAccept = -1;
        obj.data.GetField(ref idAccept, "R_ACCEPT_FRIEND");

        if (idAccept != -1)
        {
            FriendRow info = new FriendRow()
            {
                ID_Player = idAccept,
                AcceptTime = 0,
                RemoveTime = 0,
                RequestBool = false,
                
            };
            friends.UpdateFriendInfo(info);
            NotifyAll();
        }
    }

    public void Register(IObserver observer)
    {
        Observers.Add(observer);
    }

    public void Remove(IObserver observer)
    {
        int index = Observers.IndexOf(observer);
        if (index >= 0 && index < Observers.Count)
        {
            Observers.RemoveAt(index);
        }
    }

    public void NotifyAll()
    {
        for (int i = 0; i < Observers.Count; i++)
        {
            Observers[i].SubjectUpdated(null);
        }
    }
}
