using UnityEngine;
using static UnityEngine.UI.Selectable;
using UnityEngine.UI;

namespace UI.Composites
{
    [RequireComponent(typeof(Button))]
    public class ButtonTransitionComp : TransitionComp
    {
        [SerializeField, HideInInspector] private Button button;

        public Button Button
        {
            get { return button ?? (button = GetComponent<Button>()); }
        }

        public override Transition Transition
        {
            get { return Button.transition; }
            set
            {
                Button.transition = value;
            }
        }

        public override Object TargetDirty
        {
            get { return Button; }
        }

        public override bool ConfirmOffset()
        {
            bool isChanged = false;
            if (transitionObject != null)
            {
                if (Button.colors != transitionObject.Colors)
                {
                    Button.colors = transitionObject.Colors;
                    isChanged = true;
                }

                if (!Button.spriteState.Equal(transitionObject.SpriteState))
                {
                    Button.spriteState = transitionObject.SpriteState;
                    isChanged = true;
                }
            }
            return isChanged;
        }

        public override void Refresh()
        {
            base.Refresh();
            FindButton();
        }

        private void FindButton()
        {
            button = GetComponent<Button>();
        }
    }
}
