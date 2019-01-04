using ManualTable;
using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRSSData : MonoBehaviour {
    public static GetRSSData instance;
    public RSS_PositionJSONTable RSS_Table;

    private void Awake () {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }

    public void R_GET_RSS(SocketIOEvent obj)
    {
        RSS_Table.LoadTable(obj.data["Data"]);
    }
    public void S_GET_RSS(SocketIOComponent socket,int ID_User)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["Server"] = ID_User.ToString();
        socket.Emit("S_GET_RSS", new JSONObject(data));
    }
 

}
