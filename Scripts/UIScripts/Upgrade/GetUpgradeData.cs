using SocketIO;
using UnityEngine;

public sealed class GetUpgradeData : Listener
{
    public void R_UPGRADE(SocketIOEvent obj)
    {
        Debug.Log(obj);
    }

    public void R_BASE_UPGRADE(SocketIOEvent obj)
    {
        SyncData.CurrentBaseUpgrade.LoadTable(obj.data["R_BASE_UPGRADE"]);
    }

    public override void RegisterCallback()
    {
        Conn.On("R_BASE_UPGRADE", R_BASE_UPGRADE);
        Conn.On("R_UPGRADE", R_UPGRADE);
    }
}
