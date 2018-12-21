using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class GUIProgressSlider : CustomGUI
{
    public bool IsShowText { get; private set; }
    public Slider Slider { get; private set; }
    public TextMeshProUGUI PlaceHolder { get; private set; }    

    public float Value
    {
        get { return Slider.value; }
        set
        {
            Slider.value = value;
            if (IsShowText && PlaceHolder)
                PlaceHolder.text = string.Format("{0}/{1}", Value, MaxValue);
        }
    }

    public float MaxValue
    {
        get { return Slider.maxValue; }
        set { Slider.maxValue = value; }
    }

    public float MinValue
    {
        get { return Slider.minValue; }
        set { Slider.minValue = value; }
    }
    
    public Graphic Backgroud
    {
        get { return Slider.targetGraphic; }
        set { Slider.targetGraphic = value; }
    }

    private void Awake()
    {
        PlaceHolder = GetComponentInChildren<TextMeshProUGUI>();
        Slider = GetComponent<Slider>();
        Slider.interactable = false;
    }

    public void PlaceholderText(string text)
    {
        if(PlaceHolder == null)
            PlaceHolder = GetComponentInChildren<TextMeshProUGUI>();
        PlaceHolder.text = text;
    }

    public override void InteractableChange(bool value)
    {
        if (Slider == null)
            Slider = GetComponent<Slider>();
        Slider.interactable = value;
        Interactable = value;
    }

    public void IsShowTextChange(bool value)
    {
        if (PlaceHolder == null)
            PlaceHolder = GetComponentInChildren<TextMeshProUGUI>();
        PlaceHolder.enabled = value;
        IsShowText = value;
    }
}
