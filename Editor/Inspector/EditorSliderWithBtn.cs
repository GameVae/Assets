

#if UNITY_EDITOR
using UI.Widget;
using UnityEditor;
using UnityEngine;
namespace UI.CustomInspector
{
    [CustomEditor(typeof(GUISliderWithBtn))]
    public class EditorSliderWithBtn : EditorGUIBase
    {
        private GUISliderWithBtn Owner;

        private float sliderRatio;

        protected override void OnEnable()
        {
            base.OnEnable();
            Owner = (GUISliderWithBtn)target;

            sliderRatio = Owner.SliderRatio;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Owner.UIDependent && !Application.isPlaying)
                sliderRatio = EditorGUILayout.DelayedFloatField("Slider Ratio", Owner.SliderRatio);
            if (!Mathf.Approximately(sliderRatio, Owner.SliderRatio))
            {
                Owner.SliderRatioChange(sliderRatio);
            }
        }
    }
}
#endif