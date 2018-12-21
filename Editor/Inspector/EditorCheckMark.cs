#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GUICheckMark))]
public class EditorCheckMark : Editor
{
    private GUICheckMark Owner;

    private bool maskable;
    private bool interactable;
    private bool isShowText;
    private string placeholder;


    private void OnEnable()
    {
        Owner = (GUICheckMark)target;
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

        isShowText = EditorGUILayout.Foldout(isShowText, "Use Placeholder");
        if (isShowText != Owner.IsShowText)
            Owner.IsShowTextChange(isShowText);
        if(isShowText)
        {
            placeholder = EditorGUILayout.TextField("Placeholder", placeholder);
            Owner.PlaceholderText(placeholder);
        }
        base.OnInspectorGUI();
    }
}
#endif