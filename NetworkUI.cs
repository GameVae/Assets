using Generic.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkUI : MonoBehaviour
{
    public float Angualar;
    public GameObject Panel;
    public Connection Conn;
    public RectTransform WaitingUI;

    private bool isActive;
    private void Active(bool value)
    {
        Panel.SetActive(value);
        isActive = value;
    }

    private void Start()
    {
        if (!Conn)
            Conn = Singleton.Instance<Connection>();
    }

    private void FixedUpdate()
    {
        if (!isActive)
        {
            if (!Conn.IsServerConnected)
                Active(true);
        }
        else
        {
            WaitingUI.Rotate(0, 0, Angualar * Time.deltaTime);
            if(Conn.IsServerConnected)
            {
                Active(false);
            }
        }

    }
}
