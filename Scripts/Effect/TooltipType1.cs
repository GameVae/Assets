using UnityEngine;
using UnityEngine.UI;

public class TooltipType1 : MonoBehaviour
{
    public static TooltipType1 Instance;

    private bool isTrigger;
    private bool isOpened;
    private float lifeTimeCounter;

    private Text contentTxt;
    private Image tooltipImg;
    private Vector2 dimention;
    private FadeInOut bgFadeColor;
    private FadeInOut txtFadeColor;
    private RectTransform txtRectTrans;
    private RectTransform tooltipTrans;
    private ContentSizeFitter sizeFitter;


    public float LifeTime;
    public float ExpandSpeed;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != null) Destroy(Instance.gameObject);

        contentTxt = GetComponentInChildren<Text>();
        sizeFitter = contentTxt.GetComponent<ContentSizeFitter>();
        txtRectTrans = contentTxt.GetComponent<RectTransform>();
        tooltipImg = GetComponentInChildren<Image>();
        bgFadeColor = tooltipImg.GetComponent<FadeInOut>();
        txtFadeColor = contentTxt.GetComponent<FadeInOut>();
        tooltipTrans = (RectTransform)transform;

        // config
        HideTooltip();
        tooltipTrans.SetAsLastSibling();
    }
    private void Update()
    {
        if (!isTrigger) return;
        if (!isOpened)
        {
            tooltipTrans.sizeDelta = Vector2.Lerp(tooltipTrans.sizeDelta, dimention, ExpandSpeed * Time.deltaTime);
            if (dimention.magnitude - tooltipTrans.sizeDelta.magnitude < 0.25f)
            {
                tooltipTrans.sizeDelta = dimention;
                isOpened = true;
                contentTxt.gameObject.SetActive(true);
            }
        }
        else
        {
            LifeTimeCounter();
        }
        if (lifeTimeCounter <= 0 && !bgFadeColor.IsPlaying)
        {
            HideTooltip();
        }
    }

    private void HideTooltip()
    {
        isOpened = false;
        isTrigger = false;
        tooltipImg.gameObject.SetActive(false);
        contentTxt.gameObject.SetActive(false);
        bgFadeColor.ResetColor();
    }
    private void InitTooltip(string content)
    {
       
            tooltipTrans.sizeDelta = new Vector2(0.25f, 0.25f);
            tooltipImg.gameObject.SetActive(true);
            isTrigger = true;
            lifeTimeCounter = LifeTime;
            //-----------------------------------------------//
            /// SET TEXT AND RE-CALCULATE SIZE BOUNDING BOX /// 
            //-----------------------------------------------//
            contentTxt.gameObject.SetActive(true);
            //Canvas.ForceUpdateCanvases();
            contentTxt.text = content;
            LayoutRebuilder.ForceRebuildLayoutImmediate(txtRectTrans);
            dimention = txtRectTrans.sizeDelta;
            dimention = dimention + dimention * 0.5f;
            txtFadeColor.IsFadeIn = true;
            contentTxt.gameObject.SetActive(false);
    }
    private void InitFadeOut()
    {
        bgFadeColor.Play();
        txtFadeColor.FadeSpeed = bgFadeColor.FadeSpeed * 1.2f;
        txtFadeColor.IsFadeIn = false;
        txtFadeColor.Play();
    }
    private void LifeTimeCounter()
    {
        if (lifeTimeCounter <= 0) return;
        lifeTimeCounter -= Time.deltaTime;
        if (lifeTimeCounter <= 0)
        {
            lifeTimeCounter = 0;
            InitFadeOut();
        }
    }

    /// <summary>
    /// Show tooltip at specified position
    /// </summary>
    /// <param name="position">Local position</param>
    /// <param name="content">Tooltip's content</param>
    public void DisplayTooltip(Vector3 position, string content)
    {
        if (!isTrigger)
        {
            InitTooltip(content);
            tooltipTrans.localPosition = position;
        }
    }

    // uitls
    public void ResiseFont(int size)
    {
        contentTxt.fontSize = size;
    }
    public void SetTextColor(Color color)
    {
        contentTxt.color = color;
        txtFadeColor.SetDefaultColor(color);
    }
}
