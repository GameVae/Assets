using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class AgentNodeManager 
{
    private Dictionary<Vector3Int, NodeInfo> infos;

    public AgentNodeManager()
    {
        infos = new Dictionary<Vector3Int, NodeInfo>();
    }

    public bool Add(Vector3Int pos, NodeInfo info)
    {
        if (IsHolding(pos))
            return false;
        infos[pos] = info;
        return true;
    }

    public bool GetInfo(Vector3Int pos, out NodeInfo info)
    {
        return infos.TryGetValue(pos, out info);
    }

    public bool IsHolding(Vector3Int pos)
    {
        return infos.ContainsKey(pos);
    }

    public bool Remove(Vector3Int pos)
    {
        return infos.Remove(pos);
    }
}
