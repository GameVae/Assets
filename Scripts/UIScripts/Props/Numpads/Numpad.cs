using System;
using UI.Widget;
using UnityEngine;

public class Numpad : MonoBehaviour
{
    [SerializeField] private int maxLenght;
    [SerializeField] private GUIInteractableIcon[] numbers;
    [SerializeField] private GUIInteractableIcon enterButton;
    [SerializeField] private GUIInteractableIcon textField;
    [SerializeField] private GUIInteractableIcon backspace;

    private Action<int> onValueChange;
    private Action<int> onEnter;

    public event Action<int> OnEnter
    {
        add { onEnter += value; }
        remove { onEnter -= value; }
    }

    public event Action<int> OnValueChange
    {
        add { onValueChange += value; }
        remove { onValueChange -= value; }
    }

    public string InputString
    {
        get { return textField.Placeholder.text; }
        private set { textField.Placeholder.text = value; }

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
        Open();
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
        if (InputString.Length <= maxLenght)
        {
            InputString += capture.ToString();
            textField.Placeholder.text = InputString;

            onValueChange?.Invoke(InputInt);
        }
    }

    private void OnBackspace()
    {
        if(InputString.Length > 0)
        {
            InputString = InputString.Remove(InputString.Length - 1);
            onValueChange?.Invoke(InputInt);
        }
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        Refresh();
        gameObject.SetActive(true);
    }

    public void Refresh()
    {
        InputString = "";
    }
}
