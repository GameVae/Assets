using Generic.Singleton;
using Network.Sync;
using SocketIO;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;
using WebSocketSharp;

public sealed class Connection : MonoSingle<Connection>
{
    public Sync Sync;

    public SocketIOComponent SocketComponent;

    public SocketIOComponent Socket
    {
        get { return SocketComponent; }
    }

    /// <summary>
    /// have to bigger than Ping Interval and smaller than Ping Time Out
    /// </summary>
    [Header("Have to bigger than Ping Interval and smaller than Ping Time Out")]
    public float NetworkTimeOut;

    public bool IsServerConnected
    {
        get
        {
            return !IsClose && SocketComponent.socket.IsConnected;
        }
    }

    public bool IsClose
    {
        get
        {
            try { return !SocketComponent.IsConnected; }
            catch { return true; }
        }
    }

    public bool IsLosedNetwork
    {
        get
        {
            return pingElapsed > NetworkTimeOut;
        }
    }

    public float PingTimeOut { get { return SocketComponent.pingTimeout; } }

    private volatile float pingElapsed;
    private Decoder decoder;
    private Thread timer;

    protected override void Awake()
    {
        base.Awake();
        decoder = new Decoder();
        if (SocketComponent == null)
            SocketComponent = FindObjectOfType<SocketIOComponent>();
    }

    private void Start()
    {
        SocketComponent.socket.OnMessage += OnMessage;

        timer = new Thread(() =>
        {
            pingElapsed = SocketComponent.pingInterval;
            while (true)
            {
                if (!IsClose)
                {
                    Thread.Sleep(20);
                    pingElapsed += 0.02f;
                }
                else
                {
                    Thread.Sleep(200);
                }
            }
        });
        timer.Start();
    }

    private void Update()
    {
        if (!IsClose)
        {
            if (pingElapsed >= PingTimeOut)
            {
                SocketComponent.Close();
#if UNITY_EDITOR
                Debugger.Log("Connection Closed");
#endif
            }
            Sync?.SyncUpdate(Time.deltaTime);
        }
    }

    private void OnApplicationQuit()
    {
        timer?.Abort();
        SocketComponent?.Close();
    }

    private void OnMessage(object sender, MessageEventArgs e)
    {
        Packet packet = decoder.Decode(e);
        switch (packet.enginePacketType)
        {
            case EnginePacketType.PONG:
                pingElapsed = 0;
                break;
        }
    }

    public void On(string ev, System.Action<SocketIOEvent> callback)
    {
        SocketComponent.On(ev, callback);
    }

    public void Emit(string v, JSONObject data)
    {
        SocketComponent.Emit(v, data);
    }
}
