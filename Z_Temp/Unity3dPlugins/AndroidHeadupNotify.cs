using System;
using UnityEngine;

public class AndroidHeadupNotify : MonoBehaviour
{
    private AndroidJavaObject utils;
    private AndroidJavaObject context;

    public AndroidAdbLog Logger;
    private void Awake()
    {
        try
        {
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                context = activityClass.GetStatic<AndroidJavaObject>("currentActivity");

                using (AndroidJavaClass utilclass = new AndroidJavaClass("com.unity.u3dplugins.Utils"))
                {
                    utils = utilclass.CallStatic<AndroidJavaObject>("getInstance");
                }
            }
        }
        catch (Exception e)
        {
            Debugger.Log(e.ToString());
        }
    }

    private void Start()
    {
        if (utils == null)
            Logger.LogW("Utils instance is null");
        else
        {
            Logger.LogI("Utils :" + utils);
        }
    }

    public void ShowNotify()
    {
        utils.Call("notify", context);
    }
}
