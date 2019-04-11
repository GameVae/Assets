using UnityEngine;
using static InputFieldv2;

public abstract class Keyboard : MonoBehaviour
{
    public enum KeyboardType
    {
        MobileNative,
        Standalone,
        Numpad,
    }

    protected abstract void HandleInput();

    private bool isActive;
    private string inputString;
    protected ContentValidate contentValidate;

    public string InputString
    {
        get
        {
            return inputString;
        }
        set
        {
            inputString = contentValidate != null ? contentValidate.CheckContent(value) : value;
            InputField?.SetContent(inputString);
        }
    }

    public bool IsActive { get { return isActive; } }

    public InputFieldv2 InputField { get; private set; }

    private void UnAcive()
    {
        InputField?.Active(false);
        InputField = null;

        inputString = null;
        contentValidate = null;
    }

    private void Active(InputFieldv2 inputField)
    {
        InputField?.Active(false); // last input field

        InputField = inputField;
        InputField?.Active(true); // current input field

        inputString = InputField?.Text;
        contentValidate = InputField?.Validator;
    }

    protected void Update()
    {
        if (isActive)
        {
            HandleInput();
        }
    }

    protected virtual void Active(bool value, InputFieldv2 inputField)
    {
        isActive = value;

        if (isActive)
            Active(inputField);
        else
            UnAcive();
    }

    public void Close()
    {
        Active(false, null);
    }

    public void Open(InputFieldv2 inputField)
    {
        Active(true, inputField);
    }
}
