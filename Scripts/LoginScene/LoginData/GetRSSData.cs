using ManualTable;
using Network.Sync;
using SocketIO;
using System.Collections.Generic;
using UnityEngine;

public class GetRSSData : MonoBehaviour {
    public static GetRSSData instance;
    public RSS_PositionJSONTable RSS_Table;
    public PositionJSONTable Position_Table;
    public Connection Conn;

    private GameProgress getDataProgress;

    private void Awake () {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
        Conn = Connection.Instance;
        getDataProgress = GameProgress.Instance;
    }

    public void R_GET_RSS(SocketIOEvent obj)
    {
        RSS_Table.LoadTable(obj.data["R_GET_RSS"]);
    }

    public void S_GET_RSS(SocketIOComponent socket,int ID_User)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["Server"] = ID_User.ToString();
        socket.Emit("S_GET_RSS", new JSONObject(data));
    }
 
    public void R_BASE_INFO(SocketIOEvent obj)
    {
        string data = obj.data["R_BASE_INFO"].ToString();
        Conn.Sync.BaseInfo = JsonUtility.FromJson<BaseInfo>(data);
        getDataProgress.Done("get base info");

    }

    public void S_BASE_INFO(SocketIOComponent socket, int ID_User)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["Server"] = ID_User.ToString();
        socket.Emit("S_GET_RSS", new JSONObject(data));
    }

    public void R_USER_INFO(SocketIOEvent obj)
    {
        string data = obj.data["R_USER_INFO"].ToString();
        Conn.Sync.UserInfo = JsonUtility.FromJson<UserInfo>(data);
        getDataProgress.Done("get user info");
    }

    public void S_USER_INFO(SocketIOComponent socket, int ID_User)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["Server"] = ID_User.ToString();
        socket.Emit("S_GET_RSS", new JSONObject(data));
    }

    public void R_GET_POSITION(SocketIOEvent obj)
    {
        Position_Table.LoadTable(obj.data["R_GET_POSITION"]);
        getDataProgress.Done("get position");
    }

    public void S_GET_POSITION(SocketIOComponent socket, int ID_User)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["Server"] = ID_User.ToString();
        socket.Emit("S_GET_RSS", new JSONObject(data));
    }
    
}
