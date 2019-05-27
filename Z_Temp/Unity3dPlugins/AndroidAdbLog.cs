using Generic.Singleton;
using System;
using UnityEngine;

public class AndroidAdbLog : MonoSingle<AndroidAdbLog>
{
    public string Tag = "Unity";

    private static AndroidAdbLog instance;

    private AndroidJavaObject logger;

    protected override void Awake()
    {
        base.Awake();
        try
        {
            using (AndroidJavaClass adbLogClass = new AndroidJavaClass("com.unity.u3dplugins.ADBLogPlugin"))
            {
                logger = adbLogClass.CallStatic<AndroidJavaObject>("getInstance");
                SetTag(Tag);
            }
        }
        catch(Exception e)
        {
            Debugger.Log(e.ToString());
        }
    }

    public void LogI(string msg)
    {
        logger.Call("logI", msg);
    }

    public void LogW(string msg)
    {
        logger.Call("logW", msg);
    }

    public void LogE(string msg)
    {
        logger.Call("logE", msg);
    }

    public void SetTag(string tag)
    {
        logger.Call("setTag", tag);
    }

    public static void LogInfo(string msg)
    {
        if (instance == null)
            instance = Singleton.Instance<AndroidAdbLog>();
        instance.LogI(msg);
    }

    public static void LogInfo(object msg)
    {
        if (instance == null)
            instance = Singleton.Instance<AndroidAdbLog>();
        AndroidAdbLog.LogInfo(msg == null ? "null" : msg.ToString());
    }
}
