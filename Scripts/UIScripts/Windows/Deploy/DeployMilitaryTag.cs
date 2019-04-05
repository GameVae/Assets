using EnumCollect;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;

public class DeployMilitaryTag : MonoBehaviour
{
    public GUIInteractableIcon Icon;
    public GUIProgressSlider Slider;
    public GUIInputField InputField;
    public ListUpgrade Type;

    public DeployMilitaryWindow deployWind;
    public float MaxQuality
    {
        get
        {
            return Slider.MaxValue;
        }
        set { Slider.MaxValue = value; }
    }

    private void Awake()
    {
        Slider.OnValueChanged.AddListener(OnSliderValueChanged);
        InputField.OnSelectField += delegate { deployWind.TagSelected(this); };
        InputField.OnValueChange += OnInputValueChanged;
    }   

    public void SetSliderValue(int value)
    {
        Slider.Value = value;
    }

    private void OnSliderValueChanged(float value)
    {
        int iValue = (int)value;

        SetSliderValue(iValue);
        deployWind.TagSelected(this);
        InputField.SetText(iValue.ToString());
    }

    private void OnInputValueChanged(string value)
    {
        int.TryParse(value, out int iValue);
        SetSliderValue(iValue);
    }
}
