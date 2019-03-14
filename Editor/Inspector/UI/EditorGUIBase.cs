#if UNITY_EDITOR

using UnityEditor;
using UI.Widget;
using UnityEngine;
using UnityEditor.SceneManagement;

namespace UI.CustomInspector
{
    [CustomEditor(typeof(CustomGUI), editorForChildClasses: true), CanEditMultipleObjects]
    public class EditorGUIBase : Editor
    {
        private CustomGUI BaseOwner;

        protected bool maskable;
        protected Sprite maskSprite;

        protected bool useBackgroud;
        protected Sprite backgroudSprite;

        protected bool interactable;
        protected bool isPlaceholder;
        protected float fontSize;
        protected string placeholder;
        protected Color placeholderColor;

        [SerializeField]
        public bool isCollapseOption;

        protected virtual void OnEnable()
        {
            BaseOwner = (CustomGUI)target;
            Undo.RecordObject(BaseOwner, BaseOwner.name);
            BaseOwner.SetChildrenDependence();

            // init 
            interactable = BaseOwner.Interactable;

            // backgroud sprite
            backgroudSprite = BaseOwner.BackgroudSprite;
            useBackgroud = BaseOwner.IsBackground;

            // mask field setup
            maskable = BaseOwner.Maskable;
            maskSprite = BaseOwner.MaskSprite;

            // placeholder field
            isPlaceholder = BaseOwner.IsPlaceholder;
            placeholder = BaseOwner.Placeholder?.text;
            placeholderColor = BaseOwner.PlaceholderColor;
        }

        public override void OnInspectorGUI()
        {
            if (!BaseOwner.UIDependent && !Application.isPlaying)
            {
                InteractableGUI();
                MaskableGUI();
                BackgroudGUI();
                PlaceholderGUI();

                if (GUI.changed)
                {
                    EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                }
                base.OnInspectorGUI();
            }
        }


        protected virtual void MaskableGUI()
        {
            maskable = EditorGUILayout.Toggle("Maskable", maskable);
            if (maskable != BaseOwner.Maskable)
            {
                BaseOwner.MaskableChange(maskable);
            }
            if (maskable)
            {
                maskSprite = EditorGUILayout.ObjectField("Mask Sprite", maskSprite, typeof(Sprite), false) as Sprite;
                if (BaseOwner.MaskSprite != maskSprite)
                {
                    BaseOwner.MaskSpriteChange(maskSprite);
                    EditorUtility.SetDirty(BaseOwner.MaskSprite);
                }
            }
        }

        protected virtual void BackgroudGUI()
        {
            useBackgroud = EditorGUILayout.Toggle("Is Use Backgroud", useBackgroud);
            if (useBackgroud != BaseOwner.IsBackground)
            {
                BaseOwner.IsBackgroudChange(useBackgroud);
            }
            if (useBackgroud)
            {
                backgroudSprite = EditorGUILayout.ObjectField("Backgroud Sprite", backgroudSprite, typeof(Sprite), false) as Sprite;
                if (BaseOwner.BackgroudSprite != backgroudSprite && backgroudSprite != null)
                {
                    BaseOwner.BackgroundChange(backgroudSprite);
                    EditorUtility.SetDirty(BaseOwner.BackgroudSprite);
                }
            }
        }

        protected virtual void InteractableGUI()
        {
            interactable = EditorGUILayout.Toggle("Interactable", interactable);
            if (interactable != BaseOwner.Interactable)
            {
                BaseOwner.InteractableChange(interactable);
            }
        }

        protected virtual void PlaceholderGUI()
        {
            isPlaceholder = EditorGUILayout.Foldout(BaseOwner.IsPlaceholder, "Use Placeholder");
            if (isPlaceholder != BaseOwner.IsPlaceholder)
                BaseOwner.IsPlaceholderChange(isPlaceholder);
            if (isPlaceholder)
            {
                placeholder = EditorGUILayout.DelayedTextField("Placeholder", placeholder);
                BaseOwner.PlaceholderValueChange(placeholder);

                placeholderColor = EditorGUILayout.ColorField("Color", placeholderColor);
                if (placeholderColor != BaseOwner.PlaceholderColor)
                {
                    BaseOwner.PlaceholderColorChange(placeholderColor);
                }

                fontSize = EditorGUILayout.DelayedFloatField("Font Size", fontSize);
                if (!Mathf.Approximately(fontSize, BaseOwner.FontSize))
                {
                    BaseOwner.FontSizeChange(fontSize);
                }
            }
        }
    }
}
#endif
