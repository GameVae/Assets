#if UNITY_EDITOR
using UnityEditor;

public class EditorLayoutHelper
{
    public class SerializedPropertyDrawer
    {
        private readonly SerializedProperty colorBlock;
        
        public SerializedPropertyDrawer(SerializedObject serObj,string fieldName)
        {
            colorBlock = serObj.FindProperty(fieldName);
        }
        public void Draw()
        {
            EditorGUILayout.PropertyField(colorBlock);
        }
    }
}

#endif