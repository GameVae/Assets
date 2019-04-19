using Generic.Contants;
using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class Range1 : Range
{	
    public override Queue<NodeInfo> GetInfo()
    {
        if (cellInfors == null)
            cellInfors = new Queue<NodeInfo>();
        else cellInfors.Clear();

        Vector3Int currentCell = mapIns.WorldToCell(Owner.position).ZToZero();
        Vector3Int[] pattern = Constants.GetNeighboursRange(currentCell, 1);

        for (int i = 0; i < pattern.Length; i++)
        {
            if(agentNodeManager.GetInfo(currentCell + pattern[i],out NodeInfo info))
                cellInfors.Enqueue(info);
        }
        return cellInfors;
    }
}
