#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UI.Widget;

namespace UI.CustomInspector
{
    [CustomEditor(typeof(GUIToggle))]
    public class EditorToggle : EditorGUIBase
    {
        private GUIToggle Owner;
        private GUIToggle.AxisType Type;
        private Sprite activeSprite;
        private Sprite unactiveSprite;

        protected override void OnEnable()
        {
            base.OnEnable();
            Owner = (GUIToggle)target;

            Type = Owner.Type;
            activeSprite = Owner.ActiveSprite;
            unactiveSprite = Owner.UnactiveSprite;
        }

        public override void OnInspectorGUI()
        {
            isPlaceholder = false;
            base.OnInspectorGUI();
            if (!Owner.UIDependent && !Application.isPlaying)
            {
                FuncGroup();
                SpriteOption();
            }
        }

        private void FuncGroup()
        {
            Type = (GUIToggle.AxisType)EditorGUILayout.EnumPopup(Type, GUILayout.MaxWidth(150));
            if (Type != Owner.Type)
            {
                Owner.TypeChange(Type);
            }

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+", GUILayout.MaxWidth(50)))
            {
                Selection.activeGameObject = Owner.gameObject;
                string path = @"Assets/Prefabs/UI/GUICheckMask.prefab";
                Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
                if (asset != null)
                {
                    GameObject checkMark = Instantiate(asset) as GameObject;
                    GameObjectUtility.SetParentAndAlign(checkMark, Owner.gameObject);
                    Undo.RegisterCreatedObjectUndo(checkMark, "Create " + checkMark.name);
                    Owner.Add(checkMark.GetComponent<GUICheckMark>());
                }
            }
            if (GUILayout.Button("-", GUILayout.MaxWidth(50)))
            {
                Owner.DestroyLastIndex();
            }
            if (GUILayout.Button("#", GUILayout.MaxWidth(50)))
            {
                Owner.Refresh();
            }
            GUILayout.EndHorizontal();
        }

        private void SpriteOption()
        {
            GUILayout.BeginHorizontal();
            activeSprite = EditorGUILayout.ObjectField("Active Sprite", activeSprite, typeof(Sprite), true, sizeOption) as Sprite;
            if (activeSprite != Owner.ActiveSprite)
            {
                Owner.ActiveSpriteChange(activeSprite);
                EditorUtility.SetDirty(Owner.ActiveSprite);
            }

            unactiveSprite = EditorGUILayout.ObjectField("UnActive Sprite", unactiveSprite, typeof(Sprite), true, sizeOption) as Sprite;
            if (unactiveSprite != Owner.UnactiveSprite)
            {
                Owner.UnactiveSpriteChange(unactiveSprite);
                EditorUtility.SetDirty(Owner.UnactiveSprite);
            }
        }
    }
}
#endif