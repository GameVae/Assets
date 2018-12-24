using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation : MonoBehaviour {
    public static PlayerInformation instance;
    public int ID_User;
    public int ID_Server;
    [Space]
    public Connection Connection;

    public PlayerInformation(int iD_User, int iD_Server)
    {
        ID_User = iD_User;
        ID_Server = iD_Server;
    }

    void Awake () {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
    }
    private void Start()
    {
        Connection.Socket.On("S_USER_INFO", S_USER_INFO);
    }

    private void S_USER_INFO(SocketIOEvent obj)
    {
        int ID_User = int.Parse((obj.data.GetField("ID_User").ToString()));
        int ID_Server = int.Parse((obj.data.GetField("ID_Server").ToString()));
        PlayerInformation playerInformation = new PlayerInformation(ID_User, ID_Server);
    }
}

