using EnumCollect;
using UI.Composites;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;

public class DeployMilitaryTag : MonoBehaviour
{
    public GUIInteractableIcon Icon;
    public GUIProgressSlider Slider;
    public CustomInputField InputField;
    public PlaceholderComp RemainInfo;
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

    private void Start()
    {
        InputField.OnSelected += () => FocusThisField(true);
        InputField.OnValueChanged += OnInputValueChanged;
        Slider.OnValueChanged.AddListener(OnSliderValueChaned);
    }

    public void Refresh()
    {
        Slider.Value = 0;
        InputField.Keyboard?.Close();
        InputField.Active(false);
    }

    private void OnSliderValueChaned(float v)
    {
        FocusThisField(false);
        SetContent(((int)v));
    }

    private void OnInputValueChanged(string value)
    {
        FocusThisField(true);
        int.TryParse(value, out int iValue);
        iValue = Mathf.Clamp(iValue, 0, (int)MaxQuality);
        Slider.Value = iValue;

        SetContent(iValue);
    }

    private void SetContent(int c)
    {
        InputField.SetContent(c.ToString());
        if (RemainInfo != null)
            RemainInfo.Text = string.Format("/{0}", Slider.MaxValue);
    }

    private void FocusThisField(bool isInput)
    {
        deployWind.TagSelected(this,isInput);
    }
}
