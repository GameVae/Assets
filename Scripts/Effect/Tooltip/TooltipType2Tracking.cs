using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipType2Tracking : MonoBehaviour
{
    private List<RaycastResult> result;

    private Text contentTxt;
    private Image tooltipImg;
    private EventSystem eventSystem;
    private PointerEventData pointData;
    private GraphicRaycaster raycaster;
    private void Awake()
    {
        contentTxt = GetComponentInChildren<Text>();
        tooltipImg = GetComponentInChildren<Image>();
        // raycaster
        raycaster = FindObjectOfType<GraphicRaycaster>();
        eventSystem = FindObjectOfType<EventSystem>();
        result = new List<RaycastResult>();  
    }
    private void Start()
    {
        HideTooltip();
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(!Tracking())
            {
                HideTooltip();
            }
        }
    }

    private void HideTooltip()
    {
        contentTxt.gameObject.SetActive(false);
        tooltipImg.gameObject.SetActive(false);
    }

    private bool Tracking()
    {
        pointData = new PointerEventData(eventSystem)
        {
            position = Input.mousePosition
        };
        if (result != null) result.Clear();
        raycaster.Raycast(pointData, result);
        if (result != null)
        {
            Tooltip temp = null;
            for (int i = 0; i < result.Count; i++)
            {
                temp = result[i].gameObject.GetComponent<Tooltip>();
                if(temp != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
