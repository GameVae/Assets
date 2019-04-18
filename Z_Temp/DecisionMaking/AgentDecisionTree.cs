using Entities.Navigation;

public class AgentDecisionTree : DecisionTreeNode
{
    private DecisionTreeNode root;
    private NavAgentController navAgentController;
    private SIO_AttackListener sio_attackListener;

    public AgentDecisionTree(NavAgentController agentController, SIO_AttackListener attackListener)
    {
        navAgentController = agentController;
        sio_attackListener = attackListener;

        BooleanDecisionNode isEnemyInAttackRange = new BooleanDecisionNode(
            new ActionNode(Attack_Action),
            new ActionNode(Move_Action),
            IsEnemyInAttackRange_Boolean
                );

        BooleanDecisionNode isEnemyAtSelectedPosition = new BooleanDecisionNode(
            isEnemyInAttackRange,
            new ActionNode(Move_Action),
            agentController.IsEnemyAtTarget_Boolean
            );

        root = isEnemyAtSelectedPosition;
    }

    public override DecisionTreeNode MakeDecision()
    {
        return root.MakeDecision();
    }

    private void Attack_Action()
    {
        NavRemote owner = navAgentController.CurrentAgent.Remote;
        NavRemote enemy = navAgentController.GetEnemyAt(navAgentController.CursorController.SelectedPosition);

        sio_attackListener.S_ATTACK(owner, enemy);
    }

    private void Move_Action()
    {
        navAgentController.Move_Action();
    }

    private bool IsEnemyInAttackRange_Boolean()
    {
        return navAgentController.IsTargetInRange_Boolean(1);
    }
}
