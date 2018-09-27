using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStatus : MonoBehaviour {

    private bool invertBool;
    private int currentZoom;

    public static CameraStatus instance;
    public bool InvertBool { get => invertBool; set => invertBool = value; }
    public int CurrentZoom { get => currentZoom; set => currentZoom = value; }

    void Awake () {
       
        if (PlayerPrefs.HasKey("InvertBool"))
        {
            InvertBool = PlayerPrefs.HasKey("InvertBool") ? true : false;
        }
        instance = this;

    }
	
	
}
