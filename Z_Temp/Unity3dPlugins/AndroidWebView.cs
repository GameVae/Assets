using System;
using UnityEngine;

public class AndroidWebView : MonoBehaviour
{
    private AndroidJavaObject intent;
    private AndroidJavaObject activityContext;

    public int value;

    private void Awake()
    {
        try
        {
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                using (AndroidJavaClass webViewClass = new AndroidJavaClass("com.unity.u3dplugins.WebViewPlugin"))
                {
                    intent = webViewClass.CallStatic<AndroidJavaObject>("getIntent", activityContext);
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

            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                Debugger.Log(activityClass.GetStatic<AndroidJavaObject>("currentActivity").ToString());                
            }

        }
        catch (Exception e)
        {
            Debugger.Log(e.ToString());
        }
    }
}
