using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Base_User_Info
{

}
[Serializable]
public class Rss_User_Info
{
    public Vector3Int Position_Rss_Cell;
    public int Quality;
}
[Serializable]
public class User_Info
{
    public int ID_User;
    public int ID_Server;
    public int Diamond;  
    public int? Guild_ID;
    public int? Last_Guild_ID;
    public int Might;
    public int Killed;

    public string NameInGame;
    public string ChatWorldColor;
    public string GuildName;

    public User_Info(int iD_User, int iD_Server, int diamond, int? guild_ID, int? last_Guild_ID, int might, int killed, string nameInGame, string chatWorldColor, string guildName)
    {
        ID_User = iD_User;
        ID_Server = iD_Server;
        Diamond = diamond;
        Guild_ID = guild_ID;
        Last_Guild_ID = last_Guild_ID;
        Might = might;
        Killed = killed;
        NameInGame = nameInGame;
        ChatWorldColor = chatWorldColor;
        GuildName = guildName;
    }
}
public class PlayerInformation : MonoBehaviour {
    public static PlayerInformation instance;
    [Space]
    public User_Info User_Info;
    [Space]
    public Base_User_Info[] Base_User_Info = new Base_User_Info[5];
    [Space]
    public Rss_User_Info[] rss = new Rss_User_Info[10];
    [Space]
    private Connection Connection;

    

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
        Debug.Log("gán vào Scriptable Object");
    }
}

