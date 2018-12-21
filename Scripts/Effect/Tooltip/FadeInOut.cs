using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class FadeInOut : MonoBehaviour
{
    public bool AutoPlay;
    public bool IsFadeIn;
    public float FadeSpeed;
    public bool IsPlaying { get; protected set; }

    private Color tempColor;
    private Color defaultColor;
    private Graphic uiGraphic;

    private void Awake()
    {
        IsPlaying = false;
        uiGraphic = GetComponent<Graphic>();
        defaultColor = uiGraphic.color;
    }
    private void OnEnable()
    {
        if(AutoPlay)
        {
            Play();
        }
    }
    public void SetDefaultColor(Color color)
    {
        defaultColor = color;
        tempColor = color;
    }
    public void ResetColor()
    {
        uiGraphic.color = defaultColor;
    }
    public void Play()
    {
        if (IsPlaying) return;
        if (uiGraphic == null)
        {
            IsPlaying = false;
#if UNITY_EDITOR
            Debug.LogError("UI Graphic not found !");
#endif
            return;
        }
        IsPlaying = true;
        tempColor = defaultColor;
        tempColor.a = IsFadeIn ? 0 : 1;
        uiGraphic.color = tempColor;
    }

    private void Update()
    {
        if(IsPlaying)
        {
            if (IsFadeIn)
            {
                tempColor.a = Mathf.Lerp(tempColor.a, 1, Time.deltaTime * FadeSpeed);
                if (tempColor.a >= 0.9f)
                {
                    tempColor.a = 1;
                    IsPlaying = false;
                }
            }
            else
            {
                tempColor.a = Mathf.Lerp(tempColor.a, 0, Time.deltaTime * FadeSpeed);
                if (tempColor.a <= 0.1f)
                {
                    IsPlaying = false;
                    tempColor.a = 0;
                }
            }
            uiGraphic.color = tempColor;
        }
    }

}
