using Generic.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkWaitingUI : MonoBehaviour
{
    private bool isActive;
    private Image image;
    private Color defaultColor;

    public float Angular;
    public Graphic Background;

    private void Awake()
    {
        image = GetComponentInChildren<Image>();

        isActive = false;
        image.enabled = isActive;
        if(Background != null)
        {
            defaultColor = Background.color;
        }
    }

    public void Active(bool value)
    {
        isActive = value;
        image.enabled = isActive;
        if (Background != null)
        {
            if (value)
            {
                Color temp = defaultColor;
                temp *= 0.75f;
                Background.color = temp;
            }
            else
            {
                Background.color = defaultColor;
            }
        }
    }

    private void FixedUpdate()
    {
        if(isActive)
        {
            image.transform.Rotate(0, 0, Angular * Time.deltaTime);
        }
        if(!isActive)
        {
            if(Application.internetReachability == NetworkReachability.NotReachable)
            {
                Active(true);
            }
        } else
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                Active(false);
            }
        }
    }
}
