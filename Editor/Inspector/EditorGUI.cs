#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace UI.CustomInspector
{
    public sealed class EditorCustomUI : MonoBehaviour
    {
        [MenuItem("GameObject/UI/GUI/Slider")]
        public static void CreateSlider(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUISlider.prefab";
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            if (asset != null)
            {
                GameObject slider = Instantiate(asset) as GameObject;
                GameObjectUtility.SetParentAndAlign(slider, menuCmd.context as GameObject);
                Undo.RegisterCreatedObjectUndo(slider, "Create " + slider.name);
                Selection.activeObject = slider;
            }
        }

        [MenuItem("GameObject/UI/GUI/InteractableIcon")]
        public static void CreateInteractableIcon(MenuCommand menuCmd)
        {

            string path = @"Assets/Prefabs/GUIInteractableIcon.prefab";
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            if (asset != null)
            {
                GameObject icon = Instantiate(asset) as GameObject;
                GameObjectUtility.SetParentAndAlign(icon, menuCmd.context as GameObject);
                Undo.RegisterCreatedObjectUndo(icon, "Create " + icon.name);
                Selection.activeObject = icon;
            }
        }

        [MenuItem("GameObject/UI/GUI/CheckMask")]
        public static void CreateCheckMask(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUICheckMask.prefab";
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            if (asset != null)
            {
                GameObject checkMark = Instantiate(asset) as GameObject;
                GameObjectUtility.SetParentAndAlign(checkMark, menuCmd.context as GameObject);
                Undo.RegisterCreatedObjectUndo(checkMark, "Create " + checkMark.name);
                Selection.activeObject = checkMark;
            }
        }

        [MenuItem("GameObject/UI/GUI/Toggle")]
        public static void CreateToggle(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUIToggle.prefab";
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            if (asset != null)
            {
                GameObject toggle = Instantiate(asset) as GameObject;
                GameObjectUtility.SetParentAndAlign(toggle, menuCmd.context as GameObject);
                Undo.RegisterCreatedObjectUndo(toggle, "Create " + toggle.name);
                Selection.activeObject = toggle;
            }
        }

        [MenuItem("GameObject/UI/GUI/OnOffSwitch")]
        public static void CreateSwitch(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUIOnOffSwitch.prefab";
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            if (asset != null)
            {
                GameObject onOffSwitch = Instantiate(asset) as GameObject;
                GameObjectUtility.SetParentAndAlign(onOffSwitch, menuCmd.context as GameObject);
                Undo.RegisterCreatedObjectUndo(onOffSwitch, "Create " + onOffSwitch.name);
                Selection.activeObject = onOffSwitch;
            }
        }

        [MenuItem("GameObject/UI/GUI/GUISliderWithBtn")]
        public static void CreateSliderWithBtn(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUISliderWithBtn.prefab";
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            if (asset != null)
            {
                GameObject sliderWithBtn = Instantiate(asset) as GameObject;
                GameObjectUtility.SetParentAndAlign(sliderWithBtn, menuCmd.context as GameObject);
                Undo.RegisterCreatedObjectUndo(sliderWithBtn, "Create " + sliderWithBtn.name);
                Selection.activeObject = sliderWithBtn;
            }
        }

        [MenuItem("GameObject/UI/GUI/GUIHorizontalBarInfo")]
        public static void CreateHorizontalBarInfo(MenuCommand menuCmd)
        {
            string path = @"Assets/Prefabs/GUIHorizontalBarInfo.prefab";
            Object asset = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            if (asset != null)
            {
                GameObject infoBar = Instantiate(asset) as GameObject;
                GameObjectUtility.SetParentAndAlign(infoBar, menuCmd.context as GameObject);
                Undo.RegisterCreatedObjectUndo(infoBar, "Create " + infoBar.name);
                Selection.activeObject = infoBar;
            }
        }
    }
}
#endif
