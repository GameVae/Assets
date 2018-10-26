using UnityEngine;
using System;

public class TimeControl : MonoBehaviour
{
    public static TimeControl instance;
    private DateTime localTimeNow;
    private DateTime uTCTimeNow;

    public DateTime LocalTimeNow
    {
        get
        {
            localTimeNow = DateTime.Now;
            return localTimeNow;
        }
    }

    public DateTime UTCTimeNow
    {
        get
        {
            uTCTimeNow = DateTime.UtcNow;
            return uTCTimeNow;
        }
    }

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
       
        //string dateInput = "Wed Aug 01 2018 08:55:04 GMT+0700";
        //string dateInput = "Jul 31 2018 22:16:25";
        //CalcTime(dateInput);
        Debug.Log("LocalTimeNow: "+LocalTimeNow);
        Debug.Log("UTCTimeNow: "+ UTCTimeNow);
    }

    public DateTime CalcToLocalTime(string dateInput)
    {
        DateTime retDateTime = DateTime.Parse(dateInput);
        Debug.Log("retDateTime: "+retDateTime.ToLocalTime());
        return retDateTime.ToLocalTime();
    }

}
 