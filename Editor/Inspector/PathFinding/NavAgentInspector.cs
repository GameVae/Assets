#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NavAgent))]
public class NavAgentInspector : Editor
{
    private NavAgent owner;

    private bool showInfo;

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
            InfoGroup();

            if(GUILayout.Button("Offset Move"))
            {
                owner.StartOffsetMove();
            }
            // mask end vertical layout group
            EditorGUILayout.EndVertical();
        }
        base.OnInspectorGUI();
    }
    private void InfoGroup()
    {
        // mask start vertical layout group
        EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));

        EditorGUILayout.LabelField("Star Cell: " + owner.StartCell.ToString());
        EditorGUILayout.LabelField("End Cell : " + owner.EndCell.ToString());
        EditorGUILayout.LabelField("Current Cell : " + owner.CurrentCell.ToString());

        // mask end vertical layout group
        EditorGUILayout.EndVertical();
    }
}
#endif