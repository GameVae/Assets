#if UNITY_EDITOR
using UI.Composites;
using UnityEditor;
#endif

using UnityEngine;
using System.Collections.Generic;
namespace UI.Composites
{
    public abstract class GUIComposite : MonoBehaviour
    {
        public T FindTypeWithCustomMask<T>(CustomLayerMask.CustomMask maskType)
            where T : Component
        {
            CustomLayerMask[] marks = GetComponentsInChildren<CustomLayerMask>();
            List<CustomLayerMask> sameType = new List<CustomLayerMask>();
            int length = marks.Length;

            for (int i = 0; i < length; i++)
            {
                if (marks[i].Mask == maskType)
                    sameType.Add(marks[i]);
            }

            length = sameType.Count;
            CustomLayerMask r;
            r = length > 0 ? sameType[0] : null;

            for (int i = 1; i < length; i++)
            {
                if (r.SameTypePiority < marks[i].SameTypePiority)
                    r = marks[i];
            }
            return r?.GetComponent<T>();
        }

        /// <summary>
        /// Confirm offset
        /// </summary>
        /// <returns>Determine whether something changed</returns>
        public virtual bool ConfirmOffset() { return false; }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GUIComposite), true)]
public abstract class EditorUIComposite : Editor
{
    protected virtual void OnEnable()
    {
        bool changed = (target as GUIComposite).ConfirmOffset();
        EditorUtility.SetDirty(target);
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Draw();
    }

    protected abstract void Draw();
}
#endif
