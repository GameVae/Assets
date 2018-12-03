using System.Collections.Generic;
using UnityEngine;

public class Range3 : Range
{
    public override Queue<CellInfomation> GetInfo()
    {
        if (cellInfors == null) cellInfors = new Queue<CellInfomation>();
        cellInfors.Clear();
        Vector3Int currentCell = HexMap.Instance.WorldToCell(transform.position);
        Vector3Int[] pattern = (currentCell.y % 2 == 0) ? HexaPatternEven3 : HexaPatternOdd3;

        for (int i = 0; i < pattern.Length; i++)
        {
            CellInfomation info = CellInfoManager.Instance.GetCellInfo(currentCell + pattern[i]);
            if (info != null)
                cellInfors.Enqueue(info);
        }
        return cellInfors;
    }
}