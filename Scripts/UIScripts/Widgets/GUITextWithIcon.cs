using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    public class GUITextWithIcon : CustomGUI
    {
        [SerializeField, HideInInspector] private float iconRatio;
        [SerializeField,HideInInspector] private GUIInteractableIcon icon;

        public GUIInteractableIcon Icon
        {
            get { return icon ?? (icon = GetComponentInChildren<GUIInteractableIcon>()); }
        }

        public override Image MaskImage
        {
            get { return maskImage ?? (maskImage = GetComponent<Image>()); }
            protected set { maskImage = value; }
        }

        public override TextMeshProUGUI Placeholder
        {
            get
            {
                if (placeholder == null)
                {
                    placeholder = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x != Icon.Placeholder);
                    return placeholder;
                }
                else
                {
                    return placeholder != Icon.Placeholder ? placeholder : 
                        (placeholder = GetComponentsInChildren<TextMeshProUGUI>().FirstOrDefault(x => x != Icon.Placeholder));
                }
            }
        }

        public float IconRatio
        {
            get { return iconRatio; }
            protected set { iconRatio = value; }
        }

        public override void InteractableChange(bool value) { }

        public override void SetChildrenDependence()
        {
            Icon.UIDependent = true;
            Icon.InteractableChange(false);
        }

        public void ImgRatioChange(float value)
        {
            IconRatio = value;
            if (Icon && Placeholder)
            {
                RectTransform icon = Icon.transform as RectTransform;
                RectTransform placeholder = Placeholder.transform as RectTransform;

                icon.anchorMax = new Vector2(Mathf.Clamp(IconRatio, 0, 1), 1);
                icon.offsetMin = icon.offsetMax = Vector2.zero;

                placeholder.anchorMin = new Vector2(Mathf.Clamp(IconRatio, 0, 1), 0);
                placeholder.offsetMin = icon.offsetMax = Vector2.zero;
            }
        }
    }
}