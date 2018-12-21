#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GUIProgressSlider))]
public class EditorProgressSlider : Editor
{
    private GUIProgressSlider Owner;

    private bool maskable;
    private bool interactable;
    private string placeholder;
    private bool showPlaceholder;

    private void OnEnable()
    {
        Owner = (GUIProgressSlider)target;

        maskable = Owner.Maskable;
        interactable = Owner.Interactable;
        placeholder = Owner.PlaceHolder?.text;
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

        showPlaceholder = EditorGUILayout.Toggle("Show Placeholder", showPlaceholder);
        if(showPlaceholder != Owner.IsShowText)
        {
            Owner.IsShowTextChange(showPlaceholder);
        }

        placeholder = EditorGUILayout.TextField("Placeholder", placeholder);
        Owner.PlaceholderText(placeholder);

        base.OnInspectorGUI();
    }
}
#endif