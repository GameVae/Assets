using UnityEngine;
using UnityEngine.UI;

public class NavigateIcon : MonoBehaviour
{   
    private Image image;
    private Rect rectangle;
    private RectTransform trans;

    public Rect Rectangle
    {
        get
        {
            rectangle.x = trans.position.x - (rectangle.width / 2);
            rectangle.y = trans.position.y - (rectangle.height / 2);
            return rectangle;
        }
        private set { rectangle = value; }
    }

    private void OnEnable()
    {
        trans = (RectTransform)transform;
        image = GetComponent<Image>();
        Rectangle = new Rect(0, 0, 50, 50);

    }

    public void Disable()
    {
        image.enabled = false;
    }

    public void SetPosition(Vector3 position)
    {
        if (!image.enabled) image.enabled = true;
        trans.position = position;
    }
}
