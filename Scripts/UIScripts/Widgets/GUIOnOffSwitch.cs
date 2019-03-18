using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    public delegate void OnOffAction(GUIOnOffSwitch onOff);

    [RequireComponent(typeof(Button), typeof(Image))]
    public class GUIOnOffSwitch : CustomGUI
    {
        [SerializeField, HideInInspector] private Button button;

        [SerializeField, HideInInspector] protected Sprite onSprite;
        [SerializeField, HideInInspector] protected Sprite offSprite;


        private NestedCondition switchConditions;
        private OnOffAction on;
        private OnOffAction off;

        public Button Button
        {
            get { return button ?? (button = GetComponent<Button>()); }
            protected set { button = value; }
        }

        public event Func<bool> SwitchConditions
        {
            add
            {
                if (switchConditions == null)
                    switchConditions = new NestedCondition();
                switchConditions.Conditions += value;
            }
            remove
            {
                if (switchConditions != null)
                    switchConditions.Conditions -= value;
            }
        }

        public bool IsOn { get; set; }

        public Sprite OnSprite
        {
            get { return onSprite; }
            protected set { onSprite = value; }
        }
        public Sprite OffSprite
        {
            get { return offSprite; }
            protected set { offSprite = value; }
        }

        public event OnOffAction On
        {
            add { on += value; }
            remove { on -= value; }
        }
        public event OnOffAction Off
        {
            add { off += value; }
            remove { off -= value; }
        }

        protected virtual void Awake()
        {
            if (Button)
                Button.targetGraphic = BackgroundImg;
            SwitchConditions += delegate { return Interactable; };

            SetSpriteForState();
            Button.onClick.AddListener(OnOffEffect);
        }

        private void OnOffEffect()
        {
            if (switchConditions.Evaluate())
            {
                if (!IsOn)
                    SwitchOn();
                else
                    SwitchOff();
            }
        }

        private void SetSpriteForState()
        {
            On += delegate { BackgroundImg.sprite = onSprite; };
            Off += delegate { BackgroundImg.sprite = offSprite; };

            BackgroundImg.sprite = IsOn ? onSprite : offSprite;

        }

        public void SwitchOff()
        {
            if (!IsOn) return;
            off?.Invoke(this);
            IsOn = false;
        }

        public void SwitchOn()
        {
            if (IsOn) return;
            on?.Invoke(this);
            IsOn = true;
        }

        public override void InteractableChange(bool value)
        {
            interactable = value;
            Button.interactable = value;
        }

        public void OnSpriteChange(Sprite sprite)
        {
            OnSprite = sprite;
        }

        public void OffSpriteChange(Sprite sprite)
        {
            OffSprite = sprite;
        }
    }
}