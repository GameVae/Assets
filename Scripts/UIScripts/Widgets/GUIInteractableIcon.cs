using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Widget
{
    [RequireComponent(typeof(Button))]
    public class GUIInteractableIcon : CustomGUI
    {
        [SerializeField, HideInInspector] Button button;
        [SerializeField, HideInInspector] Image maskImage;
        [SerializeField, HideInInspector] Image backgroundImage;
        [SerializeField, HideInInspector] GUIOnOffSwitch onOffSwitch;

        public Button Button
        {
            get { return button ?? GetComponent<Button>(); }
            protected set { button = value; }
        }
        public Image MaskImage
        {
            get { return maskImage ?? (maskImage = Button?.GetComponent<Image>()); }
            protected set { maskImage = value; }
        }
        public Image BackgroundImage
        {
            get { return backgroundImage ?? (backgroundImage = Button?.GetComponentInChildren<Image>()); }
            protected set { backgroundImage = value; }
        }

        public GUIOnOffSwitch OnOffSwitch
        {
            get { return onOffSwitch ?? (onOffSwitch = GetComponent<GUIOnOffSwitch>()); }
            protected set { onOffSwitch = value; }
        }
        public event UnityAction OnClickEvents;

        protected override void Awake()
        {
            Button = GetComponent<Button>();
            MaskImage = Button?.GetComponent<Image>();
            BackgroundImage = transform.GetChild(0).GetComponent<Image>();
            Placeholder = GetComponentInChildren<TextMeshProUGUI>();

            InteractableChange(Interactable);
            OnOffSwitch = GetComponent<GUIOnOffSwitch>();
            Button.targetGraphic = backgroundImage;
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
                if (value)
                    Button.transition = Selectable.Transition.ColorTint;
                else
                    Button.transition = Selectable.Transition.None;
                Button.interactable = value;
                OnOffSwitch.InteractableChange(value);
            }
        }
    }

}