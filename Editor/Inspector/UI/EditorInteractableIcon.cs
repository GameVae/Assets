#if UNITY_EDITOR
using UnityEditor;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UI.CustomInspector
{
    [CustomEditor(typeof(GUIInteractableIcon))]
    public class EditorInteractableIcon : EditorGUIBase
    {
        private GUIInteractableIcon Owner;

        private SerializedProperty onClickEvents;

        protected override void OnEnable()
        {
            base.OnEnable();
            Owner = target as GUIInteractableIcon;
            onClickEvents = serializedObject.FindProperty("onClick");
            
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.PropertyField(onClickEvents);
            serializedObject.ApplyModifiedProperties();

        }
    }
}
#endif