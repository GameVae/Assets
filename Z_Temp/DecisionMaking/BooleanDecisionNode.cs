

public class BooleanDecisionNode : DecisionNode
{
    private readonly System.Func<bool> booleanFunc;
    public BooleanDecisionNode(DecisionTreeNode tN, DecisionTreeNode fN,System.Func<bool> func) :
        base(tN, fN)
    {
        booleanFunc = func;
    }

    public override DecisionTreeNode GetBranch()
    {
        return booleanFunc?.Invoke() == true ? trueNode : falseNode;
    }
}
