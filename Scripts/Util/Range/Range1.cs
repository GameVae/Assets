﻿using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class Range1 : Range
{	
    public override Queue<CellInfomation> GetInfo()
    {
        if (cellInfors == null) cellInfors = new Queue<CellInfomation>();
        cellInfors.Clear();
        Vector3Int currentCell = Singleton.Instance<HexMap>().WorldToCell(Owner.position).ZToZero();
        Vector3Int[] pattern = (currentCell.y % 2 == 0) ? HexaPatternEven1 : HexaPatternOdd1;

        for (int i = 0; i < pattern.Length; i++)
        {
            CellInfomation info = Singleton.Instance<CellInfoManager>().GetCellInfo(currentCell + pattern[i]);
            if (info != null)
                cellInfors.Enqueue(info);
        }
        return cellInfors;
    }
}
