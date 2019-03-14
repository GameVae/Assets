using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.Slider;

namespace UI.Widget
{
    [RequireComponent(typeof(Slider))]
    public class GUIProgressSlider : CustomGUI
    {
        [SerializeField, HideInInspector] private Slider slider;

        public SliderEvent OnValueChanged
        {
            get { return slider.onValueChanged; }
        }

        public Slider Slider
        {
            get { return slider ?? (slider = GetComponent<Slider>()); }
            protected set { slider = value; }
        }

        public float Value
        {
            get { return Slider == null ? 0 : Slider.value; }
            set
            {
                if (Slider != null)
                {
                    Slider.value = value;
                    //if (IsPlaceholder && Placeholder)
                    Placeholder.text = string.Format("{0}/{1}", Value, MaxValue);
                }
            }
        }

        public float MaxValue
        {
            get { return Slider == null ? 0 : Slider.maxValue; }
            set { if (Slider != null) Slider.maxValue = value; }
        }

        public float MinValue
        {
            get { return Slider == null ? 0 : Slider.minValue; }
            set { if (Slider != null) Slider.minValue = value; }
        }

        public override void InteractableChange(bool value)
        {
            if (Slider != null)
            {
                Slider.interactable = value;
                Interactable = value;
            }
        }

        public override void SetChildrenDependence() { }

    }
}