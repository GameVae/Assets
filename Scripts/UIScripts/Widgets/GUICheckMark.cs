using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    public class GUICheckMark : GUIOnOffSwitch
    {
        private GUIToggle group;

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

        public override bool IsBackground
        {
            get { return false; }
            protected set { base.IsBackground = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            SwitchConditions += delegate 
            {
                return (group == null || group.ActiveMark != this);
            };
            //OnOffSwitch.On += delegate
            //{
            //    BackgroundImg.sprite = onSprite;
            //    MaskImage.color = Color.cyan;
            //};
            //OnOffSwitch.Off += delegate
            //{
            //    BackgroundImg.sprite = offSprite;
            //    MaskImage.color = Color.white;
            //};
        }
     

        public void SetGroup(GUIToggle agroup)
        {
            group = agroup;
            On += delegate
            {
                if (agroup.ActiveMark != this)
                {
                    agroup.ActiveMark?.SwitchOff();
                    agroup.ActiveMark = this;
                }
            };
        }

        private void Disable()
        {
            if (MaskImage != null)
            {
                MaskImage.color = Color.gray;
                InteractableChange(true);
            }
        }

        private void Enable()
        {
            if (MaskImage != null)
            {
                MaskImage.color = Color.white;
                InteractableChange(false);
            }
        }

    }
}