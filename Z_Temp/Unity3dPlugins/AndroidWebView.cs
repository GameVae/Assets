using System;
using System.Collections;
using UnityEngine;

public class AndroidWebView : MonoBehaviour
{
    private AndroidJavaObject intent;
    private AndroidJavaObject activityContext;

    public int value;
    private void Start()
    {
        try
        {
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                using (AndroidJavaClass unity3dWv = new AndroidJavaClass("com.unity3d.unity3dtoast.Unity3dWebView"))
                {
                    intent = unity3dWv.CallStatic<AndroidJavaObject>("GetIntent", activityContext);
                    Debugger.Log(intent == null ? "intent null" : intent.ToString());
                }
            }

        }
        catch (Exception e)
        {
            Debugger.Log(e.ToString());
        }

        StartCoroutine(FibonacciLog(value));
    }

    public void LoadWeb()
    {
        try
        {
            activityContext.Call("startActivity", intent);
        }
        catch (Exception e)
        {
            Debugger.Log(e.ToString());
        }
    }

    public int Fibonacci(int index)
    {
        if (index < 0) return 0;
        if (index == 0 || index == 1) return 1;
        return Fibonacci(index - 1) + Fibonacci(index - 2);
    }

    private IEnumerator FibonacciLog(int maxIndex)
    {
        int index = 0;
        while (index < maxIndex)
        {
            Debugger.Log(index + ": " + Fibonacci(index));
            index++;
            yield return null;
        }
    }
}
