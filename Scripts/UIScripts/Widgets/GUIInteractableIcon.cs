using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Widget
{
    [RequireComponent(typeof(Button), typeof(Image), typeof(GUIOnOffSwitch))]
    public class GUIInteractableIcon : CustomGUI
    {
        [SerializeField, HideInInspector] private GUIOnOffSwitch onOffSwitch;

        public Button Button
        {
            get { return OnOffSwitch.Button; }
        }

        public override Image MaskImage
        {
            get { return OnOffSwitch.MaskImage; }
            protected set { return; }
        }

        public Image BackgroundImage
        {
            get { return OnOffSwitch.BackgroundImage; }
        }

        public GUIOnOffSwitch OnOffSwitch
        {
            get { return onOffSwitch ?? (onOffSwitch = GetComponent<GUIOnOffSwitch>()); }
            protected set { onOffSwitch = value; }
        }

        public event UnityAction OnClickEvents;

        protected override void Awake()
        {
            OnOffSwitch.Off += delegate { };
            base.Awake();
        }

        protected override void Start()
        {
            if (OnClickEvents != null)
                Button.onClick.AddListener(OnClickEvents);
            base.Awake();
        }

        public override void InteractableChange(bool value)
        {
            if (Button != null)
            {
                Interactable = value;
                //if (value)
                //    Button.transition = Selectable.Transition.ColorTint;
                //else
                //    Button.transition = Selectable.Transition.None;
                Button.interactable = value;
                OnOffSwitch.InteractableChange(value);
            }
        }

        public override void SetChildrenDependence()
        {
            OnOffSwitch.UIDependent = true;
        }

        public void ForceChangeEvent(UnityAction events)
        {
            OnClickEvents = events;
        }
    }

}