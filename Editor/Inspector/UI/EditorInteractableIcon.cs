#if UNITY_EDITOR
using UnityEditor;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using static EditorLayoutHelper;
using static UnityEngine.UI.Selectable;

namespace UI.CustomInspector
{
    [CustomEditor(typeof(GUIInteractableIcon))]
    public class EditorInteractableIcon : EditorGUIBase
    {
        private GUIInteractableIcon Owner;

        private SerializedPropertyDrawer onClickEvents;

        private Transition transitionType;
        private SerializedPropertyDrawer colorBlock;
        private SerializedPropertyDrawer spriteBlock;

        protected override void OnEnable()
        {
            base.OnEnable();
            Owner = target as GUIInteractableIcon;

            onClickEvents = new SerializedPropertyDrawer(serializedObject, "onClick");

            transitionType = Owner.Button.transition;
            colorBlock = new SerializedPropertyDrawer(serializedObject, "colorBlock");
            spriteBlock = new SerializedPropertyDrawer(serializedObject, "spriteBlock");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TransitionOpition();

            onClickEvents.Draw();

            serializedObject.ApplyModifiedProperties();

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

                if (transitionType != Owner.Button.transition)
                    Owner.TransitionChange(transitionType);

                if (transitionType != Transition.None)
                    Owner.ApplyModifiedProperties();

            }
            GUILayout.EndHorizontal();
        }
    }
}
#endif