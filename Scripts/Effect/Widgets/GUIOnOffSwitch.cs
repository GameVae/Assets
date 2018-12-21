using System;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnOffAction(GUIOnOffSwitch onOff);

[RequireComponent(typeof(Button), typeof(Image))]
public class GUIOnOffSwitch : MonoBehaviour
{
    public Button Button { get; private set; }
    public bool IsOn { get; private set; }
    public Image Image { get; private set; }

    public Func<bool> CanSwitch;
    public event OnOffAction On;
    public event OnOffAction Off;

    public Button.ButtonClickedEvent OnClick;

    private void Awake()
    {
        Button = GetComponent<Button>();
        Image = GetComponent<Image>();
        Button.onClick = OnClick;
    }

    private void Start()
    {
        Button.onClick.AddListener(OnOffEffect);
        if (On == null)
            On += delegate { Image.color = Color.white; };
        if (Off == null)
            Off += delegate { Image.color = Color.gray; };
        CanSwitch = CanSwitch ?? delegate { return true; };
        Off(this);
    }

    private void OnOffEffect()
    {
        if (!CanSwitch()) return;
        if (!IsOn)
            SwitchOn();
        else
            SwitchOff();
    }

    public void SwitchOff()
    {
        Off(this);
        IsOn = false;
    }

    public void SwitchOn()
    {
        On(this);
        IsOn = true;
    }
}
