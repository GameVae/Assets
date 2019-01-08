using Network.Sync;
using SocketIO;
using System.Collections;
using System.Threading;
using UnityEngine;
using WebSocketSharp;

public sealed class Connection : MonoBehaviour
{
    public static Connection Instance;

    public Sync Sync;

    public SocketIOComponent SocketComponent;

    public SocketIOComponent Socket
    {
        get { return SocketComponent; }
    }

    public bool IsServerConnected
    {
        get
        {
            return !IsClose && SocketComponent.socket.IsConnected;
        }
    }

    public bool IsClose
    {
        get { return !SocketComponent.IsConnected; }
    }

    public float PingTimeOut { get { return SocketComponent.pingTimeout; } }

    private volatile float pingElapsed;
    private Decoder decoder;
    private Thread timer;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        decoder = new Decoder();
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
        StartCoroutine("CheckVersion");

        GameProgress.Instance.AddTask("check version");
        GameProgress.Instance.StartTask();
    }

    private void Update()
    {
        if (!IsClose)
        {
            if (pingElapsed >= PingTimeOut)
            {
                SocketComponent.Close();
#if UNITY_EDITOR
                Debug.Log("Connection Closed");
#endif
            }
        }
        // Sync?.Update();
    }

    private void OnApplicationQuit()
    {
        timer.Abort();
        SocketComponent.Close();
    }

    private IEnumerator CheckVersion()
    {
        while (!IsServerConnected)
        {
            yield return null;
        }

        Debug.Log("Server: " + IsServerConnected);
        VersionGame.Instance.S_CHECK_VERSION();
        yield break;
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
}
