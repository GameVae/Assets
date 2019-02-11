using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3IntExtension
{
    public static Vector3Int ZToZero(this Vector3Int vectorInt)
    {
        vectorInt.z = 0;
        return vectorInt;
    }
}

public static class Vector3Extension
{
    public static Vector3 AddY(this Vector3 vector3,float y)
    {
        vector3.y += y;
        return vector3;
    }
}

public class GreedySearch : MonoBehaviour
{
    public static GreedySearch Instance { get; private set; }
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
    
    private HexMap HexMap;

    public bool IsMoving            { get; private set; }
    public List<Vector3Int> Path    { get; private set; }
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        HexMap = Singleton.Instance<HexMap>();
        Path = new List<Vector3Int>();
    }
    private Vector3Int ToTargetMinCostCell(Vector3Int currentCell, Vector3Int endPos)
    {
        if (endPos == currentCell) return currentCell;
        int minCost = int.MaxValue;
        Vector3Int result = Vector3Int.zero; ;

        currentCell = currentCell.ZToZero();
        endPos = endPos.ZToZero();

        Vector3Int[] neighbours = (currentCell.y % 2) == 0 ? HexaPatternEven1 : HexaPatternOdd1;
        for (int i = 0; i < neighbours.Length; i++)
        {
            int dist = Mathf.RoundToInt(Vector3Int.Distance(currentCell + neighbours[i], endPos));
            if (dist < minCost)
            {
                minCost = dist;
                result = currentCell + neighbours[i];
            }
        }
        return result;
    }

    public bool FindPath(Vector3Int start,Vector3Int end)
    {
        //StartCell = HexGrid.WorldToCell(transform.position).ProjectOnPlan();
        //TargetCell = TargetCell.ProjectOnPlan();
        start   = start.ZToZero();
        end     = end.ZToZero();

        if (!HexMap.IsValidCell(end.x,end.y)) return false;
        Path.Clear();
        Path.Add(start);

        while (Path[Path.Count - 1] != end)
        {
            Path.Add(ToTargetMinCostCell(Path[Path.Count - 1], end));
        }
        return true;
    }  
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Path == null) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < Path.Count; i++)
        {
            Gizmos.DrawSphere(HexMap.CellToWorld(Path[i]), 0.5f);
        }

    }
#endif
}
