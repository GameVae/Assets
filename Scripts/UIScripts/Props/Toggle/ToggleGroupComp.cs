#if UNITY_EDITOR
using UnityEditor;
using UI.Composites;
#endif

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Composites
{
    public class ToggleGroupComp : GUIComposite
    {
        [System.Serializable]
        public class ToggleGroupEvent : UnityEvent<ToggleComp> { }

        [SerializeField] private List<ToggleComp> toggles;
        [SerializeField] private ToggleGroupEvent toggleSelectedEvt;

        public ToggleSpriteObject DefaultSpriteState;

        public event UnityAction<ToggleComp> ToggleSelectedEvt
        {
            add { toggleSelectedEvt.AddListener(value); }
            remove { toggleSelectedEvt.RemoveListener(value); }
        }

        public List<ToggleComp> Toggles
        {
            get { return toggles ?? (toggles = new List<ToggleComp>()); }
        }

        public int ActiveToggleIndex
        { get; private set; }

        private void Awake()
        {
            SetupEvent();
            Active(0);
        }

        public void Active(int index)
        {
            if (index >= 0 && index < Toggles.Count)
            {
                OnToggleSelected(Toggles[index]);
            }
        }

        public override void Refresh()
        {
            base.Refresh();
            Toggles.Clear();
            ToggleComp[] comps = GetComponentsInChildren<ToggleComp>();
            Toggles.AddRange(comps);
            Toggles.ForEach(toggle => RefreshToggleElement(toggle));
        }

        public void SetDefaultSpriteStateAll()
        {
            Toggles.ForEach(toggle => ForceChangeSpriteState(toggle));
        }

        private void RefreshToggleElement(ToggleComp toggle)
        {
            toggle.Refresh();
            toggle.SetGroup(GetInstanceID());
        }
        private void ForceChangeSpriteState(ToggleComp toggle)
        {
            toggle.ToggleSpriteState = DefaultSpriteState;
            RefreshToggleElement(toggle);
        }

        private void OnToggleSelected(ToggleComp toggle)
        {
            DefaultToggleSelect(toggle);
            toggleSelectedEvt?.Invoke(toggle);
        }

        private void SetupEvent()
        {
            int count = Toggles.Count;
            for (int i = 0; i < count; i++)
            {
                int capture = i;
                Toggles[capture].OnClickEvents += delegate { OnToggleSelected(Toggles[capture]); };
            }
        }

        private void DefaultToggleSelect(ToggleComp toggle)
        {
            ActiveToggleIndex = Toggles.IndexOf(toggle);
            //if(!toggle.IsActive)
            {
                toggle.Active(true);
            }

            int count = Toggles.Count;
            for (int i = 0; i < count; i++)
            {
                if(i != ActiveToggleIndex)
                {
                    if (Toggles[i].IsActive) Toggles[i].Active(false);
                }
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(ToggleGroupComp))]
public class EditorToggleGroup : EditorComposite
{
    protected override void Draw()
    {
        if (GUILayout.Button("Set Default Sprite State All"))
        {
            (target as ToggleGroupComp).SetDefaultSpriteStateAll();
            SetDirtyToggles();
        }
    }

    private void SetDirtyToggles()
    {
        List<ToggleComp> toggles = (target as ToggleGroupComp).Toggles;
        for (int i = 0; i < toggles.Count; i++)
        {
            EditorUtility.SetDirty(toggles[i]);
        }
    }
}
#endif