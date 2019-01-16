#if UNITY_EDITOR
using UI.Widget;
using UnityEditor;
using UnityEngine;
namespace UI.CustomInspector
{
    [CustomEditor(typeof(GUITextWithIcon))]
    public class EditorTextWithIcon : EditorGUIBase
    {
        private GUITextWithIcon Owner;

        private float iconRatio;

        protected override void OnEnable()
        {
            base.OnEnable();
            Owner = (GUITextWithIcon)target;

            iconRatio = Owner.IconRatio;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Owner.UIDependent && !Application.isPlaying)
                iconRatio = EditorGUILayout.DelayedFloatField("Icon Ratio", Owner.IconRatio);
            if (!Mathf.Approximately(iconRatio, Owner.IconRatio))
            {
                Owner.ImgRatioChange(iconRatio);
            }
        }
    }
}
#endif

