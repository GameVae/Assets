using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class CustomGUI : MonoBehaviour
{
    [SerializeField, HideInInspector] protected TextMeshProUGUI placeholder;
    [SerializeField, HideInInspector] protected Mask mask;
    [SerializeField, HideInInspector] protected bool maskable;
    [SerializeField, HideInInspector] protected bool interactable;
    [SerializeField, HideInInspector] protected bool isPlaceholder;


    public Mask Mask
    {
        get { return mask ?? GetComponent<Mask>(); }
        protected set { mask = value; }
    }

    public TextMeshProUGUI Placeholder
    {
        get { return placeholder ?? GetComponentInChildren<TextMeshProUGUI>(); }
        protected set { placeholder = value; }
    }

    public bool Maskable
    {
        get { return maskable; }
        protected set { maskable = value; }
    }

    public bool IsPlaceholder
    {
        get { return isPlaceholder; }
        set { isPlaceholder = value; }
    }

    public virtual bool Interactable
    {
        get { return interactable; }
        protected set { interactable = value; }
    }

    public void PlaceholderText(string text)
    {
        if (Placeholder != null)
            Placeholder.text = text;
    }

    public void MaskableChange(bool value)
    {
        Maskable = value;
        if (Maskable)
        {
            Mask = GetComponent<Mask>();
            if (Mask == null)
                Mask = gameObject.AddComponent<Mask>();
            else Mask.enabled = true;
        }
        else
        {
            Mask = GetComponent<Mask>();
            if (Mask != null)
                Mask.enabled = false;
        }
    }

    public void IsPlaceholderChange(bool value)
    {
        IsPlaceholder = value;
        if (Placeholder != null)
            Placeholder.enabled = value;
    }

    public abstract void InteractableChange(bool value);

}
