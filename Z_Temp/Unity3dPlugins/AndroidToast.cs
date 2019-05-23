#if UNITY_ANDROID
using Generic.Singleton;
using UnityEngine;
using System;


public sealed class AndroidToast : MonoSingle<AndroidToast>
{
    private AndroidJavaClass toast;
    private AndroidJavaObject activityContext;

    private void Start()
    {
        try
        {
            using (AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                activityContext = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
                //using (AndroidJavaClass toastClass = new AndroidJavaClass("com.unity.u3dplugins.ToastPlugin"))
                //{
                //    //toast = toastClass.CallStatic<AndroidJavaObject>("GetInstance");
                //    //toast.Call("SetContext", activityContext);
                //}
                toast = new AndroidJavaClass("com.unity.u3dplugins.ToastPlugin");
            }

        }
        catch (Exception e)
        {
            Debugger.Log(e.ToString());
        }
    }

    public void ShowToast()
    {
        try
        {
            toast.CallStatic("showToast", activityContext, "This is a message call from plugin");
            Debugger.Log("toast.show called: toast " + toast + " - activityContext " + activityContext);
        }
        catch (Exception e)
        {
            Debugger.Log(e.ToString());
        }
    }

    public void ShowToast(string msg)
    {
        try
        {
            toast.Call("ShowPopup", msg);
            Debugger.Log("toast.show called: toast " + toast + " - activityContext " + activityContext);
        }
        catch (Exception e)
        {
            Debugger.Log(e.ToString());
        }
    }
}
#endif
