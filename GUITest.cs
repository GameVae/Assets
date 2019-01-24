using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUITest : MonoBehaviour {

    public Sprite sprite;
    public Image image; 

    [ContextMenu("apply")]
    public void Apply()
    {
        image.sprite = sprite;
    }
}
