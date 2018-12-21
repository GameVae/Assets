using UnityEngine;
using UnityEngine.UI;

public abstract class CustomGUI : MonoBehaviour
{
    public Mask Mask { get; protected set; }

    public bool Maskable { get; protected set; }

    public virtual bool Interactable { get; protected set; }

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

    public abstract void InteractableChange(bool value);

}
