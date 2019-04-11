﻿using System;
using Animation;
using Generic.Singleton;
using TMPro;
using UI.Composites;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Winker), typeof(SelectableComp))]
public partial class InputFieldv2 : MonoBehaviour
{
    public partial class ContentValidate
    {
        public enum ContentType
        {
            Interger,
            Text
        }
    }

    [SerializeField] private RectTransform textCursor;
    [SerializeField] private TextMeshProUGUI placeholder;
    [SerializeField] private TextMeshProUGUI textField;

    [Space, Header("Keyboard type")]
    [SerializeField] private Keyboard.KeyboardType keyboardType;
    private KeyboardProvider keyboardProvider;
    private Keyboard keyboard;

    [Space, Header("Content validate type")]
    [SerializeField] private ContentValidate.ContentType contentType;
    private ContentValidateProvider validateProvider;
    private ContentValidate validator;

    // Auto-Properties
    // CONTENT VALIDATOR
    protected ContentValidateProvider ValidateProvider
    {
        get { return validateProvider ?? (validateProvider = Singleton.Instance<ContentValidateProvider>()); }
    }
    public ContentValidate Validator
    {
        get
        {
            return validator ?? (validator = ValidateProvider.GetValidator(contentType));
        }
    }
    /////////////////////////////
    // KEYBOARD
    protected KeyboardProvider Provider
    {
        get
        {
            return keyboardProvider ?? (keyboardProvider = Singleton.Instance<KeyboardProvider>());
        }
    }
    public Keyboard Keyboard
    {
        get { return keyboard; }
    }
    ////////////////////////////

    private SelectableComp selectableComp;
    private UnityAction<string> onValueChanged;

    private Winker cursorWinker;
    private bool isActive;
    private string text;

    public string Text
    {
        get { return text; }
        private set
        {
            text = value;
            textField.text = text;
        }
    }

    protected SelectableComp SelectableComp
    {
        get
        {
            return selectableComp ?? (selectableComp = GetComponent<SelectableComp>());
        }
    }

    public event UnityAction OnSelected
    {
        add { SelectableComp.OnClickEvents += value; }
        remove { SelectableComp.OnClickEvents -= value; }
    }

    public event UnityAction<string> OnValueChanged
    {
        add { onValueChanged += value; }
        remove { onValueChanged -= value; }
    }

    private void Awake()
    {
        cursorWinker = GetComponent<Winker>();
        OnValueChanged += delegate { PlaceholderHidden(); };
        SelectableComp.OnClickEvents += OpenKeyboard;
    }

    private void Update()
    {
        if (isActive)
        {
            CalculateCursorPosition();
        }
    }

    private void CalculateCursorPosition()
    {
        float width = textField.renderedWidth;
        Vector2 position = textCursor.anchoredPosition;

        position.x = width;
        textCursor.anchoredPosition = position;
    }

    private void PlaceholderHidden()
    {
        if (placeholder.gameObject.activeInHierarchy)
        {
            if (Text.Length > 0)
                placeholder.gameObject.SetActive(false);
        }
        else
        {
            if (string.IsNullOrEmpty(Text))
                placeholder.gameObject.SetActive(true);
        }
    }

    private void OpenKeyboard()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (keyboardType == Keyboard.KeyboardType.Standalone)
            keyboardType = Keyboard.KeyboardType.MobileNative;
#endif
        keyboard = Provider.GetKeyboard(keyboardType);
        keyboard?.Open(this);
    }

    private void CloseKeyboard()
    {
        keyboard = null;
    }

    public void Active(bool value)
    {
        isActive = value;
        cursorWinker.SetActive(isActive);
        if (!isActive)
            CloseKeyboard();
    }

    public void SetContent(string str)
    {
        if (Text != str)
        {
            Text = str;
            onValueChanged?.Invoke(str);
        }
    }
}
