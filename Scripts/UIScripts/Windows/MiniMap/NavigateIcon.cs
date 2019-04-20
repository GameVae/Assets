using UnityEngine;
using UnityEngine.UI;

public class NavigateIcon : MonoBehaviour
{
    [Range(min: 1, max: float.MaxValue)]
    public float ColorAniamtionSpeed;
    [Range(min: 0, max: 1)]
    public float MinAlpha;

    private float alphaCounter;
    private float maxValue;

    public Image Icon;
    private Color tempColor;
    private Color defaultColor;
    private Graphic imageGrap;
    private Rect rectangle;
    private RectTransform trans;

    public Rect Rectangle
    {
        get
        {
            rectangle.x = trans.localPosition.x - (rectangle.width / 2);
            rectangle.y = trans.localPosition.y - (rectangle.height / 2);
            return rectangle;
        }
        private set { rectangle = value; }
    }

    private void Awake()
    {
        trans = transform as RectTransform;

        Icon.enabled = false;
        imageGrap = Icon;
        defaultColor = imageGrap.color;
        maxValue = 100.0f * (1 - MinAlpha);
        Rectangle = new Rect(Vector2.zero, Icon.rectTransform.sizeDelta);
    }
    private void OnEnable()
    {
        imageGrap.color = defaultColor;
    }

    private void Update()
    {
        if (Icon.enabled)
        {
            alphaCounter = Mathf.PingPong(Time.time * ColorAniamtionSpeed, maxValue) * 0.01f + MinAlpha;
            tempColor = defaultColor;
            tempColor.a *= alphaCounter;
            imageGrap.color = tempColor;
        }
    }

    public void Disable()
    {
        Icon.enabled = false;
    }

    public void SetPosition(Vector3 position)
    {
        if (!Icon.enabled) Icon.enabled = true;
        trans.localPosition = position;

        //Debugger.Log("local pos: " + trans.localPosition);
        //Debugger.Log("anchored pos: " + trans.anchoredPosition);
        //Debugger.Log("world pos: " + trans.position);
        //Debugger.Log(trans.rect);
    }
}
