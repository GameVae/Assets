using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidMsg : MonoBehaviour
{
    public void MsgCallback(string msg)
    {
        AndroidAdbLog.LogInfo(msg);
    }
}
