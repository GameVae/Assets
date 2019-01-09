#if UNITY_EDITOR

using UnityEditor;
using UI.Widget;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace UI.CustomInspector
{
    [CustomEditor(typeof(CustomGUI), editorForChildClasses: true)]
    public class EditorGUIBase : Editor
    {
        private CustomGUI BaseOwner;

        protected bool maskable;
        protected bool interactable;
        protected string placeholder;
        protected bool showPlaceholder;
        protected Color placeholderColor;
        protected float fontSize;

        protected virtual void OnEnable()
        {
            BaseOwner = (CustomGUI)target;
            Undo.RecordObject(BaseOwner, BaseOwner.name);
            BaseOwner.SetChildrenDependence();

            maskable = BaseOwner.Maskable;
            interactable = BaseOwner.Interactable;
            placeholder = BaseOwner.Placeholder?.text;
            showPlaceholder = BaseOwner.IsPlaceholder;
            placeholderColor = BaseOwner.PlaceholderColor;
        }

        public override void OnInspectorGUI()
        {
            if (!BaseOwner.UIDependent && !Application.isPlaying)
            {
                maskable = EditorGUILayout.Toggle("Maskable", BaseOwner.Maskable);
                if (maskable != BaseOwner.Maskable)
                {
                    BaseOwner.MaskableChange(maskable);
                }

                interactable = EditorGUILayout.Toggle("Interactable", BaseOwner.Interactable);
                if (interactable != BaseOwner.Interactable)
                {
                    BaseOwner.InteractableChange(interactable);
                }

                showPlaceholder = EditorGUILayout.Foldout(BaseOwner.IsPlaceholder, "Use Placeholder");
                if (showPlaceholder != BaseOwner.IsPlaceholder)
                    BaseOwner.IsPlaceholderChange(showPlaceholder);
                if (showPlaceholder)
                {
                    placeholder = EditorGUILayout.DelayedTextField("Placeholder", BaseOwner.Placeholder.text);
                    BaseOwner.PlaceholderText(placeholder);

                    placeholderColor = EditorGUILayout.ColorField("Color", BaseOwner.PlaceholderColor);
                    if (placeholderColor != BaseOwner.PlaceholderColor)
                    {
                        BaseOwner.PlaceholderColorChange(placeholderColor);
                    }

                    fontSize = EditorGUILayout.DelayedFloatField("Font Size", BaseOwner.FontSize);
                    if (!Mathf.Approximately(fontSize, BaseOwner.FontSize))
                    {
                        BaseOwner.FontSizeChange(fontSize);
                    }
                }

                if (GUI.changed)
                {
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                }
                base.OnInspectorGUI();
            }
        }
    }
}
#endif
