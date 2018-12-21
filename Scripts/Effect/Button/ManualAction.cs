using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;

[RequireComponent(typeof(Button))]
public class ManualAction : MonoBehaviour
{
    private Button button;

    public event UnityAction ClickAction;
    public ButtonClickedEvent OnClick;

    public bool IsBlocked
    {
        get { return button.interactable; }
    }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        button.onClick = OnClick;
        if (ClickAction != null)
            button.onClick.AddListener(ClickAction);
    }

    public void Block(bool value)
    {
        button.interactable = value;
    }
}
