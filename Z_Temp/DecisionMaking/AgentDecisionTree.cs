using Entities.Navigation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentDecisionTree : DecisionTreeNode
{
    private DecisionTreeNode root;

    public AgentDecisionTree(NavAgentController agentController)
    {

    }

    public override DecisionTreeNode MakeDecision()
    {
        return root.MakeDecision();
    }
}
