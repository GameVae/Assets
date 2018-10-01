using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Require at least 1 EventSystem
/// </summary>
public class Debugger : MonoBehaviour
{
    public static Debugger debuger;
    private GameObject content;
    private Text message;
    private Scrollbar verticleBar;

    private List<Text> logs;
	private void Awake ()
    {
        if (debuger == null) debuger = this;
        else if (debuger != null) Destroy(debuger);
        logs = new List<Text>();

        message = GetComponentInChildren<Text>();
        verticleBar = GetComponentInChildren<Scrollbar>();
        content = GetComponentInChildren<ContentSizeFitter>().gameObject;

        message.gameObject.SetActive(false); // prefab
	}
    private void Start()
    {
        Clear();
    }
    public void Clear()
    {
        for(int i = 0; i < logs.Count; i++)
        {
            Destroy(logs[i].gameObject);
        }
        logs.Clear();
    }

    public void Log(object obj)
    {
#if UNITY_ANDROID

        Text log = Instantiate(message, content.transform);
        log.text = DateTime.Now + " : " + obj.ToString();
        log.gameObject.SetActive(true);
        logs.Add(log);
        verticleBar.value = 0;
#endif
#if UNITY_EDITOR
        Debug.Log(obj);
#endif
    }
}
