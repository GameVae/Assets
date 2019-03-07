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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        //string dateInput = "Wed Aug 01 2018 08:55:04 GMT+0700";
        //string dateInput = "Jul 31 2018 22:16:25";
        //CalcTime(dateInput);

        Debug.Log("LocalTimeNow: " + LocalTimeNow);
        Debug.Log("UTCTimeNow: " + UTCTimeNow);
        //2018 - 10 - 18 09:34:58
        //Debug.Log("Test Time: "+ CalcToLocalTime("null"));
       // Debug.Log("Test Mins Time: " +(CalcToLocalTime("2018 - 11 - 06 2:49:58") - LocalTimeNow).TotalSeconds);
    }

    public DateTime CalcToLocalTime(string dateInput)
    {
        DateTime retDateTime = DateTime.Parse(dateInput);
        //Debug.Log("retDateTime: " + retDateTime);
        return retDateTime.ToLocalTime();
    }

    public double CalculateTime(DateTime date1, DateTime date2)
    {
        int retSeconds = 0;
        retSeconds = int.Parse((date1 - date2).TotalSeconds.ToString());
        return retSeconds;
    }
}
