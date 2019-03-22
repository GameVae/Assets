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

        protected bool showMaskGrap;
        protected bool maskable;
        protected Sprite maskSprite;

        protected bool useBackgroud;
        protected Sprite backgroudSprite;

        protected bool interactable;
        protected bool isPlaceholder;
        protected float fontSize;
        protected string placeholder;
        protected Color placeholderColor;

        protected readonly GUILayoutOption[] sizeOption = new GUILayoutOption[]
        {
            GUILayout.MaxWidth(200),
            GUILayout.MaxHeight(50)
        };

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
            showMaskGrap = BaseOwner.Mask ? BaseOwner.Mask.showMaskGraphic : false;

            // placeholder field
            isPlaceholder = BaseOwner.IsPlaceholder;
            placeholder = BaseOwner.Placeholder?.text;
            placeholderColor = BaseOwner.PlaceholderColor;
            fontSize = BaseOwner.FontSize;
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
            GUILayout.BeginHorizontal();
            maskable = EditorGUILayout.Toggle("Maskable", maskable, sizeOption);
            if (maskable != BaseOwner.Maskable)
            {
                BaseOwner.MaskableChange(maskable);
            }
            if (maskable)
            {
                showMaskGrap = EditorGUILayout.Toggle("Show Mask Graphic", showMaskGrap);
                BaseOwner.Mask.showMaskGraphic = showMaskGrap;
            }
            GUILayout.EndHorizontal();


            if (maskable)
            {
                maskSprite = (Sprite)
                   EditorGUILayout.ObjectField("Mask Sprite", maskSprite, typeof(Sprite), false, sizeOption);
                if (BaseOwner.MaskSprite != maskSprite)
                {
                    BaseOwner.MaskSpriteChange(maskSprite);
                    EditorUtility.SetDirty(BaseOwner.MaskSprite);
                }
            }
        }

        protected virtual void BackgroudGUI()
        {
            useBackgroud = EditorGUILayout.Toggle("Is Use Backgroud", BaseOwner.IsBackground);
            if (useBackgroud != BaseOwner.IsBackground)
            {
                BaseOwner.IsBackgroudChange(useBackgroud);
            }
            if (useBackgroud)
            {
                backgroudSprite = (Sprite)EditorGUILayout.ObjectField("Backgroud Sprite", backgroudSprite, typeof(Sprite), false, sizeOption);
                if (BaseOwner.BackgroudSprite != backgroudSprite)
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
            isPlaceholder = EditorGUILayout.Foldout(isPlaceholder, "Use Placeholder");
            if (isPlaceholder != BaseOwner.IsPlaceholder)
                BaseOwner.IsPlaceholderChange(isPlaceholder);
            if (isPlaceholder)
            {
                bool isChanged = false;
                placeholder = EditorGUILayout.DelayedTextField("Placeholder", placeholder);
                if (placeholder != BaseOwner.Placeholder?.text)
                {
                    BaseOwner.PlaceholderValueChange(placeholder);
                    isChanged = true;
                }

                placeholderColor = EditorGUILayout.ColorField("Color", placeholderColor);
                if (placeholderColor != BaseOwner.PlaceholderColor)
                {
                    BaseOwner.PlaceholderColorChange(placeholderColor);
                    isChanged = true;
                }

                fontSize = EditorGUILayout.DelayedFloatField("Font Size", fontSize);
                if (!Mathf.Approximately(fontSize, BaseOwner.FontSize))
                {
                    BaseOwner.FontSizeChange(fontSize);
                    isChanged = true;
                }
                if (isChanged) EditorUtility.SetDirty(BaseOwner.Placeholder);
            }
        }
    }
}
#endif
