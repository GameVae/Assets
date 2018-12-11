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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Connection.Socket.On("R_CHECK_VERSION", R_CHECK_VERSION);
    }
  
    private void R_CHECK_VERSION(SocketIOEvent obj)
    {
        //Debug.Log("Recieve data version");
        //Debug.Log("R_CHECK_VERSION: " + obj.data["Version"]);
        //Debug.Log("R_CHECK_VERSION: " + obj.data["Data"]);

        Loader.ServerVersion = obj.data.GetField("Version")?.ToString().JsonToString();
        if (Loader.CheckVersion())
        {
            // @"file://DESKTOP-FHHKHH7/FileDownload/Infantry.sqlite"
            string link = obj.data["Data"]?.ToString().JsonToString();
            string saveAt = Application.dataPath + @"\Data\DB\Infantry.sqlite";
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
        System.Net.WebClient client = new WebClient(); 
        client.DownloadFileAsync(new Uri(link),saveAt);
        client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadComplete);
    }

    private void DownloadComplete(object sender, AsyncCompletedEventArgs e)
    {
        Debug.Log("Download Complete " + sender.ToString());
        WebClient client = (WebClient)sender;
        client.CancelAsync();
        client.Dispose();

        Loader.InitSQLConnection();
        Loader.ReloadData();        
    }

    public void S_CHECK_VERSION()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["Version"] = "1";
        Connection.Socket.Emit("S_CHECK_VERSION", new JSONObject(data));
    }
}
