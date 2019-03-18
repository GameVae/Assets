using System.Collections;
using System.Collections.Generic;
using UI.Tooltip;
using UI.Widget;
using UnityEngine;

public class MiniTaskWDOElement : MonoBehaviour
{
    public GUIInteractableIcon Icon;
    public GUIProgressSlider Progress;
    public GUIInteractableIcon InstButton;
    public FadeInOut Fader;
    public string Placeholder
    {
        get
        {
            return Progress.Placeholder.text;
        }
        set
        {
            Progress.Placeholder.text = value;
        }
    }

    private bool isTitleShown;
    private System.Func<string> getTitle;
    private System.Func<string> getRemainTime;
    private System.Func<string> currentPlaceholder;
    private System.Func<int> remainingTime;

    private void Awake()
    {
        Fader.LifeCycleDone += SwitchText;
    }

    private void FixedUpdate()
    {
        Placeholder = currentPlaceholder?.Invoke();
        Progress.Value = (int)remainingTime?.Invoke();

    }

    private void SwitchText()
    {
        currentPlaceholder = isTitleShown ? getRemainTime : getTitle;
        isTitleShown = !isTitleShown;
    }

    public void SetTitleFunc(System.Func<string> titleFunc)
    {
        getTitle = titleFunc;
    }

    public void SetGetTimeFunc(System.Func<string> timeFunc,int maxTime)
    {
        getRemainTime = timeFunc;
        Progress.MaxValue = maxTime;
    }

    public void SetSliderValue(System.Func<int> getRemainTime)
    {
        remainingTime = getRemainTime;
    }
}
