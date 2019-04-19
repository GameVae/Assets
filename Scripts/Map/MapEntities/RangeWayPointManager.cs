using System.Collections.Generic;
using Map;
using UnityEngine;

public class RangeWayPointManager : INodeManager
{
    private List<Vector3Int> centers;
    private Dictionary<Vector3Int, RangeWayPoint> positions;

    public List<Vector3Int> Centers
    {
        get { return centers ?? (centers = new List<Vector3Int>()); }
    }

    public RangeWayPointManager()
    {
        positions = new Dictionary<Vector3Int, RangeWayPoint>();
    }

    private void AddWayPoint(Vector3Int pos, RangeWayPoint wayPoint)
    {
        Centers.Add(pos);

        positions.Add(pos, wayPoint);
        AddRange(wayPoint);
    }
    private void RemoveWayPoint(Vector3Int pos, RangeWayPoint wayPoint)
    {
        Centers.Remove(pos);

        positions.Remove(pos);
        RemoveRange(wayPoint);
    }

    private void AddRange(RangeWayPoint wayPoint)
    {
        Vector3Int[] bound = wayPoint.Bound();
        for (int i = 0; i < bound.Length; i++)
        {
            if (!positions.ContainsKey(bound[i]))
                positions.Add(bound[i], wayPoint);
        }
    }
    private void RemoveRange(RangeWayPoint wayPoint)
    {

        Vector3Int[] bound = wayPoint.Bound();
        for (int i = 0; i < bound.Length; i++)
        {
            if (positions[bound[i]] == wayPoint)
                positions.Remove(bound[i]);
        }
    }

    /// <summary>
    /// Add way point
    /// </summary>
    /// <param name="wayPoint">type of RangeWayPoint</param>
    /// <returns></returns>
    public bool Add(WayPoint wayPoint)
    {
        Vector3Int pos = wayPoint.Position;
        if (positions.ContainsKey(pos))
        {
            return false;
        }
        AddWayPoint(pos, (RangeWayPoint)wayPoint);
        Debugger.Log(wayPoint.gameObject.name + " add range");
        return true;
    }

    public bool GetInfo(Vector3Int pos, out NodeInfo info)
    {
        info = null;
        if (positions.TryGetValue(pos, out RangeWayPoint wayPoint))
        {
            info = wayPoint.NodeInfo;
            return true;
        }
        return false;
    }

    public bool IsHolding(Vector3Int position)
    {
        return positions.ContainsKey(position);
    }

    public bool Remove(WayPoint wayPoint)
    {
        Vector3Int pos = wayPoint.Position;
        if (positions.ContainsKey(pos))
        {
            if (positions[pos] == wayPoint)
            {
                Debugger.Log(wayPoint.gameObject.name + " remove range");
                RemoveWayPoint(pos, (RangeWayPoint)wayPoint);
                return true;
            }
        }
        return false;
    }
}
