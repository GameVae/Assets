using UnityEngine;

public struct GMath
{
    public static float PingPong(float t, float min, float max)
    {
        return Mathf.Lerp(min, max, Mathf.PingPong(t, 1));
    }

    public static float Round(float value,int digits)
    {
        return(float)System.Math.Round(value, digits);
    }

    public static float SecondToMilisecond(float second)
    {
        return second * 1000f;
    }
}
