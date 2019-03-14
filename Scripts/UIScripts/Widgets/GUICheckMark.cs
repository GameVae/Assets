using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    public class GUICheckMark : CustomGUI
    {
        private GUIToggle group;
        private GUIOnOffSwitch onOffSwitch;

        [Header("Check Animation")]
        public Sprite OnSprite;
        public Sprite OffSprite;

        public GUIOnOffSwitch OnOffSwitch
        {
            get { return onOffSwitch ?? (onOffSwitch = GetComponentInChildren<GUIOnOffSwitch>()); }
            private set { onOffSwitch = value; }
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

        protected void Awake()
        {
            OnOffSwitch.CanSwitch = delegate 
            {
                return Interactable && (group == null || group.ActiveMark != this);
            };
            OnOffSwitch.On += delegate
            {
                BackgroundImg.sprite = OnSprite;
                MaskImage.color = Color.cyan;
            };
            OnOffSwitch.Off += delegate
            {
                BackgroundImg.sprite = OffSprite;
                MaskImage.color = Color.white;
            };

            OnOffSwitch.InteractableChange(Interactable);
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