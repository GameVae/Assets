using Animation;
using Generic.Singleton;
using TMPro;
using UI.Composites;
using UnityEngine;
using UnityEngine.Events;
using UI.Keyboard;

[RequireComponent(typeof(Winker), typeof(SelectableComp))]
public partial class CustomInputField : MonoBehaviour
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
    private KeyboardFactory keyboardFactory;
    private Keyboard keyboard;

    [Space, Header("Content validate type")]
    [SerializeField] private ContentValidate.ContentType contentType;
    private ContentValidateFactory validateFactory;
    private ContentValidate validator;

    // Auto-Properties
    // CONTENT VALIDATOR
    public ContentValidateFactory ValidateFactory
    {
        get { return validateFactory ?? (validateFactory = Singleton.Instance<ContentValidateFactory>()); }
    }
    public ContentValidate Validator
    {
        get
        {
            return validator ?? (validator = ValidateFactory.GetValidator(contentType));
        }
    }
    /////////////////////////////
    // KEYBOARD
    protected KeyboardFactory KeyboardFactory
    {
        get
        {
            return keyboardFactory ?? (keyboardFactory = Singleton.Instance<KeyboardFactory>());
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

    public void OpenKeyboard()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (keyboardType == Keyboard.KeyboardType.Standalone)
            keyboardType = Keyboard.KeyboardType.MobileNative;
#endif
        keyboard = KeyboardFactory.GetKeyboard(keyboardType);
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
            Keyboard?.SetInputString(str);
            onValueChanged?.Invoke(str);
        }
    }
}
