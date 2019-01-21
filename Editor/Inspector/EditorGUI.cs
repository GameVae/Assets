﻿#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UI.CustomInspector
{
    public sealed class EditorCustomUI : MonoBehaviour
    {
        private static void Create(MenuCommand menuCmd, string path)
        {
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            if (asset != null)
            {
                GameObject obj = Instantiate(asset) as GameObject;
                GameObjectUtility.SetParentAndAlign(obj, menuCmd.context as GameObject);
                Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
                Selection.activeObject = obj;
            }
        }

        [MenuItem("GameObject/UI/GUI/GUIVerticalList")]
        public static void CreateVerticalList(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUIVerticalList.prefab";
            Create(menuCmd, path);
        }

        [MenuItem("GameObject/UI/GUI/GUITextWithIcon")]
        public static void CreateTextWithIcon(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUITextWithIcon.prefab";
            Create(menuCmd, path);
        }

        [MenuItem("GameObject/UI/GUI/GUISlider")]
        public static void CreateSlider(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUISlider.prefab";
            Create(menuCmd, path);
        }

        [MenuItem("GameObject/UI/GUI/GUIInteractableIcon")]
        public static void CreateInteractableIcon(MenuCommand menuCmd)
        {

            string path = @"Assets/Prefabs/GUIInteractableIcon.prefab";
            Create(menuCmd, path);
        }

        [MenuItem("GameObject/UI/GUI/GUICheckMask")]
        public static void CreateCheckMask(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUICheckMask.prefab";
            Create(menuCmd, path);
        }

        [MenuItem("GameObject/UI/GUI/GUIToggle")]
        public static void CreateToggle(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUIToggle.prefab";
            Create(menuCmd, path);
        }

        [MenuItem("GameObject/UI/GUI/GUIOnOffSwitch")]
        public static void CreateSwitch(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUIOnOffSwitch.prefab";
            Create(menuCmd, path);
        }

        [MenuItem("GameObject/UI/GUI/GUISliderWithBtn")]
        public static void CreateSliderWithBtn(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUISliderWithBtn.prefab";
            Create(menuCmd, path);

        }

        [MenuItem("GameObject/UI/GUI/GUIHorizontalBarInfo")]
        public static void CreateHorizontalBarInfo(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUIHorizontalBarInfo.prefab";
            Create(menuCmd, path);
        }
    }
}
#endif