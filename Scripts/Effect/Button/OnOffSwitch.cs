using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

public delegate void OnOffSwitchEffect(OnOffSwitch onOff);

[RequireComponent(typeof(Button),typeof(Image))]
public class OnOffSwitch : MonoBehaviour
{
    private Button button;

    public bool IsOn { get; private set; }
    public Image Image { get; private set; }

    public Func<bool> CanSwitch;
    public event OnOffSwitchEffect On;
    public event OnOffSwitchEffect Off;

    public ButtonClickedEvent OnClick;

    private void Awake()
    {
        button = GetComponent<Button>();
        Image = GetComponent<Image>();
        button.onClick = OnClick;
    }

    private void Start()
    {
        button.onClick.AddListener(OnOffEffect);
        if (On == null || Off == null)
        {
            On += delegate { Image.color = Color.white; };
            Off += delegate { Image.color = Color.gray; };
        }
        CanSwitch = CanSwitch ?? delegate { return true; };
        Off(this);
    }

    private void OnOffEffect()
    {
        if (!CanSwitch()) return;
        IsOn = !IsOn;
        if (IsOn)
            On(this);
        else
            Off(this);
    }
}
