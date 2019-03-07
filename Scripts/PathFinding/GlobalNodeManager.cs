using Generic.Contants;
using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class GlobalNodeManager : ISingleton
{
    private static int Id = 0;
    public static int ID()
    {
        return Id++;
    }

    private TowerNodeManager towerNode;
    private AgentNodeManager agentNode;
    public TowerNodeManager TowerNode
    {
        get { return towerNode; }
    }
    public AgentNodeManager AgentNode
    {
        get { return agentNode; }
    }

    public List<Vector3Int> TowerPositions
    {
        get { return TowerNode.Positions; }
    }

    private GlobalNodeManager()
    {
        agentNode = new AgentNodeManager();
        towerNode = new TowerNodeManager();
    }

    #region old
    ///// <summary>
    ///// For dynamic objects
    ///// </summary>
    ///// <param name="key"></param>
    ///// <param name="value"></param>
    ///// <returns></returns>
    //public bool AddInfo(NodeType nodeType,Vector3Int key, NodeInfo value)
    //{
    //    if(nodeDict.TryGetValue(nodeType, out TowerNodeManager manager))
    //    {
    //        if(manager.IsHolding(key))
    //        {
    //            return false;
    //        }
    //        manager.AddInfo(key, value);
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    //public void AddRange(NodeType nodeType,Vector3Int pos, NodeInfo info, int range)
    //{
    //    if (nodeDict.TryGetValue(nodeType, out TowerNodeManager manager))
    //    {
    //        manager.AddRange(pos, info, range);
    //    }
    //}

    ///// <summary>
    ///// for dynamic objects
    ///// </summary>
    ///// <param name="key"></param>
    ///// <returns></returns>
    //public bool RemoveInfo(NodeType nodeType, Vector3Int key)
    //{
    //    if (nodeDict.TryGetValue(nodeType, out TowerNodeManager manager))
    //    {
    //        return manager.Remove(key);
    //    }
    //    return false;
    //}

    ///// <summary>
    ///// Ignore tower
    ///// </summary>
    ///// <param name="key"></param>
    ///// <returns></returns>
    //public bool IsEmptyAt(NodeType nodeType,Vector3Int key)
    //{
    //    if (nodeDict.TryGetValue(nodeType, out TowerNodeManager manager))
    //    {
    //        return !manager.IsHolding(key);
    //    }
    //    return false;
    //}

    //public bool GetInfo(NodeType nodeType,Vector3Int key, out NodeInfo result)
    //{
    //    if (nodeDict.TryGetValue(nodeType, out TowerNodeManager manager))
    //    {
    //        return manager.GetInfo(key,out result);
    //    }
    //    result = default(NodeInfo);
    //    return false;
    //}

    //public NodeInfo GetInfo(NodeType nodeType,Vector3Int key)
    //{
    //    if (nodeDict.TryGetValue(nodeType, out TowerNodeManager manager))
    //    {
    //        manager.GetInfo(key, out NodeInfo result);
    //        return result;
    //    }
    //    else
    //    {
    //        return default(NodeInfo);
    //    }
    //}
    #endregion
}
