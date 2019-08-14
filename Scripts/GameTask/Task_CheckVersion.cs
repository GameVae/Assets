using Generic.Singleton;
using DataTable;
using DataTable.Loader;
using SocketIO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using UnityGameTask;

public class Task_CheckVersion : IGameTask
{
    public bool IsDone
    {
        get { return isDone; }
        private set { isDone = value;}
    }
    public float Progress
    {
        get { return progress; }
        private set { progress = value; }
    }
    public Connection Connection
    {
        get
        {
            return connection ?? (connection = Singleton.Instance<Connection>());
        }
    }

    private bool isDone;
    private float progress;
    private Connection connection;
    private ManualTableLoader sqlTableLoader;

    public Task_CheckVersion(ManualTableLoader tableLoader)
    {
        Connection.On("R_CHECK_VERSION", R_CHECK_VERSION);
        sqlTableLoader = tableLoader;

        IsDone = false;
        Progress = 0.0f;
    }

    private void S_CHECK_VERSION()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        sqlTableLoader.GetCurrentVersion();
        data["Version"] = sqlTableLoader.ClientVersion;

        JSONObject verObj = new JSONObject(data);
        Connection.Emit("S_CHECK_VERSION", verObj);
        //Debugger.Log(verObj);

    }
    private void R_CHECK_VERSION(SocketIOEvent obj)
    {
        sqlTableLoader.ServerVersion = obj.data.GetField("Version").ToString().Trim('"');
        bool isUpdate = sqlTableLoader.CheckVersion();
        
        Debugger.Log(obj.data);
        Debugger.Log("isupdate " + isUpdate);

        if (isUpdate)
        {
            // @"file://DESKTOP-FHHKHH7/FileDownload/DB.sqlite"
            string link = obj.data["Data"].ToString().Trim('"');
            
            string saveAt = UnityPath.Combinate(@"DB\Infantry.sqlite", UnityPath.AssetPath.Persistent);
            try
            {
                if (UnityPath.Exist(saveAt))
                {
                    File.Delete(saveAt);
                    Debugger.Log("DELETED FILE: " + saveAt);
                }
                else
                {
                    UnityPath.CreateFileAnywhere(saveAt);
                }
                DownloadFile(link, saveAt);
            }
            catch (Exception e)
            {
                Debugger.ErrorLog(e.ToString());
            }
        }
        else
        {
            ReloadDB();
            CheckVerisonComplete();
        }
    }

    private void DownloadFile(string link, string saveAt)
    {
        // @"file://DESKTOP-FHHKHH7/FileDownload/Infantry.sqlite"),Application.dataPath + @"\Infantry.sqlite"
        WebClient client = DownloadFileAsync.Instance.DownloadFile(link, saveAt);
        if (client != null)
        {
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadComplete);
        }
    }

    private void DownloadComplete(object sender, AsyncCompletedEventArgs e)
    {
        ReloadDB();
        CheckVerisonComplete();
        Debugger.Log("Download file complete");
    }

    private void ReloadDB()
    {
        sqlTableLoader.ReloadAll();
    }   

    private void CheckVerisonComplete()
    {
        IsDone = true;
        Progress = 1.0f;
    }

    public IEnumerator Action()
    {
        while (!Connection.IsServerConnected)
        {
            yield return null;
        }

        //Debugger.Log("Server connected: " + Connection.IsServerConnected);
        S_CHECK_VERSION();
        yield break;
    }
}
