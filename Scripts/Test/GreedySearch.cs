using System.Collections.Generic;
using UnityEngine;

public static class Vector3IntExtension
{
    public static Vector3Int ProjectOnPlan(this Vector3Int vectorInt)
    {
        vectorInt.z = 0;
        return vectorInt;
    }
}
public class GreedySearch : MonoBehaviour
{
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

    private Vector3Int CurrentCell;
    private List<Vector3Int> trackingList = new List<Vector3Int>();
    private float timeCounter = 0.0f;

    public float Speed;
    public Grid HexGrid;
    public Vector3Int TargetCell;
    public Camera CameraRaycaster;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 mousePos = Input.mousePosition;

            bool raycastHitted = Physics.Raycast(
                CameraRaycaster.ScreenPointToRay(mousePos),
                out RaycastHit hitInfo,
                int.MaxValue);
            if (raycastHitted)
            {
                TargetCell = HexGrid.WorldToCell(hitInfo.point);
                FindPath();
            }
        }

        MoveToTarget();
    }
    private Vector3Int ToTargetMinCostCell(Vector3Int cell, Vector3Int target)
    {
        if (target == cell) return cell;
        int minCost = int.MaxValue;
        Vector3Int result = Vector3Int.zero; ;

        cell = cell.ProjectOnPlan();
        target = target.ProjectOnPlan();

        Vector3Int[] neighbours = (cell.y % 2) == 0 ? HexaPatternEven1 : HexaPatternOdd1;
        for (int i = 0; i < neighbours.Length; i++)
        {
            int dist = Mathf.RoundToInt(Vector3Int.Distance(cell + neighbours[i], target));
            if (dist < minCost)
            {
                minCost = dist;
                result = cell + neighbours[i];
            }
        }
        return result;
    }

    [ContextMenu("Find Path")]
    public void FindPath()
    {
        CurrentCell = HexGrid.WorldToCell(transform.position).ProjectOnPlan();
        TargetCell = TargetCell.ProjectOnPlan();
        trackingList.Clear();
        trackingList.Add(CurrentCell);
        while (trackingList[trackingList.Count - 1] != TargetCell)
        {
            trackingList.Add(ToTargetMinCostCell(trackingList[trackingList.Count - 1], TargetCell));
        }
    }
  
    private void MoveToTarget()
    {
        if (trackingList.Count > 0)
        {
            Vector3 currentWayPoint = HexGrid.CellToWorld(trackingList[0]);
            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, Time.deltaTime * Speed);
            timeCounter += Time.deltaTime;
            if ((currentWayPoint - transform.position).magnitude <= 0.1f)
            {
                Debug.Log(timeCounter);
                timeCounter = 0.0f;
                trackingList.RemoveAt(0);
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (trackingList == null) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < trackingList.Count; i++)
        {
            Gizmos.DrawSphere(HexGrid.CellToWorld(trackingList[i]), 0.5f);
        }

    }
#endif
}
