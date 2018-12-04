using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestReveiceFile : MonoBehaviour
{
    public Connection Conn;
    Button getFileBtn;
    void Awake()
    {
        getFileBtn = gameObject.GetComponent<Button>();
        getFileBtn.onClick.AddListener(() => S_CHECK_VERSION());

    }
    private void Start()
    {

        Conn.Socket.On("R_CHECK_VERSION", R_CHECK_VERSION);
    }
    private void S_CHECK_VERSION()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["Version"] = "1";

        Conn.Socket.Emit("S_CHECK_VERSION", new JSONObject(data));

    }

    private void R_CHECK_VERSION(SocketIOEvent obj)
    {
        Debug.Log(obj);
        Debug.Log(obj.data);
    }
}
