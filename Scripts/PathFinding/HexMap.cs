using Generic.Contants;
using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

public sealed class HexMap : MonoSingle<HexMap>
{
    public Grid HexGrid;

    public const int TotalCol = GConstants.TOTAL_COL;
    public const int TotalRow = GConstants.TOTAL_ROW;

    public int ConvertToIndex(int x, int y)
    {
        return y * TotalCol + x;
    }

    public Vector3Int ConvertToVector3Int(int index)
    {
        Vector3Int result = Vector3Int.zero;
        result.x = index % TotalCol;
        result.y = index / TotalCol;

        if (IsValidCell(result.x, result.y)) return Vector3Int.one * -1;
        return result;
    }

    public Vector3Int[] GetNeighbours(Vector3Int cell)
    {
        #region old
        //List<Vector3Int> neighbours = new List<Vector3Int>();
        //Vector3Int neighbour;
        //Vector3Int[] pattern = (cell.y % 2 == 0) ? HexaPatternEven1 : HexaPatternOdd1;
        //for (int i = 0; i < pattern.Length; i++)
        //{
        //    neighbour = pattern[i] + cell;
        //    if (IsValidCell(neighbour.x, neighbour.y))
        //    {
        //        neighbours.Add(neighbour);
        //    }
        //}
        //return neighbours.ToArray();
        #endregion

        return GConstants.GetNeighboursRange(cell, 1);
    }

    #region Not use
    public Vector3Int[] GetNeighboursEmpty(Vector3Int cell)
    {
        List<Vector3Int> neighbours = new List<Vector3Int>();
        Vector3Int neighbour;
        Vector3Int[] pattern = (cell.y % 2 == 0) ?
            GConstants.NeighbourHexCell.HexaPatternEven1 : GConstants.NeighbourHexCell.HexaPatternOdd1;
        for (int i = 0; i < pattern.Length; i++)
        {
            neighbour = pattern[i] + cell;
            if (IsValidCell(neighbour.x, neighbour.y) &&
                !Singleton.Instance<CellInfoManager>().ContainsKey(neighbour.ZToZero()))
            {
                neighbours.Add(neighbour);
            }
        }
        return neighbours.ToArray();
    }

    public bool IsValidCell(int x, int y)
    {
        return GConstants.IsValidCell(x, y);
    }
    #endregion

    public Vector3 CellToWorld(Vector3Int cell)
    {
        return HexGrid.CellToWorld(cell);
    }

    public Vector3Int WorldToCell(Vector3 position)
    {
        if (HexGrid != null)
            return HexGrid.WorldToCell(position);
        return Vector3Int.zero;
    }
}
