using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using SocketIO;
using Generic.Singleton;

public class BlockUser : MonoBehaviour
{
    [Space]
    private SocketIOComponent socketIO;

    void Start()
    {
        socketIO = Singleton.Instance<Connection>().Socket;
        socketIO.On("R_BLOCKED", R_BLOCKED);
    }

    private void R_BLOCKED(SocketIOEvent obj)
    {
        Debug.Log("R_BLOCKED: " + obj);
        int blockedForever = int.Parse(obj.data["BlockedForever"].ToString());
        switch (blockedForever)
        {
            case 0:
                DateTime now = TimeControl.instance.LocalTimeNow;
                double blockTime = TimeControl.instance.CalculateTime(now, (TimeControl.instance.CalcToLocalTime(obj.data["Time"].ToString())));
                Debug.Log("Block Time: " + blockTime);
                if (blockTime > 0)
                {
                    Debug.Log("Block Time: " + blockTime + "_" + TimeControl.instance.CalcToLocalTime((obj.data["Time"].ToString())));
                }
                break;
            case 1:
                Debug.Log("Block Forever");
                break;
        }
    }

}
