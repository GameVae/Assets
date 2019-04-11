using UnityEngine;
using UnityEngine.Events;

namespace UI.Composites
{
    [RequireComponent(typeof(SelectableComp))]
    public class ToggleComp : GUIComposite
    {
        [SerializeField, HideInInspector] private SelectableComp selectableComp;
        [SerializeField, HideInInspector] private int groupInsID = -1;
        [SerializeField] private bool isActive;

        public ToggleSpriteObject ToggleSpriteState;

        protected SelectableComp SelectableComp
        {
            get
            {
                return selectableComp ?? (selectableComp = GetComponent<SelectableComp>());
            }
        }

        protected BackgroundComp BackgroundComp
        {
            get { return SelectableComp?.Background; }
        }

        public event UnityAction OnClickEvents
        {
            add { SelectableComp.OnClickEvents += value; }
            remove { SelectableComp.OnClickEvents -= value; }
        }

        public bool IsActive
        {
            get { return isActive; }
        }

        private void Awake()
        {
            OnClickEvents += OnSelected;
        }

        private void OnSelected()
        {
            if (groupInsID == -1)
                Active(!isActive);
        }

        public void Active(bool active)
        {
            isActive = active;
            if (isActive)
                Active();
            else UnActive();
        }

        private void Active()
        {
            BackgroundComp.Sprite = ToggleSpriteState.ActiveSprite;
        }

        private void UnActive()
        {
            BackgroundComp.Sprite = ToggleSpriteState.UnActiveSprite;
        }

        public void SetGroup(int id)
        {
            groupInsID = id;
        }

        public override void Refresh()
        {
            groupInsID = -1;
            Active(isActive);
        }

#if UNITY_EDITOR
        public override bool ConfirmOffset()
        {
            BackgroundComp.UnActiveModify = true;
            return base.ConfirmOffset();
        }

        private void OnValidate()
        {
            Active(isActive);
        }
#endif
    }
}