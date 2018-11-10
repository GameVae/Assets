using System.Collections.Generic;
using UnityEngine;

public class AStartAlgorithm : MonoBehaviour
{
    public static AStartAlgorithm Instance { get; private set; }
    private HexMap hexMap;
    private List<HexCell> openCell;
    private List<HexCell> closedCell;
    private List<int> closedIndex;

    public List<Vector3Int> Path { get; protected set; }
    public int MaxLevel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance.gameObject);

        hexMap = GetComponent<HexMap>();
    }
    private void Start()
    {
        Path = new List<Vector3Int>();
        openCell = new List<HexCell>();
        closedCell = new List<HexCell>();
        closedIndex = new List<int>();
    }

    public bool FindPath(Vector3Int pos, Vector3Int target)
    {
        // init value to calculate
        target.z = 0;
        Path.Clear();
        openCell.Clear();
        closedCell.Clear();
        closedIndex.Clear();
        PoolHexCell.Instance.ResetAll();
        // set current cell at 'pos' as origin node
        HexCell startCell = PoolHexCell.Instance.GetCell(pos.x, pos.y);
        startCell.G = 0;
        startCell.H = Vector3Int.Distance(pos, target);
        openCell.Add(startCell);

        bool result = Calculate(startCell, target, 0);
        if (result)
        {
            HexCell targetCell = FindHexCellInClosedList(target);
            Tracking(targetCell, IsObstacle(targetCell));
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning("A STAR TERMINAL:Target not found");
#endif
        }
        return result;
    }

    private bool Calculate(HexCell currentCell, Vector3Int target, int level)
    {
        // termnial : Path not found
        if (openCell.Count == 0 || level > MaxLevel) return false;

        // mask current cell visited
        closedIndex.Add(hexMap.ConvertToIndex(currentCell.X, currentCell.Y));
        closedCell.Add(currentCell);
        openCell.RemoveAt(openCell.IndexOf(currentCell));

        // check goal
        Vector3Int currentPos = new Vector3Int(currentCell.X, currentCell.Y, 0);
        if (currentPos == target) return true;

        // add neigbours to queue
        Vector3Int[] neighbours = hexMap.GetNeigboursPosition(currentPos);
        for (int i = 0; i < neighbours.Length; i++)
        {
            HexCell cell = PoolHexCell.Instance.GetCell(neighbours[i].x, neighbours[i].y);
            if (cell == null)
            {
                //#if UNITY_EDITOR
                //                Debug.Log("Pool empty");
                //#endif
                continue; // pool empty
            }
            int index = hexMap.ConvertToIndex(cell.X, cell.Y);
            if (!closedIndex.Contains(index) && (!IsObstacle(cell) || index == hexMap.ConvertToIndex(target.x, target.y)))
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

    public void Tracking(HexCell cell, bool ignoreThis)
    {
        if (cell == null) return;
        Vector3Int node = new Vector3Int(cell.X, cell.Y, 0);
        if (!ignoreThis)
            Path.Add(node);
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

    private bool IsObstacle(HexCell cell)
    {
        int index = hexMap.ConvertToIndex(cell.X, cell.Y);
        return ObstacleManager.Instance.Contain(index);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Path != null)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < Path.Count; i++)
            {
                Gizmos.DrawSphere(hexMap.CellToWorld(Path[i]), 0.5f);
            }
        }
    }
#endif
}
