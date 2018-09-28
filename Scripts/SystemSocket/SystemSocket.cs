using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class SystemSocket : MonoBehaviour {
    public static SystemSocket instance;
    [HideInInspector]
    public SocketIOComponent SocketIO;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        SocketIO = gameObject.GetComponent<SocketIOComponent>();
    }
    void Start () {
        DontDestroyOnLoad(this);
	}
	
	
}
