using SocketIO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

public class Connection : MonoBehaviour
{
    public struct ServerInfo
    {
        public int Port { get; set; }
        public string Host { get; set; }
        public string Name { get; set; }
    }

    private static Connection instance;
    private string url;
    private string defaultUrl;
    private bool isConnected;
    private bool isServerClosed;
    private int reconnectCountInCurrentServer;

    private int serverIndex;
    private int reconnectCount;
    private SocketIOComponent socket;
    private List<ServerInfo> serversInfo;
    private ServerInfo currentSever;

    private Debugger Loger;

    [Header("Socket IO")]
    public string ConnectingServer;
    public int ConnectingPort;
    public string ConnectingHost;

    public float TimeOut;
    [Tooltip("How many time reconnect on all server?")]
    public int MaxReconnect;
    public int MaxReconnectPerSever;
    public GameObject IOPrefab;

    [Header("UI")]
    public Button connectButton;

    #region Properties
    public bool IsConnected
    {
        get { return isConnected; }
        private set { isConnected = value; }
    }
    public bool IsServerClosed
    {
        get { return isServerClosed; }
        private set { isServerClosed = value; }
    }

    public ServerInfo CurrentServer
    {
        get { return currentSever; }
    }
    #endregion
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != null) { Destroy(instance); }

        InitServerInfo();
        IsServerClosed = false;
        IsConnected = false;
        // init socket
        socket = Instantiate(IOPrefab).GetComponent<SocketIOComponent>();
        defaultUrl = socket.url;
        CreateUrl(ref url);
        socket.url = url;
        socket.gameObject.SetActive(true);
        RegisterListener();

        // UI
        connectButton.onClick.AddListener(OnConnectButton);
    }
    private void Start()
    {
        Loger = Debugger.instance;
        InvokeRepeating("Reconnection", 1.0f, TimeOut * (MaxReconnectPerSever * (serversInfo.Count == 0 ? 1 : serversInfo.Count) + serversInfo.Count));
    }
    private void CreateUrl(ref string _url)
    {
        currentSever = serversInfo[serverIndex];
        ConnectingServer = currentSever.Name;
        ConnectingPort = currentSever.Port;
        ConnectingHost = currentSever.Host;

        _url = defaultUrl.Replace("4567", currentSever.Port.ToString());
        _url = _url.Replace("127.0.0.1", currentSever.Host);
    }
    private void UpToNextServer()
    {
        serverIndex = (serverIndex + 1) % serversInfo.Count;
    }
    private void RegisterListener()
    {
        if (socket != null)
        {
            socket.socket.OnClose += OnServerClose;
            socket.On("connection", OnConnection);
        }
    }
    private void Reconnection()
    {
        if (IsServerClosed && reconnectCount < MaxReconnect)
        {
            Loger.Log(reconnectCount);
            StartCoroutine(ReconnectToServer());
        }
    }
    private void InitServerInfo()
    {
        serversInfo = new List<ServerInfo>
        {
            new ServerInfo() { Name = "server1", Port = 9933, Host = "127.0.0.1" },
            new ServerInfo() { Name = "server2", Port = 1000, Host = "192.168.1.18" },
            new ServerInfo() { Name = "server3", Port = 1000, Host = "192.168.1.18" },

        };
    }

    private void CreateNewSocket()
    {
        CreateUrl(ref url);
        socket = Instantiate(IOPrefab).GetComponent<SocketIOComponent>();
        socket.url = url;
        socket.gameObject.SetActive(true);
        RegisterListener();
    }
    #region Request to server

    #endregion
    #region Listener
    private void OnServerClose(object sender, CloseEventArgs e)
    {
        if (IsConnected)
        {
            IsServerClosed = true;
            IsConnected = false;
            Loger.Log("Connection Losed");
        }
    }
    private void OnConnection(SocketIOEvent e)
    {
        IsConnected = true;
        IsServerClosed = false;
        reconnectCount = 0;
        Loger.Log("Connected");
    }
    #endregion
    #region Coroutine
    private IEnumerator ConnectToServer()
    {
        float elapsedTime = 0.0f;
        bool firstConnect = true;
        do
        {
            if (firstConnect)
            {
                Debug.Log(Loger);
                Loger.Log("Connecting ..." + socket.url);
                firstConnect = false;
                socket.Connect();
            }
            yield return new WaitForSecondsRealtime(0.5f);
            elapsedTime += 0.5f;
            if (elapsedTime > TimeOut)
            {
                Loger.Log("Time out");
                socket.Close();
                Destroy(socket.gameObject);
                socket = null;
                UpToNextServer();
                StartCoroutine(ForwardToServer());
                yield break;
            }
        }
        while (!isConnected);
        yield break;
    }
    private IEnumerator ForwardToServer()
    {
        float elapsedTime = 0.0f;
        bool firstConnect = true;
        do
        {
            if (firstConnect)
            {
                CreateNewSocket();
                socket.Connect();
                Loger.Log("Forwarding ..." + socket.url);
                firstConnect = false;
            }
            yield return new WaitForSecondsRealtime(0.5f);
            elapsedTime += 0.5f;
            if (elapsedTime > TimeOut)
            {
                socket.Close();
                Loger.Log("Timeout: forward to " + socket.url + " fail !");
                Destroy(socket.gameObject);
                socket = null;

                // reset and forward next server
                firstConnect = true;
                elapsedTime = 0.0f;

                if (serverIndex == 0)
                {
                    // fail all
                    Loger.Log("Fail All !");
                    yield break;
                }
                UpToNextServer();
            }
        }
        while (!isConnected);
        yield break;
    }
    private IEnumerator ReconnectToServer()
    {
        Loger.Log("Reconnect function called !");
        float elapsedTime = 0.0f;
        bool firstConnect = true;
        serverIndex = 0;
        reconnectCountInCurrentServer = 0;
        if (socket != null)
        {
            Destroy(socket.gameObject);
            socket = null;
        }
        do
        {
            if (firstConnect)
            {
                CreateNewSocket();
                socket.Connect();
                firstConnect = false;
                reconnectCountInCurrentServer++;
                Loger.Log("Reconnecting ..." + socket.url + " | Times: " + reconnectCountInCurrentServer);
            }
            yield return new WaitForSecondsRealtime(0.5f);
            elapsedTime += 0.5f;
            if (elapsedTime > TimeOut)
            {
                if (socket != null)
                {
                    // Loger.Log("Timeout: reconnect to " + socket.url + " fail !");
                    socket.Close();
                    Destroy(socket.gameObject);
                    socket = null;
                }

                // reset and forward next server
                firstConnect = true;
                elapsedTime = 0.0f;
                if (reconnectCountInCurrentServer == MaxReconnectPerSever)
                {
                    reconnectCountInCurrentServer = 0;
                    if (serverIndex == serversInfo.Count - 1)
                    {
                        Loger.Log("Fail All !");
                        reconnectCount++;
                        yield break;
                    }
                    UpToNextServer();
                }
            }
        }
        while (!isConnected);
        yield break;
    }
    #endregion
    #region UI
    public void OnConnectButton()
    {
        StartCoroutine(ConnectToServer());
    }
    #endregion
}
