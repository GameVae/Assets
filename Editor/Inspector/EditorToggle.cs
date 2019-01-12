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
        public GUIToggle.ToggleType Type;


        protected override void OnEnable()
        {
            base.OnEnable();
            Owner = (GUIToggle)target;

            Type = Owner.Type;
        }

        public override void OnInspectorGUI()
        {
            showPlaceholder = false;
            base.OnInspectorGUI();
            if (!Owner.UIDependent && !Application.isPlaying)
            {
                Type = (GUIToggle.ToggleType)EditorGUILayout.EnumPopup(Type, GUILayout.MaxWidth(150));
                if (Type != Owner.Type)
                {
                    Owner.TypeChange(Type);
                }

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("+", GUILayout.MaxWidth(50)))
                { 
                    Selection.activeGameObject = Owner.gameObject;
                    string path = @"Assets/Prefabs/GUICheckMask.prefab";
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
        }
    }
}
#endif