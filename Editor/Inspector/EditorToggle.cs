#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(GUIToggle))]
public class EditorToggle : Editor
{
    private GUIToggle Owner;

    public bool maskable;
    public bool interactable;
    public GUIToggle.ToggleType Type;


    private void OnEnable()
    {
        Owner = (GUIToggle)target;
        Undo.RecordObject(Owner.gameObject, Owner.name);

        maskable = Owner.Maskable;
        interactable = Owner.Interactable;
        Type = Owner.Type;
    }

    public override void OnInspectorGUI()
    {
        maskable = EditorGUILayout.Toggle("Maskable", maskable);
        if (maskable != Owner.Maskable)
        {
            Owner.MaskableChange(maskable);
        }

        interactable = EditorGUILayout.Toggle("Interactable", Owner.Interactable);
        if (interactable != Owner.Interactable)
        {
            Owner.InteractableChange(interactable);
        }

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
        base.OnInspectorGUI();
        GUILayout.EndHorizontal();

        if(GUI.changed)
        {
            EditorUtility.SetDirty(Owner);
        }
    }
}
#endif