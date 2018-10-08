using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static float PixelDependencyDevice;
    private void Awake()
    {

        PixelDependencyDevice = 1.0f / Screen.dpi;        
    }
    private void Start()
    {
        Debugger.instance.Log("DPI: " + Screen.dpi);
    }
}
