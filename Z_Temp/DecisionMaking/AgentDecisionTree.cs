using Entities.Navigation;
using System;
using System.Reflection;
using System.Xml;
using UnityEngine;
using UnityEngine.Events;

public class AgentDecisionTree : DecisionTreeNode
{
    private string xmlPath;
    private DecisionTreeNode root;
    private NavAgentController navAgentController;
    private SIO_AttackListener sio_attackListener;

    public AgentDecisionTree() { }

    public AgentDecisionTree(NavAgentController agentController, SIO_AttackListener attackListener)
    {
        navAgentController = agentController;
        sio_attackListener = attackListener;

        //BooleanDecisionNode isEnemyInAttackRange = new BooleanDecisionNode(
        //    new ActionNode(Attack_Action),
        //    new ActionNode(Move_Action),
        //    IsEnemyInAttackRange_Boolean
        //        );

        //BooleanDecisionNode isEnemyAtSelectedPosition = new BooleanDecisionNode(
        //    isEnemyInAttackRange,
        //    new ActionNode(Move_Action),
        //    IsEnemyAtTarget_Boolean
        //    );

        //root = isEnemyAtSelectedPosition;

        xmlPath = Application.dataPath + @"/Z_Temp/DecisionMaking/AgentDecisionTree.xml";
        root = Root(xmlPath);
    }

    public override DecisionTreeNode MakeDecision()
    {
        return root.MakeDecision();
    }

    private void Attack_Action()
    {
        AgentRemote owner = navAgentController.CurrentAgent.Remote;
        AgentRemote enemy = navAgentController.GetEnemyAt(navAgentController.CursorController.SelectedPosition);

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

    private bool IsEnemyAtTarget_Boolean()
    {
        return navAgentController.IsEnemyAtTarget_Boolean();
    }

    public DecisionTreeNode Root(string path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);        
        return CreateNode(xmlDoc.FirstChild);
    }

    public DecisionTreeNode CreateNode(XmlNode node)
    {
        string nodeType = node.LocalName;
        switch (nodeType)
        {
            case "BooleanDecisionNode":
                BooleanDecisionNode boolean = new BooleanDecisionNode();
                boolean.trueNode = CreateNode(GetNodeByAttribute(node, "condition", "true"));
                boolean.falseNode = CreateNode(GetNodeByAttribute(node, "condition", "true"));
                boolean.booleanFunc = CreateEvaluatorFunc(node.ChildNodes[2]);
                return boolean;
            case "ActionNode":
                ActionNode action = new ActionNode();
                action.doAction = CreateAction(node);
                return action;
            case "DecisionTreeNode":
                return CreateNode(node.FirstChild);
        }
        return null;
    }

    private Func<bool> CreateEvaluatorFunc(XmlNode xmlNode)
    {
        string funcName = xmlNode.InnerText;
        Type type = typeof(AgentDecisionTree);

        MethodInfo methodInfo = type.GetMethod(funcName, BindingFlags.NonPublic | BindingFlags.Instance);
        return Delegate.CreateDelegate(typeof(Func<bool>), this, methodInfo.Name) as Func<bool>;
    }

    private UnityAction CreateAction(XmlNode xmlNode)
    {
        string funcName = xmlNode.InnerText;
        Type type = typeof(AgentDecisionTree);

        MethodInfo methodInfo = type.GetMethod(funcName, BindingFlags.NonPublic | BindingFlags.Instance);
        return Delegate.CreateDelegate(typeof(UnityAction), this, methodInfo.Name) as UnityAction;
    }

    public XmlNode GetNodeByAttribute(XmlNode nodes, string arrName, string arrValue)
    {
        XmlNodeList list = nodes.ChildNodes;
        for (int i = 0; i < list.Count; i++)
        {
            XmlAttributeCollection attributes = list[i].Attributes;
            for (int j = 0; j < attributes.Count; j++)
            {
                if (attributes[j].Name.Equals(arrName))
                {
                    if (attributes[j].Value.Equals(arrValue))
                        return list[i];
                }
            }
        }
        return null;
    }

}
