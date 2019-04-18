using Entities.Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDecisionBehavior : MonoBehaviour
{
    public SIO_AttackListener attackListener;
    public NavAgentController navAgentController;

    private AgentDecisionTree agentDecisionMaking;

    private void Start()
    {
        agentDecisionMaking = new AgentDecisionTree(navAgentController,attackListener);
    }

    public void MakeDecision()
    {
        agentDecisionMaking.MakeDecision();
    }
}
