#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NavAgent))]
public class NavAgentInspector : Editor
{
    private NavAgent owner;

    private bool showInfo;
    private bool showOption;

    private void OnEnable()
    {
        owner = (NavAgent)target;
        showInfo = true;
    }

    public override void OnInspectorGUI()
    {
        showInfo = EditorGUILayout.Foldout(showInfo, "Show Info");
        if(showInfo)
        {
            // mask start vertical layout group
            EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
            OptionGroup();
            InfoGroup();
            // mask end vertical layout group
            EditorGUILayout.EndVertical();
        }
        base.OnInspectorGUI();
    }

 
    private void OptionGroup()
    {
        showOption = EditorGUILayout.BeginToggleGroup(label: "Option", toggle: showOption);
        if (showOption)
        {
            owner.IsComparePath = EditorGUILayout.Toggle(   label: "Compare Path ",
                                                            value: owner.IsComparePath);
            owner.IsAutoMove    = EditorGUILayout.Toggle(   label: "Move " + owner.SearchType.ToString(),
                                                            value: owner.IsAutoMove);
        }
        EditorGUILayout.EndToggleGroup();
    }

    private void InfoGroup()
    {
        // mask start vertical layout group
        EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));

        EditorGUILayout.LabelField("Star Cell: " + owner.StartCell.ToString());
        EditorGUILayout.LabelField("End Cell : " + owner.EndCell.ToString());
        EditorGUILayout.LabelField("Algorithm: " + owner.SearchType.ToString());

        // mask end vertical layout group
        EditorGUILayout.EndVertical();
    }
}
#endif