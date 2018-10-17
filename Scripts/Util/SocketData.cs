using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;
using System;

public class SocketData : MonoBehaviour {
    public static SocketData instance;
    public Dictionary<string, List<Action<SocketIOEvent>>> Handlers;

    void Awake () {
        Handlers = new Dictionary<string, List<Action<SocketIOEvent>>>();
        instance = this;
        
    }
}
