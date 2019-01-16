using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    [RequireComponent(typeof(Image))]
    public abstract class CustomGUI : MonoBehaviour
    {
        [SerializeField, HideInInspector] protected TextMeshProUGUI placeholder;
        [SerializeField, HideInInspector] protected Mask mask;
        [SerializeField, HideInInspector] protected Image maskImage;
        [SerializeField, HideInInspector] protected bool maskable;
        [SerializeField, HideInInspector] protected bool interactable;
        [SerializeField, HideInInspector] protected bool isPlaceholder;

        [SerializeField, HideInInspector] protected bool isUIDependent;

        public float FontSize
        {
            get { return  Placeholder ? Placeholder.fontSize : 0; }
            protected set { if(Placeholder) Placeholder.fontSize = value; }
        }

        public Color PlaceholderColor
        {
            get
            {
                if (Placeholder) return Placeholder.color;
                return default(Color);
            }
            protected set { if (Placeholder) Placeholder.color = value; }
        }

        public bool UIDependent
        {
            get { return isUIDependent; }
            set { isUIDependent = value; }
        }

        public Mask Mask
        {
            get { return mask ?? GetComponent<Mask>(); }
            protected set { mask = value; }
        }

        public virtual TextMeshProUGUI Placeholder
        {
            get { return placeholder ?? (placeholder = GetComponentInChildren<TextMeshProUGUI>()); }
            set { placeholder = value; }
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

        public void FontSizeChange(float fontSize)
        {
            FontSize = fontSize;
        }

        public void IsPlaceholderChange(bool value)
        {
            IsPlaceholder = value;
            if (Placeholder != null)
                Placeholder.enabled = value;
        }

        public abstract void InteractableChange(bool value);

        public abstract Image MaskImage { get; protected set; }

        public abstract void SetChildrenDependence();

        public void PlaceholderColorChange(Color newColor)
        {
            PlaceholderColor = newColor;
        }

        protected virtual void Awake() { }

        protected virtual void Start() { }
    }
}
