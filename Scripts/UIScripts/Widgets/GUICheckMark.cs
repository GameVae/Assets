using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    public class GUICheckMark : CustomGUI
    {
        private GUIToggle group;
        private Image backgroudImg;
        private GUIOnOffSwitch onOffSwitch;

        [Header("Check Animation")]
        public Sprite OnSprite;
        public Sprite OffSprite;

        public GUIOnOffSwitch OnOffSwitch
        {
            get { return onOffSwitch ?? (onOffSwitch = GetComponentInChildren<GUIOnOffSwitch>()); }
            private set { onOffSwitch = value; }
        }

        public override Image MaskImage
        {
            get { return maskImage ?? (maskImage = GetComponent<Image>()); }
            protected set { maskImage = value; }
        }

        public Image BackgroundImage
        {
            get
            {
                return backgroudImg ?? (backgroudImg = GetComponentsInChildren<Image>().
                                                                  FirstOrDefault(img => img.gameObject.GetInstanceID() != gameObject.GetInstanceID()));
            }
        }

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
            OnOffSwitch.CanSwitch = delegate 
            {
                return Interactable && (group == null || group.ActiveMark != this);
            };
            OnOffSwitch.On += delegate
            {
                BackgroundImage.sprite = OnSprite;
                MaskImage.color = Color.cyan;
            };
            OnOffSwitch.Off += delegate
            {
                BackgroundImage.sprite = OffSprite;
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

        public void SetGroup(GUIToggle agroup)
        {
            group = agroup;
            OnOffSwitch.On += delegate
            {
                if (agroup.ActiveMark != this)
                {
                    agroup.ActiveMark?.OnOffSwitch.SwitchOff();
                    agroup.ActiveMark = this;
                }
            };
        }

        public override void SetChildrenDependence()
        {
            OnOffSwitch.UIDependent = true;
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