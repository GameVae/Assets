﻿using SocketIO;
using UnityEngine;

public sealed class SIO_StartupListener : Listener
{
    public PlayerInfo Player;
    public SIO_MovementListener MovementListener;

    public void R_GET_RSS(SocketIOEvent obj)
    {
        //Debug.Log(obj);
        SyncData.RSS_Position.AsyncLoadTable(obj.data["R_GET_RSS"]);
    }

    public void R_BASE_INFO(SocketIOEvent obj)
    {
        //Debugger.Log(obj);
        SyncData.BaseInfos.LoadTable(obj.data["R_BASE_INFO"]);

        if (Player != null)
            Player.BaseInfo = SyncData.BaseInfos.ReadOnlyRows[0];
    }

    public void R_USER_INFO(SocketIOEvent obj)
    {

        SyncData.LoadUserInfo(obj.data["R_USER_INFO"]);
        if (Player != null)
            Player.Info = SyncData.MainUser;

        //string data = obj.data["R_USER_INFO"].ToString();
        //Debugger.Log(obj);
    }

    public void R_GET_POSITION(SocketIOEvent obj)
    {
        //Debugger.Log(obj);
        SyncData.Position.AsyncLoadTable(obj.data["R_GET_POSITION"]);
    }

    public void R_TRAINNING(SocketIOEvent obj)
    {
        //Debug.Log(obj);
    }

    public void R_BASE_DEFEND(SocketIOEvent obj)
    {
        //Debug.Log(obj);
        SyncData.BaseDefends[0].LoadTable(obj.data["R_BASE_DEFEND"]);
    }

    public void R_UPGRADE(SocketIOEvent obj)
    {
        //Debug.Log(obj);
    }

    public void R_BASE_UPGRADE(SocketIOEvent obj)
    {
        SyncData.CurrentBaseUpgrade.LoadTable(obj.data["R_BASE_UPGRADE"]);
    }

    private void R_UNIT(SocketIOEvent obj)
    {
        //Debugger.Log(obj);
        SyncData.UnitTable.AsyncLoadTable(obj.data["R_UNIT"]);
    }

    private void R_PLAYER_INFO(SocketIOEvent obj)
    {
        // Debugger.Log(obj);
        // TODO: upgrade next time
        SyncData.UserInfos.UpdateTable(obj.data["R_PLAYER_INFO"]);

    }

    private void R_BASE_PLAYER(SocketIOEvent obj)
    {
        //Debugger.Log(obj);
        SyncData.BasePlayerTable.LoadTable(obj.data["R_BASE_PLAYER"]);
    }

    private void R_FRIEND_INFO(SocketIOEvent obj)
    {

        //Debugger.Log(obj.data["R_FRIEND_INFO"]);
        SyncData.FriendTable.AsyncLoadTable(obj.data["R_FRIEND_INFO"]);
    }

    private void R_USER_GUILD(SocketIOEvent obj)
    {
        Debugger.Log(obj);
        SyncData.GuildTable.AsyncLoadTable(obj.data["R_USER_GUILD"]);
    }

    public override void RegisterCallback()
    {
        On("R_GET_RSS", R_GET_RSS);
        On("R_BASE_INFO", R_BASE_INFO);
        On("R_USER_INFO", R_USER_INFO);
        On("R_GET_POSITION", R_GET_POSITION);
        On("R_USER_GUILD", R_USER_GUILD);

        On("R_TRAINING", R_TRAINNING);
        On("R_BASE_DEFEND", R_BASE_DEFEND);
        On("R_BASE_UPGRADE", R_BASE_UPGRADE);
        On("R_UPGRADE", R_UPGRADE);
        On("R_UNIT", R_UNIT);
        On("R_PLAYER_INFO", R_PLAYER_INFO);
        On("R_BASE_PLAYER", R_BASE_PLAYER);
        On("R_FRIEND_INFO", R_FRIEND_INFO);
        On("R_MOVE", MovementListener.R_MOVE);
        On("R_TESTMOVE", MovementListener.R_TESTMOVE);

    }
}
