using System;
using TMPro;
using UI.Widget;
using UnityEngine;

public class Numpad : Keyboard
{
    [SerializeField] private int maxLenght;
    [SerializeField] private GUIInteractableIcon[] numbers;
    [SerializeField] private GUIInteractableIcon enterButton;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private GUIInteractableIcon backspace;

    private Action<int> onEnter;

    public event Action<int> OnEnter
    {
        add { onEnter += value; }
        remove { onEnter -= value; }
    }

    public int InputInt
    {
        get
        {
            int.TryParse(InputString, out int rs);
            return rs;
        }
        set
        {
            InputString = value.ToString();
        }
    }

    private void Awake()
    {
        InitalizeNumbers();
        backspace.OnClickEvents += OnBackspace;
        enterButton.OnClickEvents += delegate { Close(); };
    }

    private void InitalizeNumbers()
    {
        for (int i = 0; i < numbers.Length; i++)
        {
            int capture = i;
            numbers[i].OnClickEvents += delegate
            {
                OnNumber(capture);
            };
        }
    }

    private void OnNumber(int capture)
    {
        if (InputString == null || InputString.Length <= maxLenght)
        {
            InputString += capture.ToString();
            RefreshNumpadDisplay();
        }
    }

    private void OnBackspace()
    {
        if (InputString != null && InputString.Length > 0)
        {
            InputString = InputString.Remove(InputString.Length - 1);
            RefreshNumpadDisplay();
        }
    }

    protected void RefreshNumpadDisplay()
    {
        textField.text = InputString ?? "0";
    }

    protected override void HandleInput() { }

    protected override void Active(bool value, InputFieldv2 inputField)
    {
        base.Active(value, inputField);

        RefreshNumpadDisplay();
        gameObject.SetActive(value);
    }
}
