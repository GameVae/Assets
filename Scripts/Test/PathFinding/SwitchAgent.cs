using UnityEngine;
using UnityEngine.UI;

public class SwitchAgent : MonoBehaviour
{
    public NavAgent[] AgentArray;

    private void Awake()
    {
        for (int i = 0; i < AgentArray.Length && i < transform.childCount; i++)
        {
            Button child = transform.GetChild(i).GetComponent<Button>();
            child.onClick.AddListener(DisableAllAgent);
        }
    }

    void DisableAllAgent()
    {
        for (int i = 0; i < AgentArray.Length; i++)
        {
            AgentArray[i].enabled = false;
            AgentArray[i].ClearPath();
        }
    }
}
