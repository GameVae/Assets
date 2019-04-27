using Entities.Navigation;
using Generic.Singleton;
using UnityEngine;

public class AgentDecisionTree : DecisionTreeNode
{
    private DecisionTreeNode root;

    private bool isLoaded;
    private string xmlPath;

    public readonly DecisionTreeLoader TreeLoader;
    public readonly NavAgentController NavAgentController;
    public readonly SIO_AttackListener SIO_AttackListener;

    public AgentDecisionTree(SIO_AttackListener attackListener)
    {
        isLoaded = false;
        xmlPath = Application.dataPath + @"/Z_Temp/DecisionMaking/AgentDecisionTree.xml";

        TreeLoader = Singleton.Instance<DecisionTreeLoader>();
        NavAgentController = Singleton.Instance<NavAgentController>();
        SIO_AttackListener = attackListener;

        DecisionTreeLoader.TreeInfo info = new DecisionTreeLoader.TreeInfo()
        {
            XmlLocalPath = xmlPath,
            MethodContainer = this,
            ResultHanlder = LoadTreeComplete
        };
        TreeLoader.AsyncCreateNode(info);

        // TODO:[for test]
        // ManualLoadTree();
        // root = Root(xmlPath);
    }

    private void ManualLoadTree()
    {
        BooleanDecisionNode isEnemyInAttackRange = new BooleanDecisionNode(
            new ActionNode(Attack_Action),
            new ActionNode(Move_Action),
            IsEnemyInAttackRange_Boolean
                );

        BooleanDecisionNode isEnemyAtSelectedPosition = new BooleanDecisionNode(
            isEnemyInAttackRange,
            new ActionNode(Move_Action),
            IsEnemyAtTarget_Boolean
            );

        root = isEnemyAtSelectedPosition;
    }

    public override DecisionTreeNode MakeDecision()
    {
        if (isLoaded)
            return root.MakeDecision();
        return null;
    }

    private void Attack_Action()
    {
        AgentRemote owner = NavAgentController.CurrentAgent.Remote;
        AgentRemote enemy = NavAgentController.GetEnemyAt(NavAgentController.CursorController.SelectedPosition);

        SIO_AttackListener.S_ATTACK(owner, enemy);
    }

    private void Move_Action()
    {
        NavAgentController.Move_Action();
    }

    private bool IsEnemyInAttackRange_Boolean()
    {
        return NavAgentController.IsTargetInRange_Boolean(1);
    }

    private bool IsEnemyAtTarget_Boolean()
    {
        return NavAgentController.IsEnemyAtTarget_Boolean();
    }

    private void LoadTreeComplete(DecisionTreeNode tree)
    {
        Debugger.Log("Load decision tree done");
        isLoaded = true;
        root = tree;
    }
}
