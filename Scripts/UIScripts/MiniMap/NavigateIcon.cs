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

    private Image image;
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
        trans = (RectTransform)transform;
        image = GetComponent<Image>();

        image.enabled = false;
        imageGrap = image;
        defaultColor = imageGrap.color;
        maxValue = 100.0f * (1 - MinAlpha);
        Rectangle = new Rect(Vector2.zero, image.rectTransform.sizeDelta);
    }
    private void OnEnable()
    {
        imageGrap.color = defaultColor;
    }

    private void Update()
    {
        if (image.enabled)
        {
            alphaCounter = Mathf.PingPong(Time.time * ColorAniamtionSpeed, maxValue) * 0.01f + MinAlpha;
            tempColor = defaultColor;
            tempColor.a *= alphaCounter;
            imageGrap.color = tempColor;
        }
    }

    public void Disable()
    {
        image.enabled = false;
    }

    public void SetPosition(Vector3 position)
    {
        if (!image.enabled) image.enabled = true;
        trans.localPosition = position;
    }
}
