using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Leaf node
/// </summary>
public abstract class ActionNode : DecisionTreeNode
{
    protected abstract void DoAction();
    public override DecisionTreeNode MakeDecision()
    {
        DoAction();
        return this;
    }
}
