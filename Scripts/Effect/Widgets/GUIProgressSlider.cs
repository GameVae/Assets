using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class GUIProgressSlider : CustomGUI
{
    [SerializeField, HideInInspector] Slider slider;
    public Slider Slider
    {
        get { return slider ?? (slider = GetComponent<Slider>()); }
        protected set { slider = value; }
    }

    public float Value
    {
        get { return Slider == null ? 0 : Slider.value; }
        set
        {
            if (Slider != null)
            {
                if (IsPlaceholder && Placeholder)
                    Placeholder.text = string.Format("{0}/{1}", Value, MaxValue);
                Slider.value = value;
            }
        }
    }

    public float MaxValue
    {
        get { return Slider == null ? 0 : Slider.maxValue; }
        set { if (Slider != null) Slider.maxValue = value; }
    }

    public float MinValue
    {
        get { return Slider == null ? 0 : Slider.minValue; }
        set { if (Slider != null) Slider.minValue = value; }
    }

    public Graphic Backgroud
    {
        get { return Slider?.targetGraphic; }
        set { if (Slider) Slider.targetGraphic = value; }
    }

    private void Awake()
    {
        Placeholder = GetComponentInChildren<TextMeshProUGUI>();
        Slider = GetComponent<Slider>();
    }

    public override void InteractableChange(bool value)
    {
        if (Slider != null)
        {
            Slider.interactable = value;
            Interactable = value;
        }
    }
}
