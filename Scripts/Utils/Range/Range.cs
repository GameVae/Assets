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
    protected SingleWayPointManager agentNodeManager;
    protected HexMap mapIns;

    protected virtual void Awake()
    {
        mapIns = Singleton.Instance<HexMap>();
        agentNodeManager = mapIns.AgentNodeManager;
    }

    public abstract Queue<NodeInfo> GetInfo();
}
