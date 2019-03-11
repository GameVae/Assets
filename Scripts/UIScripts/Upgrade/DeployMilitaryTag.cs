using EnumCollect;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;

public class DeployMilitaryTag : MonoBehaviour
{
    public GUIInteractableIcon Icon;
    public GUIProgressSlider Slider;
    public InputField InputField;
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
        InputField.onValueChanged.AddListener((string value) => OnInputFieldValueChanged(value));
        Slider.OnValueChanged.AddListener((float value) => OnSliderValueChanged(value));
    }

    private void OnInputFieldValueChanged(string value)
    {
        int iV = int.Parse(value);
        iV = Mathf.Clamp(iV, 0,(int)MaxQuality);

        InputField.text = iV.ToString();
        Slider.Value = iV;
        deployWind.TagSelected(this);
    }

    private void OnSliderValueChanged(float value)
    {
        int iV = (int)value;

        Slider.Value = iV;
        InputField.text = iV.ToString();
        deployWind.TagSelected(this);
    }

    public void SetValue(int value)
    {
        Slider.Value = 0;
    }
}
