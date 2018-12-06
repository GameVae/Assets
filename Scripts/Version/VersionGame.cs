using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VersionGame : MonoBehaviour
{

    public SocketIOComponent Socket;

    void Start()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        //data["Version"] = ;
        

        Socket.Emit("S_CHECK_VERSION", new JSONObject(data));
        Socket.On("R_CHECK_VERSION", R_CHECK_VERSION);
    }

    private void R_CHECK_VERSION(SocketIOEvent obj)
    {
        Debug.Log("R_CHECK_VERSION: "+obj);
    }
}
