using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    public delegate void OnOffAction(GUIOnOffSwitch onOff);

    [RequireComponent(typeof(Button), typeof(Image))]
    public class GUIOnOffSwitch : CustomGUI
    {
        [SerializeField, HideInInspector] private Button button;

        public Button Button
        {
            get { return button ?? (button = GetComponent<Button>()); }
            protected set { button = value; }
        }

        //public Image BackgroundImage
        //{
        //    get
        //    {
        //        return backgroundImg ?? (backgroundImg = GetComponentsInChildren<Image>().
        //                                                          FirstOrDefault(img => img.gameObject.GetInstanceID() != gameObject.GetInstanceID()));
        //    }
        //    protected set { backgroundImg = value; }
        //}

        public bool IsOn { get; protected set; }

        public Func<bool> CanSwitch;
        public event OnOffAction On;
        public event OnOffAction Off;

        public Button.ButtonClickedEvent OnClick;

        protected void Awake()
        {
            if (Button)
            {
                Button.targetGraphic = BackgroundImg;
            }

            Button.onClick = OnClick;
            Button.onClick.AddListener(OnOffEffect);
        }

        protected void Start()
        {
            //if (On == null)
            //    On += delegate { BackgroundImg.color = Color.white; };
            //if (Off == null)
            //    Off += delegate { BackgroundImg.color = Color.gray; };
            CanSwitch = CanSwitch ?? delegate { return true; };
        }

        private void OnOffEffect()
        {
            if (!CanSwitch() || !Interactable) return;
            if (!IsOn)
                SwitchOn();
            else
                SwitchOff();
        }

        public void SwitchOff()
        {
            if (!IsOn)
                return;
            Off?.Invoke(this);
            IsOn = false;
        }

        public void SwitchOn()
        {
            if (IsOn) return;
            On?.Invoke(this);
            IsOn = true;
        }

        public void SetIsOn(bool value)
        {
            IsOn = value;
        }

        public override void InteractableChange(bool value)
        {
            Interactable = value;
            Button.interactable = value;
        }

        public override void SetChildrenDependence() { }

    }
}