#if UNITY_EDITOR
using UI.Widget;
using UnityEditor;
using UnityEngine;

namespace UI.CustomInspector
{
    [CustomEditor(typeof(GUITableElement))]
    public class EditorTableElement : EditorGUIBase
    {
        private GUITableElement Owner;
        private GUIToggle.AxisType axisType;
        protected override void OnEnable()
        {
            base.OnEnable();
            Owner = (GUITableElement)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Owner.UIDependent && !Application.isPlaying)
            {
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUILayout.MaxWidth(50)))
                {
                    Owner.Add("new text");
                }
                if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
                {
                    Owner.EditorDeleteLastIndex();
                }
                axisType = (GUIToggle.AxisType)EditorGUILayout.EnumPopup(Owner.AxisType, GUILayout.MaxWidth(100));
                if(axisType != Owner.AxisType)
                {
                    Owner.AxisTypeChange(axisType);
                }
                GUILayout.EndHorizontal();
            }
        }

    }


}
#endif