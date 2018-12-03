using System.Collections.Generic;
using UnityEngine;

public class AStartAlgorithm
{
    private List<HexCell> openCell;
    private List<HexCell> closedCell;
    private List<int> closedIndex;

    public int MaxLevel { get; set; }
    public HexMap HexMap { get; set; }
    public List<Vector3Int> Path { get; protected set; }

    public AStartAlgorithm()
    {
        Path = new List<Vector3Int>();
        openCell = new List<HexCell>();
        closedCell = new List<HexCell>();
        closedIndex = new List<int>();
    }

    public bool FindPath(Vector3Int start, Vector3Int end)
    {
        // init value to calculate
        Clear();

        // set current cell at 'pos' as origin node
        HexCell startCell = PoolHexCell.Instance.CreateCell(start.x, start.y);
        startCell.G = 0;
        startCell.H = Vector3Int.Distance(start, end);
        openCell.Add(startCell);

        bool result = Calculate(startCell, end, 0);
        if (result)
        {
            HexCell targetCell = FindHexCellInClosedList(end);
            Tracking(targetCell, false);
        }
        return result;
    }

    private bool Calculate(HexCell currentCell, Vector3Int target, int level)
    {
        // termnial : Path not found
        if (openCell.Count == 0 || level > MaxLevel) return false;

        // store for tracking
        closedCell.Add(currentCell);
        // mask current cell visited
        closedIndex.Add(HexMap.ConvertToIndex(currentCell.X, currentCell.Y));
        openCell.RemoveAt(openCell.IndexOf(currentCell));

        // check goal
        Vector3Int currentPos = new Vector3Int(currentCell.X, currentCell.Y, 0);
        if (currentPos == target) return true;

        // add neigbours to queue
        Vector3Int[] neighbours = HexMap.GetNeighbours(currentPos);
        for (int i = 0; i < neighbours.Length; i++)
        {
            HexCell cell = PoolHexCell.Instance.CreateCell(neighbours[i].x, neighbours[i].y);
            if (cell == null)
            {
                continue; // pool empty
            }
            if (!closedIndex.Contains(HexMap.ConvertToIndex(cell.X,cell.Y)))
            {
                cell.G = currentCell.G + 1;
                cell.H = Vector3Int.Distance(neighbours[i], target);
                cell.Parent = currentCell;
                openCell.Add(cell);
            }
            else
            {
                PoolHexCell.Instance.Reset(cell);
            }
        }

        // find next node has min F
        HexCell nextVisit = openCell.Count > 0 ? openCell[0] : null;
        for (int i = 1; i < openCell.Count; i++)
        {
            HexCell checkingCell = openCell[i];
            if (checkingCell.F <= nextVisit.F)
            {
                if (checkingCell.H < nextVisit.H)
                {
                    nextVisit = checkingCell;
                }
            }
        }
        return Calculate(nextVisit, target, level + 1);
    }

    public void Tracking(HexCell cell, bool ignoreTarget)
    {
        if (cell == null) return;
        Vector3Int node = new Vector3Int(cell.X, cell.Y, 0);

        // ignore target node //  start node
        if (!ignoreTarget && cell.Parent != null)
        {
            Path.Add(node);
        }
        Tracking(cell.Parent, false);
    }

    private HexCell FindHexCellInClosedList(Vector3Int pos)
    {
        for (int i = 0; i < closedCell.Count; i++)
        {
            if (closedCell[i].X == pos.x && closedCell[i].Y == pos.y)
                return closedCell[i];
        }
        return null;
    }

    private void Clear()
    {
        Path.Clear();
        openCell.Clear();
        closedCell.Clear();
        closedIndex.Clear();
        PoolHexCell.Instance.ResetAll();
    }
}
