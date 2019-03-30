using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Widget
{
    [RequireComponent(typeof(Button))]
    public class GUIInteractableIcon : CustomGUI
    {
        [SerializeField, HideInInspector]
        private Button button;
        [SerializeField, HideInInspector]
        private Button.ButtonClickedEvent onClick;

        [SerializeField, HideInInspector] private ColorBlock colorBlock;
        [SerializeField, HideInInspector] private SpriteState spriteBlock;

        public Button Button
        {
            get { return button ?? (button = GetComponent<Button>()); }
            protected set { button = value; }
        }

        public event UnityAction OnClickEvents
        {
            add
            {
                onClick.AddListener(value);
            }
            remove
            {
                onClick.RemoveListener(value);
            }
        }

        private void Awake()
        {
            Button.onClick = onClick;
            Button.targetGraphic = BackgroundImg;
        }

        public override void InteractableChange(bool value)
        {
            if (Button != null)
            {
                Interactable = value;
                Button.interactable = value;
            }
        }

#if UNITY_EDITOR
        public void TransitionChange(Selectable.Transition transitionType)
        {
            Button.transition = transitionType;
        }

        public void ApplyModifiedProperties()
        {
            Button.colors = colorBlock;
            Button.spriteState = spriteBlock;
        }
#endif
    }

}