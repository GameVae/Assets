using System.Collections.Generic;
using UnityEngine;

public class HexMap : MonoBehaviour
{
    public static HexMap Instance { get; private set; }
    private readonly Vector3Int[] HexaPatternEven1 = new Vector3Int[]
{
        new Vector3Int( 0,-1, 0),
        new Vector3Int( 0, 1, 0),
        new Vector3Int(-1,-1, 0),
        new Vector3Int(-1, 1, 0),
        new Vector3Int( 1, 0, 0),
        new Vector3Int(-1, 0, 0),
};
    private readonly Vector3Int[] HexaPatternOdd1 = new Vector3Int[]
{
        new Vector3Int( 1,-1, 0),
        new Vector3Int( 1, 1, 0),
        new Vector3Int( 0,-1, 0),
        new Vector3Int( 0, 1, 0),
        new Vector3Int( 1, 0, 0),
        new Vector3Int(-1, 0, 0),
};

    public Grid HexGrid;
    public int TotalCol;
    public int TotalRow;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance.gameObject);
    }

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
        List<Vector3Int> neighbours = new List<Vector3Int>();
        Vector3Int neighbour;
        Vector3Int[] pattern = (cell.y % 2 == 0) ? HexaPatternEven1 : HexaPatternOdd1;
        for (int i = 0; i < pattern.Length; i++)
        {
            neighbour = pattern[i] + cell;
            if (IsValidCell(neighbour.x, neighbour.y))
            {
                neighbours.Add(neighbour);
            }
        }
        return neighbours.ToArray();
    }

    public Vector3Int[] GetNeighboursEmpty(Vector3Int cell)
    {
        List<Vector3Int> neighbours = new List<Vector3Int>();
        Vector3Int neighbour;
        Vector3Int[] pattern = (cell.y % 2 == 0) ? HexaPatternEven1 : HexaPatternOdd1;
        for (int i = 0; i < pattern.Length; i++)
        {
            neighbour = pattern[i] + cell;
            if (IsValidCell(neighbour.x, neighbour.y) &&
                !CellInfoManager.Instance.ContainsKey(neighbour.ZToZero()))
            {
                neighbours.Add(neighbour);
            }
        }
        return neighbours.ToArray();
    }

    public bool IsValidCell(int x, int y)
    {
        return x >= 5 && x <= TotalCol - 5 && y >= 5 && y <= TotalRow - 5;
    }

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
