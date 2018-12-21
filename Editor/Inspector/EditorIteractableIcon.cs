#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GUIInteractableIcon))]
public class EditorIteractableIcon : Editor
{
    private GUIInteractableIcon Owner;

    private bool maskable;
    private bool interactable;
    private string placeholder;

    private void OnEnable()
    {
        Owner = (GUIInteractableIcon)target;
        maskable = Owner.Maskable;

        interactable = Owner.Interactable;
        placeholder = Owner.Placeholder?.text;
        if (placeholder == null) placeholder = Owner.GetComponent<TextMeshProUGUI>().text;
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

        placeholder = EditorGUILayout.TextField("Placeholder", placeholder);
        Owner.PlaceholderText(placeholder);

        base.OnInspectorGUI();
    }
}
#endif