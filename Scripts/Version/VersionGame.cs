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
using UnityEngine;
using Utils;

public class VersionGame : MonoSingle<VersionGame>
{
    public Connection Connection;
    public ManualTableLoader SqlTableLoader;
    public SqliteFileValidator SqlValidator;

    private bool checkVersionDone;
    private LoadingPanel gameTask;

    private void Start()
    {
        Connection.Socket.On("R_CHECK_VERSION", R_CHECK_VERSION);

        checkVersionDone = false;
        //InitProg();
        StartCoroutine(SqlCheck());
    }

    public void S_CHECK_VERSION()
    {
        Dictionary<string, string> data = new Dictionary<string, string>();

        SqlTableLoader.GetCurrentVersion();
        data["Version"] = SqlTableLoader.ClientVersion;
        Debugger.Log(SqlTableLoader.ClientVersion);

        Connection.Emit("S_CHECK_VERSION", new JSONObject(data));
    }

    private void R_CHECK_VERSION(SocketIOEvent obj)
    {
        SqlTableLoader.ServerVersion = obj.data.GetField("Version").ToString().Trim('"');
        Debugger.Log(obj.data);
        bool isUpdate = SqlTableLoader.CheckVersion();

        Debugger.Log("isupdate " + isUpdate);
        if (isUpdate)
        {
            // @"file://DESKTOP-FHHKHH7/FileDownload/DB.sqlite"
            string link = obj.data["Data"].ToString().Trim('"');

            Debugger.Log(obj.data["Data"]);
            //string saveAt = Application.dataPath + @"\Data\DB\Infantry.sqlite";
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
            checkVersionDone = true;
        }
    }

    private void DownloadFile(string link, string saveAt)
    {
        // @"file://DESKTOP-FHHKHH7/FileDownload/Infantry.sqlite"),Application.dataPath + @"\Infantry.sqlite"
        WebClient client = DownloadFileAsync.Instance.DownloadFile(link, saveAt);
        if (client != null)
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
        ReloadDB();
        checkVersionDone = true;
        Debugger.Log("Download file complete");
    }

    private void ReloadDB()
    {
        SqlTableLoader.ReloadAll();
    }

    private IEnumerator CheckVersion()
    {
        while (!Connection.IsServerConnected)
        {
            yield return null;
        }

        Debugger.Log("Server connected: " + Connection.IsServerConnected);
        S_CHECK_VERSION();
        yield break;
    }

    private void InitProg()
    {
        gameTask = Singleton.Instance<LoadingPanel>();
        GameProgress prog = new GameProgress
            (
                doneAct: null,
                t: new GameProgress.Task()
                {
                    Name = "check version",
                    GetProgress = delegate { return 1; },
                    IsDone = delegate { return checkVersionDone; },
                    Start = delegate { StartCoroutine(CheckVersion()); },
                    Title = "Checking version ..."
                }
            );
        gameTask.AddTask(prog);
    }

    private IEnumerator SqlCheck()
    {
        SqlValidator.OnStared();
        while (!SqlValidator.IsDone)
        {
            yield return null;
        }
        InitProg();
        yield break;
    }
}
