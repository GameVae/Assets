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
        SetContent(((int)v));
        FocusThisField();
    }

    private void OnInputValueChanged(string value)
    {
        int.TryParse(value, out int iValue);

        iValue = Mathf.Clamp(iValue, 0, (int)MaxQuality);

        SetContent(iValue);

        Slider.Value = iValue;
        FocusThisField();
    }

    private void SetContent(int c)
    {
        InputField.SetContent(c.ToString());
        if (RemainInfo != null)
            RemainInfo.Text = string.Format("/{0}", Slider.MaxValue);
    }

    private void FocusThisField()
    {
        deployWind.TagSelected(this);
    }
}
