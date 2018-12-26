#if UNITY_EDITOR
using UnityEditor;
using UI.Widget;

[CustomEditor(typeof(GUIOnOffSwitch))]
public class EditorOnOffSwitch : Editor
{
    private GUIOnOffSwitch Owner;

    private bool isDraw;
    private bool maskable;
    private bool interactable;
    private bool showPlaceholder;
    private string placeholder;


    private void OnEnable()
    {
        Owner = (GUIOnOffSwitch)target;
        Undo.RecordObject(Owner, Owner.name);

        maskable = Owner.Maskable;
        interactable = Owner.Interactable;
        showPlaceholder = Owner.IsPlaceholder;
        placeholder = Owner.Placeholder?.text;

        isDraw = !Owner.GetComponent<GUIInteractableIcon>();
    }

    public override void OnInspectorGUI()
    {
        if (isDraw)
        {
            UnityEditor.EditorGUI.BeginChangeCheck();
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

            showPlaceholder = EditorGUILayout.Foldout(showPlaceholder, "Use Placeholder");
            if (showPlaceholder != Owner.IsPlaceholder)
                Owner.IsPlaceholderChange(showPlaceholder);
            if (showPlaceholder)
            {
                placeholder = EditorGUILayout.TextField("Placeholder", placeholder);
                Owner.PlaceholderText(placeholder);
            }
            if (UnityEditor.EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(Owner);
            }
            base.OnInspectorGUI();
        }
    }
}
#endif