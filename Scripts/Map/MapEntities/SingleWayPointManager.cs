using Map;
using System.Collections.Generic;
using UnityEngine;

public class SingleWayPointManager : INodeManager
{
    private Dictionary<Vector3Int, SingleWayPoint> wayPoints;

    public SingleWayPointManager()
    {
        wayPoints = new Dictionary<Vector3Int, SingleWayPoint>();
    }

    private bool Add(SingleWayPoint wayPoint)
    {
        Vector3Int pos = wayPoint.Position;

        if (IsHolding(pos))
            return false;
        wayPoints.Add(pos, wayPoint);
        Debugger.Log(wayPoint.gameObject.name + " add single");
        return true;
    }

    /// <summary>
    /// Add Single Way Poit
    /// </summary>
    /// <param name="wayPoint">type of SingleWauPoint</param>
    /// <returns></returns>
    public bool Add(WayPoint wayPoint)
    {
        return Add((SingleWayPoint)wayPoint);
    }

    public bool GetInfo(Vector3Int pos, out NodeInfo info)
    {
        if (wayPoints.TryGetValue(pos, out SingleWayPoint wayPoint))
        {
            info = wayPoint.NodeInfo;
            return true;
        }
        info = null;
        return false;
    }

    public bool IsHolding(Vector3Int pos)
    {
        return wayPoints.ContainsKey(pos);
    }

    public bool Remove(WayPoint wayPoint)
    {
        Vector3Int pos = wayPoint.Position;
        if (wayPoints.ContainsKey(pos))
        {
            if (wayPoints[pos] == wayPoint)
            {
                Debugger.Log(wayPoint.gameObject.name + " remove single");
                return wayPoints.Remove(wayPoint.Position);
            }
        }
        return false;
    }
}
