#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GUIToggle))]
public class EditorToggle : Editor
{
    private GUIToggle Owner;

    private bool maskable;
    private bool interactable;

    private void OnEnable()
    {
        Owner = (GUIToggle)target;

        maskable = Owner.Maskable;
        interactable = Owner.Interactable;
    }

    public override void OnInspectorGUI()
    {
        maskable = EditorGUILayout.Toggle("Maskable", maskable);
        if (maskable != Owner.Maskable)
        {
            Owner.MaskableChange(maskable);
        }

        interactable = EditorGUILayout.Toggle("Interactable", interactable);
        if (interactable != Owner.Interactable)
        {
            Owner.InteractableChange(interactable);
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

        base.OnInspectorGUI();
    }
}
#endif