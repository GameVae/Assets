using System;
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
                using (AndroidJavaClass webViewClass = new AndroidJavaClass("com.unity.u3dplugins.WebViewPlugin"))
                {
                    intent = webViewClass.CallStatic<AndroidJavaObject>("getIntent", activityContext);

                    Debugger.Log("webViewClass " + webViewClass);
                    Debugger.Log("activityContext " + activityContext);
                    Debugger.Log("intent " + intent);
                }
            }

        }
        catch (Exception e)
        {
            Debugger.Log(e.ToString());
        }
    }

    public void LoadWeb()
    {
        try
        {
            intent.Call<AndroidJavaObject>("putExtra", "url", "https://www.google.com/");
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
}
