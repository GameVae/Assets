using ManualTable.Loader;
using SocketIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using UnityEngine;
using Utils;

public class VersionGame : MonoBehaviour
{
    public static VersionGame Instance;
    public Connection Connection;
    public ManualTableLoader Loader;
    //public RSS_PositionJSONTable RSS_Table;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        Connection.Socket.On("R_CHECK_VERSION", R_CHECK_VERSION);
        //Connection.Socket.On("R_GET_RSS", R_GET_RSS);
    }
    
    //private void R_GET_RSS(SocketIOEvent obj)
    //{
    //    RSS_Table.LoadTable(obj.data["Data"]);
    //}

    public void S_CHECK_VERSION()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["Version"] = "1";
        Connection.Socket.Emit("S_CHECK_VERSION", new JSONObject(data));
    }

    private void R_CHECK_VERSION(SocketIOEvent obj)
    {
        Loader.ServerVersion = obj.data.GetField("Version")?.ToString().JsonToString();
        if (Loader.CheckVersion())
        {

            // @"file://DESKTOP-FHHKHH7/FileDownload/Infantry.sqlite"
            string link = obj.data["Data"]?.ToString().JsonToString();
            string saveAt = Application.dataPath + @"\Data\Infantry.sqlite";
            try
            {
                if (File.Exists(saveAt))
                {
                    File.Delete(saveAt);
                    Debug.Log("DELETED FILE: " + saveAt);
                }
                DownloadFile(link, saveAt);
            }
            catch(Exception e) { Debug.Log(e.ToString()); }
        }
    }

    private void DownloadFile(string link,string saveAt)
    {
        // @"file://DESKTOP-FHHKHH7/FileDownload/Infantry.sqlite"),Application.dataPath + @"\Infantry.sqlite"
        WebClient client =  DownloadFileAsync.Instance.DownloadFile(link, saveAt);
        if(client != null)
        {
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressChange);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadComplete);
        }
    }

    private void DownloadProgressChange(object sender, DownloadProgressChangedEventArgs e)
    {

    }

    private void DownloadComplete(object sender, AsyncCompletedEventArgs e)
    {        
        Debug.Log("Download Complete " + sender.ToString());
        Loader.InitSQLConnection();
        Loader.ReloadData();
    }
}
