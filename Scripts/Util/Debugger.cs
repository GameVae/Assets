using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Require at least 1 EventSystem
/// </summary>
public class Debugger : MonoBehaviour
{
    public static Debugger instance;
    private GameObject content;
    private Text message;
    private Scrollbar verticleBar;

    private List<Text> logs;
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(instance);
        logs = new List<Text>();

        message = GetComponentInChildren<Text>();
        verticleBar = GetComponentInChildren<Scrollbar>();
        ContentSizeFitter contentObj = GetComponentInChildren<ContentSizeFitter>();
        if (contentObj != null) content = contentObj.gameObject;

        if (message != null)
            message.gameObject.SetActive(false); // prefab
    }
    private void Start()
    {
        Clear();
    }
    public void Clear()
    {
        for (int i = 0; i < logs.Count; i++)
        {
            Destroy(logs[i].gameObject);
        }
        logs.Clear();
    }

    public void Log(object obj)
    {
#if UNITY_ANDROID
        if (message != null && content != null && verticleBar != null)
        {
            if (logs.Count > 300)
            {
                for (int i = 0; i < 150; i++)
                {
                    Destroy(logs[i].gameObject);
                }
                logs.RemoveRange(0, 150);
                System.GC.Collect();
            }

            Text log = Instantiate(message, content.transform);
            log.text = DateTime.Now + " : " + obj.ToString();
            log.gameObject.SetActive(true);
            logs.Add(log);
            verticleBar.value = 0;
        }

#endif
#if UNITY_EDITOR
        Debug.Log(obj);
#endif
    }
}
