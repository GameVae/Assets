﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnOffAction(GUIOnOffSwitch onOff);

[RequireComponent(typeof(Button), typeof(Image))]
public class GUIOnOffSwitch : CustomGUI
{
    [SerializeField, HideInInspector] private Button button;
    [SerializeField, HideInInspector] private Image image;

    public Button Button
    {
        get { return button ?? (button = GetComponent<Button>()); }
        protected set { button = value; }
    }
    public Image Image
    {
        get { return image ?? (image = GetComponent<Image>()); }
        protected set { image = value; }
    }
    public bool IsOn { get; protected set; }

    public Func<bool> CanSwitch;
    public event OnOffAction On;
    public event OnOffAction Off;

    public Button.ButtonClickedEvent OnClick;

    private void Awake()
    {
        Button = GetComponent<Button>();
        Image = GetComponent<Image>();
        Placeholder = GetComponentInChildren<TextMeshProUGUI>();
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
        if (!CanSwitch() || !Interactable) return;
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

    public override void InteractableChange(bool value)
    {
        Interactable = value;
        Button.interactable = value;
    }
}
