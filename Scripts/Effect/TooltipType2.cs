using System;
using UnityEngine;
using UnityEngine.UI;

public class TooltipType2 : MonoBehaviour
{
    public static TooltipType2 Instance;

    private Text contentTxt;
    private Image tooltipImg;
    private RectTransform tooltipTrans;
    private Vector2 dimention;
    private RectTransform txtRectTrans;
    private ContentSizeFitter sizeFitter;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null) Destroy(Instance.gameObject);
        Reset();
    }
    private void Start()
    {       
        // config
        tooltipTrans.SetAsLastSibling();
    }
    private void Reset()
    {
        contentTxt = GetComponentInChildren<Text>();
        sizeFitter = contentTxt.GetComponent<ContentSizeFitter>();
        txtRectTrans = contentTxt.GetComponent<RectTransform>();
        tooltipImg = GetComponentInChildren<Image>();
        tooltipTrans = (RectTransform)transform;
    }

    private void InitTooltip(string content)
    {
        //-----------------------------------------------//
        /// SET TEXT AND RE-CALCULATE SIZE BOUNDING BOX /// 
        //-----------------------------------------------//  
        tooltipImg.gameObject.SetActive(true);
        contentTxt.gameObject.SetActive(true);
        contentTxt.text = content;
        LayoutRebuilder.ForceRebuildLayoutImmediate(txtRectTrans);
        dimention = txtRectTrans.sizeDelta;
        dimention = dimention + dimention * 0.5f;
        tooltipTrans.sizeDelta = dimention;
        
    }

    public void ResiseFont(int size)
    {
        contentTxt.fontSize = size;
    }

    public void DisplayTooltip(Vector3 position, string content)
    {
        InitTooltip(content);
        tooltipTrans.localPosition = position;
    }
}
