using System;
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
        [SerializeField, HideInInspector] private Graphic fillGrap;

        [SerializeField, HideInInspector] private ColorBlock colorBlock;
        [SerializeField, HideInInspector] private SpriteState spriteBlock;

        public SliderEvent OnValueChanged
        {
            get { return slider.onValueChanged; }
        }

        public Graphic FillGrap
        {
            get { return fillGrap ?? (fillGrap = Slider?.fillRect.GetComponent<Graphic>()); }
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

        

        public void SetDefaultPlaceholder()
        {
            Placeholder.text = string.Format("{0}/{1}", Value, MaxValue);
        }

#if UNITY_EDITOR
        public void TransitionChange(Selectable.Transition transitionType)
        {
            Slider.transition = transitionType;
        }

        public void FillColorChange(Color color)
        {
            fillGrap.color = color;
        }

        public void ApplyModifiedProperties()
        {
            Slider.colors = colorBlock;
            Slider.spriteState = spriteBlock;
        }

        public void ValueChange(float value)
        {
            Value = value;
        }
#endif
    }
}