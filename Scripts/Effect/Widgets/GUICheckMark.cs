using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUICheckMark : CustomGUI
{
    public bool IsShowText { get; private set; }
    public Image MaskImage { get; private set; }
    public TextMeshProUGUI Placeholder { get; private set; }
    public GUIOnOffSwitch OnOffSwitch;

    public Sprite OnSprite;
    public Sprite OffSprite;

    public override bool Interactable
    {
        get { return base.Interactable; }

        protected set
        {
            if (value) Enable();
            else Disable();

            base.Interactable = value;
        }
    }

    private void Awake()
    {
        MaskImage = GetComponent<Image>();
        Placeholder = GetComponentInChildren<TextMeshProUGUI>();

        OnOffSwitch.CanSwitch = delegate { return Interactable; };
        OnOffSwitch.On += delegate
        {
            OnOffSwitch.Image.sprite = OnSprite;
            MaskImage.color = Color.cyan;
        };
        OnOffSwitch.Off += delegate
        {
            OnOffSwitch.Image.sprite = OffSprite;
            MaskImage.color = Color.white;
        };
        InteractableChange(true);
    }

    public override void InteractableChange(bool value)
    {
        Interactable = value;
    }

    public void PlaceholderText(string text)
    {
        if (Placeholder == null)
            Placeholder = GetComponentInChildren<TextMeshProUGUI>();
        if (Placeholder != null)
            Placeholder.text = text;
    }

    public void IsShowTextChange(bool value)
    {
        IsShowText = value;
        if (Placeholder == null)
            Placeholder = GetComponentInChildren<TextMeshProUGUI>();
        if (Placeholder != null)
            Placeholder.enabled = value;
    }

    public void SetGroup(GUIToggle group)
    {
        OnOffSwitch.On += delegate
        {
            if(group.ActiveMark != this)
            {
                group.ActiveMark?.OnOffSwitch.SwitchOff();
                group.ActiveMark = this;
                group.CheckActionCallback();
            }
        };
    }

    private void Disable()
    {
        if (MaskImage == null)
            MaskImage = GetComponent<Image>();
        MaskImage.color = Color.gray;
    }

    private void Enable()
    {
        if (MaskImage == null)
            MaskImage = GetComponent<Image>();
        MaskImage.color = Color.white;
    }
}
