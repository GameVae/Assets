using UnityEngine;
using UI.Widget;

public class NumpadInput : MonoBehaviour
{
    public Numpad Numpad;
    public GUIInputField InputField
    {
        get; private set;
    }

    private void Awake()
    {
        Numpad.OnValueChange += OnInputByNumpad;
    }

    private void OnInputByNumpad(int value)
    {
        InputField.SetText(value.ToString());
    }

    public void OpenNumpad()
    {
        Numpad.Open();
    }

    public void CloseNumpad()
    {
        Numpad.Close();
    }

    public void SetInputField(GUIInputField inputField)
    {
        InputField = inputField;
    }
}
