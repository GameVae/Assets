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

        public GUIOnOffSwitch OnOffSwitch
        {
            get { return onOffSwitch ?? (onOffSwitch = GetComponent<GUIOnOffSwitch>()); }
            protected set { onOffSwitch = value; }
        }

        public event UnityAction OnClickEvents;

        protected void Awake()
        {
            OnOffSwitch.Off += delegate { };
        }

        protected void Start()
        {
            if (OnClickEvents != null)
                Button.onClick.AddListener(OnClickEvents);
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