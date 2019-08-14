using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecisionNode : DecisionTreeNode
{
    public DecisionTreeNode trueNode;
    public DecisionTreeNode falseNode;

    public abstract DecisionTreeNode GetBranch();

    public DecisionNode(DecisionTreeNode tN, DecisionTreeNode fN)
    {
        trueNode = tN;
        falseNode = fN;
    }

    public DecisionNode() { }

    public override DecisionTreeNode MakeDecision()
    {
        return GetBranch().MakeDecision();
    }
}
