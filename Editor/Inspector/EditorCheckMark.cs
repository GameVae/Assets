#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UI.Widget;

[CustomEditor(typeof(GUICheckMark))]
public class EditorCheckMark : Editor
{
    private GUICheckMark Owner;

    private bool maskable;
    private bool interactable;
    private bool showPlaceholder;
    private string placeholder;

    private void OnEnable()
    {
        Owner = (GUICheckMark)target;
        Undo.RecordObject(Owner, Owner.name);

        maskable = Owner.Maskable;
        interactable = Owner.Interactable;
        showPlaceholder = Owner.IsPlaceholder;
        placeholder = Owner.Placeholder?.text;
    }

    public override void OnInspectorGUI()
    {
        maskable = EditorGUILayout.Toggle("Maskable", Owner.Maskable);
        if (maskable != Owner.Maskable)
        {
            Owner.MaskableChange(maskable);
        }

        interactable = EditorGUILayout.Toggle("Interactable", Owner.Interactable);
        if (interactable != Owner.Interactable)
        {
            Owner.InteractableChange(interactable);
        }

        showPlaceholder = EditorGUILayout.Foldout(Owner.IsPlaceholder, "Use Placeholder");
        if (showPlaceholder != Owner.IsPlaceholder)
            Owner.IsPlaceholderChange(showPlaceholder);
        if(showPlaceholder)
        {
            placeholder = EditorGUILayout.TextField("Placeholder", placeholder);
            Owner.PlaceholderText(placeholder);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(Owner);
        }
        base.OnInspectorGUI();
    }
}
#endif