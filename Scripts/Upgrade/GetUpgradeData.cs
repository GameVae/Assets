using SocketIO;
using System.Collections.Generic;
using UnityEngine;

public class GetUpgradeData : Listener
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void R_UPGRADE(SocketIOEvent obj)
    {
        //Debug.Log(obj);
    }

    public void R_BASE_UPGRADE(SocketIOEvent obj)
    {
        SyncData.BaseUpgrade.LoadTable(obj.data["R_BASE_UPGRADE"]);
    }

    public override void RegisterCallback()
    {
        Conn.On("R_BASE_UPGRADE", R_BASE_UPGRADE);
        Conn.On("R_UPGRADE", R_UPGRADE);
    }
}
