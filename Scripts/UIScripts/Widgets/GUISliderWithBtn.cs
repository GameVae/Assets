
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    public delegate void SliderInteracted(GUIProgressSlider slider);

    [RequireComponent(typeof(Image))]
    public class GUISliderWithBtn : CustomGUI
    {
        [SerializeField, HideInInspector] private float sliderRatio;
        [SerializeField, HideInInspector] private GUIProgressSlider slider;
        [SerializeField, HideInInspector] private GUIInteractableIcon button;

        public float SliderRatio
        {
            get { return sliderRatio; }
            protected set { sliderRatio = value; }
        }

        public event SliderInteracted OnClickEvents;

        public GUIProgressSlider Slider
        {
            get { return slider ?? (slider = GetComponentInChildren<GUIProgressSlider>()); }
        }

        public GUIInteractableIcon Button
        {
            get { return button ?? (button = GetComponentInChildren<GUIInteractableIcon>()); }
        }

        public override TextMeshProUGUI Placeholder
        {
            get { return placeholder ?? (placeholder = Slider.Placeholder); }
            set { placeholder = value; }
        }

        protected void Start()
        {
            if (Button)
                Button.OnClickEvents += delegate { OnClickEvents?.Invoke(slider); };
        }

        public override void InteractableChange(bool value)
        {
            Interactable = value;
            Button?.InteractableChange(value);
        }

        public override void SetChildrenDependence()
        {
            Slider.UIDependent = false;
            Button.UIDependent = false;
        }

        public void SliderRatioChange(float value)
        {
            SliderRatio = value;
            if (Slider && Button)
            {
                RectTransform slider = Slider.transform as RectTransform;
                RectTransform btn = Button.transform as RectTransform;

                slider.anchorMax = new Vector2(Mathf.Clamp(SliderRatio, 0, 1), 1);
                slider.offsetMin = slider.offsetMax = Vector2.zero;

                btn.anchorMin = new Vector2(Mathf.Clamp(SliderRatio, 0, 1), 0);
                btn.offsetMin = slider.offsetMax = Vector2.zero;
            }
        }
    }
}