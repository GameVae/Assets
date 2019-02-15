using Generic.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Require at least 1 EventSystem
/// </summary>
public sealed class Debugger : MonoSingle<Debugger>
{
    private static Debugger ins = null;
    [SerializeField]
    private int MaxSentence = 300;
    private int LogCount;

    private RectTransform content;
    private TextMeshProUGUI logPrefab;
    private Queue<TextMeshProUGUI> logs;

    public GUIScrollView ScrollView;

    private Queue<TextMeshProUGUI> Logs
    {
        get { return logs ?? (logs = new Queue<TextMeshProUGUI>()); }
    }

    protected override void Awake()
    {
        base.Awake();

        ScrollView = GetComponent<GUIScrollView>();

        content = ScrollView.ScrollRect.content;
        MaxSentence = MaxSentence == 0 ? 300 : MaxSentence;

        CreatePrefab();
    }

    private TextMeshProUGUI CreateSentence()
    {
        TextMeshProUGUI text = null;
        if (LogCount < MaxSentence)
        {
            text = Instantiate(logPrefab, content);
            LogCount++;
        }
        else
        {
            text = Logs.Dequeue();
            text.text = "";
            text.rectTransform.SetAsLastSibling();
        }

        ScrollView.ScrollRect.verticalNormalizedPosition = 0.0f;
        Logs.Enqueue(text);
        text.gameObject.SetActive(true);
        return text;
    }

    public void Clear()
    {
        foreach (TextMeshProUGUI item in Logs)
        {
            item.gameObject.SetActive(false);
        }
    }

    #region TEST
    private IEnumerator PrintMessage()
    {
        int i = 0;
        while (i < 300)
        {
            CreateSentence().text = i + DateTime.Now.ToString();
            yield return null;
            i++;
        }
        yield break;
    }
    #endregion

    private void CreatePrefab()
    {
        RectTransform sentence = new GameObject("Prefab", typeof(TextMeshProUGUI), typeof(ContentSizeFitter)).GetComponent<RectTransform>();
        sentence.SetParent(content);
        sentence.pivot = new Vector2(0, 1);
        sentence.localPosition = Vector3.zero;
        sentence.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, content.RealSize().x);

        TextMeshProUGUI text = sentence.GetComponent<TextMeshProUGUI>();
        text.raycastTarget = false;
        text.enableAutoSizing = true;
        text.fontSizeMax = 32;
        text.fontSizeMin = 20;

        ContentSizeFitter sizeFitter = sentence.GetComponent<ContentSizeFitter>();
        sizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        logPrefab = text;
        logPrefab.gameObject.SetActive(false);
    }

    public static void Log(object obj)
    {
        
        if (ins == null)
            ins = Singleton.Instance<Debugger>();
#if UNITY_ANDROID
        ins.CreateSentence().text = DateTime.Now.ToLongTimeString() + " : " + obj.ToString(); ;
#endif
#if UNITY_EDITOR
        Debug.Log(obj);
#endif
    }
}
