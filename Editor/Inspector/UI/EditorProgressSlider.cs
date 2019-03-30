#if UNITY_EDITOR
using UnityEditor;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;
using EnumCollect;
using static UnityEngine.UI.Selectable;
using static EditorLayoutHelper;
using System;

namespace UI.CustomInspector
{
    [CustomEditor(typeof(GUIProgressSlider), false)]
    public class EditorProgressSlider : EditorGUIBase
    {
        private GUIProgressSlider Owner;
        private Color fillColor;

        private Transition transitionType;
        private SerializedPropertyDrawer colorBlock;
        private SerializedPropertyDrawer spriteBlock;

        private float value;

        protected override void OnEnable()
        {
            base.OnEnable();
            Owner = target as GUIProgressSlider;
            fillColor = Owner.FillGrap.color;

            colorBlock = new SerializedPropertyDrawer(serializedObject, "colorBlock");
            spriteBlock = new SerializedPropertyDrawer(serializedObject, "spriteBlock");

            transitionType = Owner.Slider.transition;
            value = Owner.Value;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TransitionOpition();

            GUILayout.Space(15);
            FillColor();
            SliderValue();

            serializedObject.ApplyModifiedProperties();
        }

        private void SliderValue()
        {
            value = EditorGUILayout.Slider("Value: ", Owner.Value, Owner.MinValue, Owner.MaxValue);
            if (value != Owner.Value)
            {
                Owner.ValueChange(value);
                EditorUtility.SetDirty(Owner.Slider);
            }
        }

        private void TransitionOpition()
        {
            GUILayout.BeginVertical();
            transitionType = (Transition)EditorGUILayout.EnumPopup("Transition: ", transitionType);

            using (new EditorGUI.IndentLevelScope())
            {
                switch (transitionType)
                {
                    case Transition.ColorTint:
                        colorBlock.Draw(); break;
                    case Transition.SpriteSwap:
                        spriteBlock.Draw(); break;
                }

                if (transitionType != Owner.Slider.transition)
                {
                    Owner.TransitionChange(transitionType);
                }

                if (transitionType != Transition.None)
                    Owner.ApplyModifiedProperties();

            }
            GUILayout.EndHorizontal();
        }

        private void FillColor()
        {
            fillColor = EditorGUILayout.ColorField("Fill Color:", Owner.FillGrap.color);

            if (fillColor != Owner.FillGrap.color)
            {
                Owner.FillColorChange(fillColor);
                EditorUtility.SetDirty(Owner.FillGrap);
            }
        }
    }
}
#endif
