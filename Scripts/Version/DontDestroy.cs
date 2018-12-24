using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour {
    public static DontDestroy instance;
    void Awake () {
        if (instance == null) instance = this;
        else { Destroy(gameObject); }


        DontDestroyOnLoad(this);
	}


    public void DestroyData()
    {
        Destroy(gameObject);
    }
}
