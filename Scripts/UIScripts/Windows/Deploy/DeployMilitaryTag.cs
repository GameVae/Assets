using EnumCollect;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;

public class DeployMilitaryTag : MonoBehaviour
{
    public GUIInteractableIcon Icon;
    public GUIProgressSlider Slider;
    public InputFieldv2 InputField;
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
        InputField.OnValueChanged += OnInputValueChanged;
        Slider.OnValueChanged.AddListener(OnSliderValueChaned);
    }

    public void Refresh()
    {
        Slider.Value = 0;
    }

    private void OnSliderValueChaned(float v)
    {
        InputField.SetContent(((int)v).ToString());
        FocusThisField();
    }

    private void OnInputValueChanged(string value)
    {
        int.TryParse(value, out int iValue);

        iValue = Mathf.Clamp(iValue, 0, (int)MaxQuality);
        if (InputField.Keyboard != null)
            InputField.Keyboard.InputString = (iValue).ToString();

        Slider.Value = iValue;
        FocusThisField();
    }

    private void FocusThisField()
    {
        deployWind.TagSelected(this);
    }
}
