using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalCircleBtn : MonoBehaviour {
    private float width;
    private float height;

    void Start()
    {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;
        if (width < height)
        {
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, width);
        }
        else if (width > height)
        {
            GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, height);
        }
    }
}
