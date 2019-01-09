#if UNITY_EDITOR
using UI.Widget;
using UnityEditor;
using UnityEngine;

namespace UI.CustomInspector
{
    [CustomEditor(typeof(GUIHorizontalInfo))]
    public class EditorHorizontalInfo : EditorGUIBase
    {
        private GUIHorizontalInfo Owner;
        private float iconRatio;
        private float desrRatio;

        protected override void OnEnable()
        {
            base.OnEnable();
            Owner = target as GUIHorizontalInfo;
            iconRatio = Owner.IconRatio;
            desrRatio = Owner.DesribeRatio;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Owner.UIDependent && !Application.isPlaying)
            {
                EditorGUILayout.BeginVertical();
                EditorGUI.BeginChangeCheck();
                iconRatio = EditorGUILayout.DelayedFloatField("Icon %", Owner.IconRatio);
                desrRatio = EditorGUILayout.DelayedFloatField("Desr %", Owner.DesribeRatio);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Btn % ");
                EditorGUILayout.LabelField(Mathf.Clamp((1 - iconRatio - desrRatio), 0, 1).ToString());
                EditorGUILayout.EndHorizontal();
                if (EditorGUI.EndChangeCheck()) Owner.RatioChange(iconRatio, desrRatio);
                EditorGUILayout.EndVertical();
            }
        }
    }
}
#endif