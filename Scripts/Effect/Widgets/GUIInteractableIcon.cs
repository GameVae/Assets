using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


[RequireComponent(typeof(Button))]
public class GUIInteractableIcon : CustomGUI
{
    public Button Button { get; private set; }
    public Image MaskImage { get; private set; }
    public Image BackgroundImage { get; private set; }
    public TextMeshProUGUI Placeholder { get; private set; }

    public event UnityAction OnClickEvents;

    private void Awake()
    {
        Button = GetComponent<Button>();
        MaskImage = Button?.GetComponent<Image>();
        BackgroundImage = GetComponentInChildren<Image>();
        Placeholder = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        Button.onClick.AddListener(OnClickEvents);
    }

    public override void InteractableChange(bool value)
    {
        if (Button == null)
            Button = GetComponent<Button>();

        Interactable = value;
        if (value)
            Button.transition = Selectable.Transition.ColorTint;
        else
            Button.transition = Selectable.Transition.None;
        Button.interactable = value;
    }

    public void PlaceholderText(string text)
    {
        if (Placeholder == null)
            Placeholder = GetComponentInChildren<TextMeshProUGUI>();
        if (Placeholder != null)
            Placeholder.text = text;
    }
}

