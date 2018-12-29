using System;
using System.ComponentModel;
using System.Net;
using UnityEngine;

[DisallowMultipleComponent]
public sealed class DownloadFileAsync : MonoBehaviour
{
    public static DownloadFileAsync Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    public WebClient DownloadFile(string link, string saveAt)
    {
        if (link != null && link != "" &&
            saveAt != null && saveAt != "")
        {
            WebClient client = new WebClient();
            client.DownloadFileAsync(new Uri(link), saveAt);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler((object sender, AsyncCompletedEventArgs e) =>
            {
                ((WebClient)sender).Dispose();
            });
            return client;
        }
        return null;
    }

}