using System;
using System.Xml;
using MultiThread;
using Extensions.Xml;
using System.Reflection;
using Generic.Singleton;
using UnityEngine.Events;

public sealed class DecisionTreeLoader : ISingleton
{
    public class TreeInfo
    {
        public string XmlLocalPath;
        public object MethodContainer;
        public Action<DecisionTreeNode> ResultHanlder;
    }

    public readonly MethodReflection MethodReflection;
    public readonly MultiThreadHelper ThreadHelper;

    public readonly Type EvaluatorType;
    public readonly Type ActionType;

    private DecisionTreeLoader()
    {
        MethodReflection = Singleton.Instance<MethodReflection>();
        ThreadHelper = Singleton.Instance<MultiThreadHelper>();

        EvaluatorType = typeof(Func<bool>);
        ActionType = typeof(UnityAction);
    }

    private Func<bool> CreateEvaluatorFunc(XmlNode xmlNode, object target)
    {
        string funcName = xmlNode.InnerText;
        BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

        return MethodReflection.CreateDelegate(EvaluatorType, funcName, target, flags)
            as Func<bool>;
    }

    private UnityAction CreateAction(XmlNode xmlNode, object target)
    {
        string funcName = xmlNode.InnerText;
        BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;

        return MethodReflection.CreateDelegate(ActionType, funcName, target, flags)
            as UnityAction;
    }

    private void Callback(object obj)
    {
        TreeInfo info = obj as TreeInfo;

        try
        {
            // TODO:
            //Debugger.Log(info.XmlLocalPath);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(info.XmlLocalPath);

            DecisionTreeNode tree = CreateNode(xmlDoc.FirstChild, info.MethodContainer);

            // TODO:
            //Debugger.Log("first node: " + xmlDoc.FirstChild.LocalName);
            //Debugger.Log("Loaded bh tree: " + tree + " - doc: " + xmlDoc);

            info.ResultHanlder.Invoke(tree);
        }
        catch(Exception e)
        {
            Debugger.Log(e);
        }
    }

    public void AsyncCreateNode(TreeInfo info)
    {
        MultiThreadHelper.ThreadInvoke(Callback, info);
    }

    public DecisionTreeNode CreateNode(XmlNode node, object target)
    {
        string nodeType = node.LocalName;
        switch (nodeType)
        {
            case "BooleanDecisionNode":
                return new BooleanDecisionNode
                {
                    trueNode = CreateNode(node.FirstChildByAttribute("condition", "true"), target),
                    falseNode = CreateNode(node.FirstChildByAttribute("condition", "false"), target),
                    booleanFunc = CreateEvaluatorFunc(node.GetChildNode("Evaluator"), target)
                };
            case "ActionNode":
                return new ActionNode
                {
                    doAction = CreateAction(node, target)
                };
            case "DecisionTreeNode":
                return CreateNode(node.FirstChild, target);
        }
        return null;
    }
   
    public DecisionTreeNode CreateNode(string xmlPath,object target)
    {
        try
        {
            // TODO:
            //Debugger.Log(xmlPath);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath);

            DecisionTreeNode tree = CreateNode(xmlDoc.FirstChild,target);

            // TODO:
            //Debugger.Log("first node: " + xmlDoc.FirstChild.LocalName);
            //Debugger.Log("Loaded bh tree: " + tree + " - doc: " + xmlDoc);
            return tree;
        }
        catch (Exception e)
        {
            Debugger.Log(e);
            return null;
        }
    }
}
