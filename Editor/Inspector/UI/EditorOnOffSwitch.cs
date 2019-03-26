#if UNITY_EDITOR
using UnityEditor;
using UI.Widget;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CustomInspector
{
    [CustomEditor(typeof(GUIOnOffSwitch), true)]
    public class EditorOnOffSwitch : EditorGUIBase
    {
        private GUIOnOffSwitch Owner;
        private Sprite onSprite;
        private Sprite offSprite;

        private SerializedProperty onClickEvt;

        protected override void OnEnable()
        {
            base.OnEnable();
            Owner = target as GUIOnOffSwitch;

            onSprite = Owner.OnSprite;
            offSprite = Owner.OffSprite;

            onClickEvt = serializedObject.FindProperty("onClick");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (!Owner.UIDependent && !Application.isPlaying)
            {
                OnOffSpriteOption();
                OnClickEventDrawer();
            }
        }

        private void OnOffSpriteOption()
        {
            GUILayout.BeginHorizontal();
            onSprite = EditorGUILayout.ObjectField("On Sprite", onSprite, typeof(Sprite), true, sizeOption) as Sprite;
            if (onSprite != Owner.OnSprite)
            {
                Owner.OnSpriteChange(onSprite);
                EditorUtility.SetDirty(Owner.OnSprite);
            }

            offSprite = EditorGUILayout.ObjectField("Off Sprite", offSprite, typeof(Sprite), true, sizeOption) as Sprite;
            if (offSprite != Owner.OffSprite)
            {
                Owner.OffSpriteChange(offSprite);
                EditorUtility.SetDirty(Owner.OffSprite);
            }

            GUILayout.EndHorizontal();
        }

        private void OnClickEventDrawer()
        {
            EditorGUILayout.PropertyField(onClickEvt);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif