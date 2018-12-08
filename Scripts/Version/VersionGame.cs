using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionGame : MonoBehaviour
{
    public static VersionGame instance;
    public Connection Connection;
    void Start()
    { 
        Connection.Socket.On("R_CHECK_VERSION", R_CHECK_VERSION);
    }
    public void S_CHECK_VERSION()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["Version"] = "1";
        Connection.Socket.Emit("S_CHECK_VERSION", new JSONObject(data));
    }
    private void R_CHECK_VERSION(SocketIOEvent obj)
    {
        Debug.Log("R_CHECK_VERSION: "+obj);
    }

}
