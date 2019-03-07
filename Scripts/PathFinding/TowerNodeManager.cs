using Generic.Contants;
using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

public sealed class TowerNodeManager
{
    private List<Vector3Int> centers;
    private Dictionary<Vector3Int, NodeInfo> infos;
    private HexMap hexMap;

    public List<Vector3Int> Positions
    {
        get { return centers; }
    }

    public TowerNodeManager()
    {
        centers = new List<Vector3Int>();

        infos = new Dictionary<Vector3Int, NodeInfo>();
        hexMap = Singleton.Instance<HexMap>();
    }

    public void AddRange(Vector3Int center, NodeInfo info, int maxRange)
    {
        for (int range = 1; range <= maxRange; range++)
        {
            Add(center, info, range);
        }
        infos[center] = info;
        if (!centers.Contains(center))
        {
            centers.Add(center);
        }
    }

    public bool GetInfo(Vector3Int pos, out NodeInfo info)
    {
        return infos.TryGetValue(pos, out info);
    }

    public bool IsHolding(Vector3Int pos)
    {
        return infos.ContainsKey(pos);
    }

    public bool Remove(Vector3Int center,int maxRange)
    {
        if (infos.ContainsKey(center))
        {
            for (int range = 1; range <= maxRange; range++)
            {
                RemoveRange(center, range);
            }
            infos.Remove(center);
            centers.Remove(center);
            return true;
        }
        return false;
    }

    private void RemoveRange(Vector3Int center, int range)
    {
        Vector3Int[] pattern = Constants.GetNeighboursRange(center, range);
        for (int i = 0; i < pattern.Length; i++)
        {
            Vector3Int temp = center + pattern[i];
            infos.Remove(temp);
        }
    }

    private void Add(Vector3Int center, NodeInfo info, int range)
    {
        Vector3Int[] pattern = Constants.GetNeighboursRange(center, range);

        for (int i = 0; i < pattern.Length; i++)
        {
            Vector3Int temp = center + pattern[i];
            if (hexMap.IsValidCell(temp.x, temp.y))
            {
                infos[temp] = info;
            }
        }
    }
}