using UnityEngine;

public class AgentDecisionBehavior : MonoBehaviour
{
    public SIO_AttackListener attackListener;
    private AgentDecisionTree agentDecisionMaking;

    private void Start()
    {
        agentDecisionMaking = new AgentDecisionTree(attackListener);
    }

    public void MakeDecision()
    {
        DecisionTreeNode action = agentDecisionMaking.MakeDecision();
        //Debugger.Log(action);
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidAdbLog.LogInfo(action);
#endif
    }
}
