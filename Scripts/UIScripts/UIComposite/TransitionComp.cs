#if UNITY_EDITOR
using UnityEditor;
using UI.Composites;
#endif

using UnityEngine;
using static UnityEngine.UI.Selectable;
using UnityEngine.UI;

namespace UI.Composites
{
    [RequireComponent(typeof(Button))]
    public abstract class TransitionComp : GUIComposite
    {
        [SerializeField] protected SelectableTransitionObject transitionObject;
        public abstract Transition Transition { get; set; }
        public abstract Object TargetDirty { get; }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TransitionComp), true)]
public class EditorTransitionComp : EditorComposite
{
    private TransitionComp owner;
    private Transition transition;

    protected override void OnEnable()
    {
        base.OnEnable();
        owner = target as TransitionComp;
        transition = owner.Transition;
    }

    protected override void Draw()
    {
        transition = (Transition)EditorGUILayout.EnumPopup("Transition: ", transition);
        if (transition != owner.Transition)
        {
            owner.Transition = transition;
            EditorUtility.SetDirty(owner.TargetDirty);
        }
    }
}
#endif
