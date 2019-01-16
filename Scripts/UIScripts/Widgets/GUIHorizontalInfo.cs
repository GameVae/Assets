using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    public class GUIHorizontalInfo : CustomGUI
    {
        [SerializeField, HideInInspector] private float iconRatio;
        [SerializeField, HideInInspector] private float desribeRatio;
        [SerializeField, HideInInspector] private GUIInteractableIcon icon;
        [SerializeField, HideInInspector] private GUIInteractableIcon button;

        public GUIInteractableIcon Icon
        {
            get { return icon ?? (icon = transform.GetChild(1).GetComponent<GUIInteractableIcon>()); }
            protected set { icon = value; }
        }

        public GUIInteractableIcon Button
        {
            get { return button ?? (icon = transform.GetChild(2).GetComponent<GUIInteractableIcon>()); }
            protected set { button = value; }
        }

        public override TextMeshProUGUI Placeholder
        {
            get
            {
                return placeholder ?? (placeholder = transform.GetChild(0).GetComponent<TextMeshProUGUI>());
            }
            set { placeholder = value; }
        }

        public override Image MaskImage
        {
            get { return maskImage ?? (maskImage = GetComponent<Image>()); }
            protected set { maskImage = value; }
        }

        public float IconRatio
        {
            get { return iconRatio; }
            protected set { iconRatio = value; }
        }

        public float DesribeRatio
        {
            get { return desribeRatio; }
            protected set { desribeRatio = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            Icon.InteractableChange(false);
            Button.InteractableChange(true);
        }
        public override void InteractableChange(bool value)
        {
            Interactable = value;
            Icon.InteractableChange(value);
            Button.gameObject.SetActive(value);
        }

        public override void SetChildrenDependence()
        {
            Button.UIDependent = false;
            Icon.UIDependent = false;
        }

        public void RatioChange(float icon, float desribeField)
        {
            IconRatio = icon;
            DesribeRatio = desribeField;

            RectTransform iconRect = Icon.transform as RectTransform;
            RectTransform desrRect = Placeholder.transform as RectTransform;
            RectTransform btnRect = Button.transform as RectTransform;

            iconRect.anchorMax = new Vector2(Mathf.Clamp(icon, 0, 1), 1);
            desrRect.anchorMin = new Vector2(Mathf.Clamp(icon + 0.05f, 0, 1), 0);// text alignment
            desrRect.anchorMax = new Vector2(Mathf.Clamp(icon + desribeField, 0, 1), 1);
            btnRect.anchorMin = new Vector2(Mathf.Clamp(icon + desribeField, 0, 1), 0); 

            iconRect.offsetMin = iconRect.offsetMax = Vector2.zero;
            desrRect.offsetMin = desrRect.offsetMax = Vector2.zero;
            btnRect.offsetMin = btnRect.offsetMax = Vector2.zero;
        }

    }
}