﻿using ManualTable.Row;
using SocketIO;
using UnityEngine;

public sealed class GetTranningData : Listener
{
    public override void RegisterCallback()
    {
        Conn.On("R_TRAINING", R_TRAINNING);
        Conn.On("R_BASE_DEFEND", R_BASE_DEFEND);

    }

    public void R_TRAINNING(SocketIOEvent obj)
    {
        Debug.Log(obj);
    }

    public void R_BASE_DEFEND(SocketIOEvent obj)
    {
        Debug.Log("R_BASE_DEFEND");
        //Debug.Log(obj);

        int baseNum = 0;
        obj.data["R_BASE_DEFEND"].GetField(ref baseNum,"BaseNumber");
        SyncData.BaseDefend[baseNum].LoadTable(obj.data["R_BASE_DEFEND"]);
    }
}