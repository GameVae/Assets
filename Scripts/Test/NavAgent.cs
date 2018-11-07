using System.Collections.Generic;
using UnityEngine;

public class NavAgent : MonoBehaviour
{
    public enum PathFindingType
    {
        AStar,
        Greedy,
    }

    private int numStep;
    private HexMap HexMap;
    private AStartAlgorithm AStarCalculator;
    private GreedySearch GreedyCalculator;
    private List<Vector3Int> path;

    public int MaxNumStep;
    public float Speed;
    public Camera CameraRaycaster;

    public PathFindingType SearchType;
    public Vector3Int StartCell { get; private set; }
    public Vector3Int EndCell   { get; private set; }
    public bool IsMoving        { get; private set; }

    public bool IsComparePath   { get; set; }
    public bool IsAutoMove      { get; set; }

    private void Start()
    {
        HexMap = HexMap.Instance;
        AStarCalculator = AStartAlgorithm.Instance;
        GreedyCalculator = GreedySearch.Instance;

        IsAutoMove = true;
        IsComparePath = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            bool raycastHitted = Physics.Raycast(
                CameraRaycaster.ScreenPointToRay(mousePos),
                out RaycastHit hitInfo,
                int.MaxValue);

            if (raycastHitted)
            {
                Vector3Int selectCell = HexMap.WorldToCell(hitInfo.point);
                if (!HexMap.IsValidCell(selectCell.x, selectCell.y)) return;

                EndCell = selectCell;
                StartCell = HexMap.WorldToCell(transform.position);

                FindPath();
            }
        }

        if (IsAutoMove)
        {
            MoveToTarget(SearchType);
        }
    }

    private void FindPath()
    {
        numStep = 0;
        switch (SearchType)
        {
            case PathFindingType.AStar:
                IsMoving = AStarCalculator.FindPath(StartCell, EndCell);
                path = AStarCalculator.Path;

                if (IsComparePath) GreedyCalculator.FindPath(StartCell, EndCell);
                break;
            case PathFindingType.Greedy:
                IsMoving = GreedyCalculator.FindPath(StartCell, EndCell);
                path = GreedyCalculator.Path;

                if (IsComparePath) AStarCalculator.FindPath(StartCell, EndCell);
                break;
        }
    }

    private void AStarMoveToTarget()
    {
        if (IsMoving)
        {
            if (path.Count > 0)
            {
                Vector3 currentTarget = HexMap.CellToWorld(path[path.Count - 1]);
                transform.position = Vector3.MoveTowards(
                    current: transform.position,
                    target: currentTarget,
                    maxDistanceDelta: Time.deltaTime * Speed);
                if ((transform.position - currentTarget).magnitude <= 0.1f)
                {
                    path.RemoveAt(path.Count - 1);
                    numStep++;
                    Debug.Log("Step: " + numStep);
                }
            }
            else
            {
                IsMoving = false;
            }
        }
    }

    private void GreedyMoveToTarget()
    {        
        if (IsMoving)
        {
            if (path.Count > 0)
            {
                Vector3 currentWayPoint = HexMap.CellToWorld(path[0]);
                transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, Time.deltaTime * Speed);
                if ((currentWayPoint - transform.position).magnitude <= 0.1f)
                {
                    path.RemoveAt(0);
                    numStep++;
                    Debug.Log("Step: " + numStep);
                }
            }
            else
            {
                IsMoving = false;
            }
        }
    }

    private void MoveToTarget(PathFindingType type)
    {
        if (numStep >= MaxNumStep) path.Clear();
        switch (type)
        {
            case PathFindingType.AStar:
                AStarMoveToTarget();
                break;
            case PathFindingType.Greedy:
                GreedyMoveToTarget();
                break;
        }
    }

}