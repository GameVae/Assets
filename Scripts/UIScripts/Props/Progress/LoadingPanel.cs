using Generic.Singleton;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;
using UnityGameTask;
using static GameProgress;

public sealed class LoadingPanel : MonoSingle<LoadingPanel>
{
    public Image Background;
    public GameObject Panel;
    public GUIProgressSlider ProgressBar;
    public TextMeshProUGUI LoadingInfo;

    private bool isOpen;
    private bool isClosing;
    private float targetProgress;

    public float TargetProgress
    {
        get
        {
            return  targetProgress;
        }
    }

    private void Update()
    {
        if (isOpen)
        {
            ProgressBar.Value = Mathf.MoveTowards(ProgressBar.Value, targetProgress, Time.deltaTime);
            if (ProgressBar.Value == targetProgress && isClosing)
            {
                Panel.SetActive(false);
                isOpen = false;
            }
        }
    }

    public void Open()
    {
        isOpen = true;
        isClosing = false;

        Panel.SetActive(true);
        ProgressBar.Value = 0;
    }

    public void Close()
    {
        isClosing = true;
    }

    public void SetValue(float value)
    {
        ProgressBar.Value = value;
        Debugger.Log(value);
    }

    public void UpdateValue(float value)
    {
        targetProgress = value;
    }
}
