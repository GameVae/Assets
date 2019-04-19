﻿using Map;
using UnityEngine;

public interface INodeManager
{
    bool Add(WayPoint wayPoint);
    bool Remove(WayPoint wayPoint);
    bool IsHolding(Vector3Int position);
    bool GetInfo(Vector3Int pos, out NodeInfo info);
}
