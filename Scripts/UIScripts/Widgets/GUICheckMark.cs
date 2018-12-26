using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    public class GUICheckMark : CustomGUI
    {
        [SerializeField, HideInInspector] Image maskImage;

        public Image MaskImage
        {
            get { return maskImage ?? (maskImage = GetComponent<Image>()); }
            protected set { maskImage = value; }
        }
        public GUIOnOffSwitch OnOffSwitch;

        public Sprite OnSprite;
        public Sprite OffSprite;

        public override bool Interactable
        {
            get { return interactable; }

            protected set
            {
                if (value) Enable();
                else Disable();
                interactable = value;
            }
        }

        protected override void Awake()
        {
            MaskImage = GetComponent<Image>();
            Placeholder = GetComponentInChildren<TextMeshProUGUI>();

            OnOffSwitch.CanSwitch = delegate { return Interactable; };
            OnOffSwitch.On += delegate
            {
                OnOffSwitch.Image.sprite = OnSprite;
                MaskImage.color = Color.cyan;
            };
            OnOffSwitch.Off += delegate
            {
                OnOffSwitch.Image.sprite = OffSprite;
                MaskImage.color = Color.white;
            };

            OnOffSwitch.InteractableChange(Interactable);
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
        }

        public override void InteractableChange(bool value)
        {
            Interactable = value;
            OnOffSwitch.InteractableChange(value);
        }

        public void SetGroup(GUIToggle group)
        {
            OnOffSwitch.On += delegate
            {
                if (group.ActiveMark != this)
                {
                    group.ActiveMark?.OnOffSwitch.SwitchOff();
                    group.ActiveMark = this;
                    group.CheckActionCallback();
                }
            };
        }

        private void Disable()
        {
            if (MaskImage != null)
            {
                MaskImage.color = Color.gray;
                OnOffSwitch.InteractableChange(true);
            }
        }

        private void Enable()
        {
            if (MaskImage != null)
            {
                MaskImage.color = Color.white;
                OnOffSwitch.InteractableChange(false);
            }
        }
    }
}