using SocketIO;
using UnityEngine;

public sealed class GetRSSData : Listener
{
    private GameProgress getDataProgress;

    protected override void Start()
    {
        base.Start();
        getDataProgress = GameProgress.Instance;
    }

    public void R_GET_RSS(SocketIOEvent obj)
    {
        SyncData.RSS_Position.LoadTable(obj.data["R_GET_RSS"]);
    }

    public void R_BASE_INFO(SocketIOEvent obj)
    {
        Debug.Log(obj);
        Conn.Sync.BaseInfo.LoadTable(obj.data["R_BASE_INFO"]);
        getDataProgress.Done("get base info");
    }

    public void R_USER_INFO(SocketIOEvent obj)
    {
        string data = obj.data["R_USER_INFO"].ToString();
        SyncData.UserInfo.LoadTable(obj.data["R_USER_INFO"]);
        Debug.Log(obj);
        getDataProgress.Done("get user info");
    }

    public void R_GET_POSITION(SocketIOEvent obj)
    {
        //Debug.Log(obj.data["R_GET_POSITION"]);
        SyncData.Position.LoadTable(obj.data["R_GET_POSITION"]);
        getDataProgress.Done("get position");
    }

    public override void RegisterCallback()
    {
        Conn.On("R_GET_RSS", R_GET_RSS);
        Conn.On("R_BASE_INFO", R_BASE_INFO);
        Conn.On("R_USER_INFO", R_USER_INFO);
        Conn.On("R_GET_POSITION", R_GET_POSITION);
    }
}
