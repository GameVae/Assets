#if UNITY_EDITOR
using UI.Composites;
using UnityEditor;
#endif  

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Composites
{
    [RequireComponent(typeof(Button), typeof(BackgroundComp))]
    public class SelectableComp : GUIComposite
    {
        private Button button;
        private BackgroundComp background;
        [SerializeField, HideInInspector] private Button.ButtonClickedEvent onClick;

        public event UnityAction OnClickEvents
        {
            add { onClick.AddListener(value); }
            remove { onClick.RemoveListener(value); }
        }

        public Button Button
        {
            get { return button ?? (button = GetComponent<Button>()); }
            protected set { button = value; }
        }

        public bool Interactable
        {
            get
            {
                if (Button) return Button.interactable;
                else return false;
            }
            set
            {
                if (Button) Button.interactable = value;
            }
        }

        public BackgroundComp Background
        {
            get { return background ?? (background = GetComponent<BackgroundComp>()); }
        }

        private void Awake()
        {
            Button.onClick = onClick;
            Button.targetGraphic = Background.BackgroundImg;
        }

        public override bool ConfirmOffset()
        {
            if (Button != null && Background != null
                && Button.targetGraphic != Background.BackgroundImg)
            {
                Button.targetGraphic = Background.BackgroundImg;
                return true;
            }
            return base.ConfirmOffset();
        }

        public override void Refresh()
        {
            base.Refresh();
            FindComponent();
        }

        public void RemoveAllListener()
        {
            onClick.RemoveAllListeners();
        }

        private void FindComponent()
        {
            button = GetComponent<Button>();
            background = GetComponent<BackgroundComp>();
        }
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(SelectableComp))]
public class EditorSelectable : EditorComposite
{
    private SelectableComp owner;
    private SerializedProperty onClickEvents;
    private bool interactable;

    protected override void OnEnable()
    {
        base.OnEnable();
        owner = target as SelectableComp;

        onClickEvents = serializedObject.FindProperty("onClick");
        interactable = owner.Interactable;

    }
    protected override void Draw()
    {
        interactable = EditorGUILayout.Toggle("Interactable", owner.Interactable);
        if (interactable != owner.Interactable)
        {
            owner.Interactable = interactable;
            EditorUtility.SetDirty(owner);
        }

        if (owner.Interactable)
        {
            EditorGUILayout.PropertyField(onClickEvents);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif