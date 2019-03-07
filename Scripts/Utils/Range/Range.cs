using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

public abstract class Range : MonoBehaviour
{
    [SerializeField] private Transform owner;

    protected Transform Owner
    {
        get { return owner ?? (owner = gameObject.transform); }
        private set { owner = value; }
    }

    protected Queue<NodeInfo> cellInfors;
    protected GlobalNodeManager nodeManager;
    protected AgentNodeManager agentNodeManager;
    protected HexMap hexMap;

    protected virtual void Awake()
    {
        nodeManager = Singleton.Instance<GlobalNodeManager>();
        agentNodeManager = nodeManager.AgentNode;
        hexMap = Singleton.Instance<HexMap>();
    }

    public abstract Queue<NodeInfo> GetInfo();
}
