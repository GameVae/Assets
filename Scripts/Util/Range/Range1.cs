using Generic.Contants;
using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class Range1 : Range
{	
    public override Queue<CellInfo> GetInfo()
    {
        if (cellInfors == null)
            cellInfors = new Queue<CellInfo>();
        else cellInfors.Clear();

        Vector3Int currentCell = Singleton.Instance<HexMap>().WorldToCell(Owner.position).ZToZero();
        Vector3Int[] pattern = GConstants.GetNeighboursRange(currentCell, 1);

        CellInfo def = default(CellInfo);
        for (int i = 0; i < pattern.Length; i++)
        {
            CellInfo info = Singleton.Instance<CellInfoManager>().GetCellInfo(currentCell + pattern[i]);
            if (info != def)
                cellInfors.Enqueue(info);
        }
        return cellInfors;
    }
}
