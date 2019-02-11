#if UNITY_EDITOR
using UnityEditor;
using UI.Widget;
using UnityEngine;

namespace UI.CustomInspector
{
    [CustomEditor(typeof(GUIHorizontalGrid))]
    public class EditorHorizontalGrid : EditorGUIBase
    {
        private GUIHorizontalGrid Owner;
        private float size;

        protected override void OnEnable()
        {
            base.OnEnable();
            Owner = (GUIHorizontalGrid)target;
            size = Owner.ElementSize;
        }

        public override void OnInspectorGUI()
        {
            showPlaceholder = false;
            interactable = false;
            base.OnInspectorGUI();
            if (!Owner.UIDependent && !Application.isPlaying)
            {
                size = EditorGUILayout.DelayedFloatField("Size %: ",Owner.ElementSize);
                if(!Mathf.Approximately(size,Owner.ElementSize))
                {
                    Owner.SizeChange(size);
                }
            }
        }
    }
}
#endif