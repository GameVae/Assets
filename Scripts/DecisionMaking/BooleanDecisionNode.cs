

public class BooleanDecisionNode : DecisionNode
{
    public System.Func<bool> booleanFunc;
    public BooleanDecisionNode(DecisionTreeNode tN, DecisionTreeNode fN,System.Func<bool> func) :
        base(tN, fN)
    {
        booleanFunc = func;
    }

    public BooleanDecisionNode() { }

    public override DecisionTreeNode GetBranch()
    {
        return booleanFunc?.Invoke() == true ? trueNode : falseNode;
    }
}
