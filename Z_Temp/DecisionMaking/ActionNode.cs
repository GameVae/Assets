using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Leaf node
/// </summary>
public class ActionNode : DecisionTreeNode
{
    public UnityAction doAction;

    public ActionNode(UnityAction actions)
    {
        doAction = actions;
    }

    public ActionNode() { }

    public override DecisionTreeNode MakeDecision()
    {
        doAction?.Invoke();
        return this;
    }
}
