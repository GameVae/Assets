using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecisionNode : DecisionTreeNode
{
    protected DecisionTreeNode trueNode;
    protected DecisionTreeNode falseNode;
    public object TestValue;

    public abstract DecisionTreeNode GetBranch();

    public DecisionNode(DecisionTreeNode tN, DecisionTreeNode fN)
    {
        trueNode = tN;
        falseNode = fN;
    }

    public override DecisionTreeNode MakeDecision()
    {
        return GetBranch().MakeDecision();
    }
}
