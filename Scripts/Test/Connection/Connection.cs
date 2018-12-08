using SocketIO;
using System.Threading;
using UnityEngine;
using WebSocketSharp;

public class Connection : MonoBehaviour
{
    public static Connection instance;

    public SocketIOComponent SocketComponent;

    public SocketIOComponent Socket
    {
        get { return SocketComponent; }
    }

    public bool IsServerConnected
    {
        get
        {
            return IsClose && SocketComponent.socket.IsConnected;
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
        if (instance == null) instance = this;
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
    }

    private void Update()
    {
        if (!IsClose)
        {
            if (pingElapsed >= PingTimeOut)
            {
                SocketComponent.Close();
                Debug.Log("Closed");
            }
        }
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

    private void OnApplicationQuit()
    {
        timer.Abort();
    }

    private void OnDestroy()
    {
        SocketComponent.Close();
        timer.Abort();
    }
}
