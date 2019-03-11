using System;
using System.Collections.Generic;
using System.Diagnostics;
using Animation;
using EnumCollect;
using Generic.Contants;
using Generic.Singleton;
using ManualTable.Row;
using Map;
using PathFinding;
using UnityEngine;


[RequireComponent(typeof(LineRenderer), typeof(WayPoint))]
public class NavAgent : MonoBehaviour
{
    private NavRemote remote;

    private int curMoveStep;
    private HexMap mapIns;
    private AStartAlgorithm aStar;
    private List<Vector3Int> path;
    private BreathFirstSearch breathFirstSearch;

    private NavPathRenderer pathRenderer;

    public MovementOffset Offset;

    private int maxSearchLevel;
    private int maxMoveStep;
    private float speed;

    private FixedMovement fixedMove;

    public WayPoint WayPoint { get; private set; }

    public bool IsMoving { get; private set; }

    public Vector3Int EndPosition { get; private set; }
    public Vector3Int StartPosition { get; private set; }
    public Vector3Int CurrentPosition
    {
        get
        {
            if (mapIns != null)
                return mapIns.WorldToCell(transform.position);
            return Vector3Int.zero;
        }
    }

    public ListUpgrade Type
    {

        get { return remote.Type; }
    }

    public AnimatorController Anim;

    #region Initalize
    private void Awake()
    {
        // LineRendererInit();
        pathRenderer = new NavPathRenderer(GetComponent<LineRenderer>());

        OffsetInit();

        remote = GetComponent<NavRemote>();
    }

    private void OffsetInit()
    {
        maxMoveStep = Offset.MaxMoveStep;
        speed = Offset.MaxSpeed;
        maxSearchLevel = Offset.MaxSearchLevel;
    }

    private void Start()
    {
        mapIns = Singleton.Instance<HexMap>();
        WayPoint = GetComponent<WayPoint>();
        breathFirstSearch = Singleton.Instance<BreathFirstSearch>();

        aStar = new AStartAlgorithm(mapIns, maxSearchLevel);
        fixedMove = new FixedMovement(this, Anim);

        MoveFinish();
    }
    #endregion

    private void Update()
    {
        //print("delta time: " + Time.deltaTime);
        MoveToTarget();

        fixedMove.Update();

        pathRenderer.Update();
    }

    private void AStarMoveToTarget()
    {
        if (IsMoving)
        {
            if (path.Count > 0)
            {
                Vector3 currentTarget = mapIns.CellToWorld(path[path.Count - 1]);
                transform.position = Vector3.MoveTowards(
                    current: transform.position,
                    target: currentTarget,
                    maxDistanceDelta: Time.deltaTime * speed);
                RotateToTarget(currentTarget);

                if ((transform.position - currentTarget).magnitude <= Constants.TINY_VALUE)
                {
                    path.RemoveAt(path.Count - 1);
                    curMoveStep++;

                    pathRenderer.RemoveForwardPoint(path.Count, GetPerHasGone());
                    if (maxMoveStep == curMoveStep) pathRenderer.Clear();
                }
            }
            else
            {
                IsMoving = false;
                MoveFinish();
                Anim.Stop(AnimState.Walking);
            }
        }
    }

    private void MoveToTarget()
    {
        if (maxMoveStep - 1 < curMoveStep || (path != null && path.Count == 0))
        {
            path?.Clear();
        }
        AStarMoveToTarget();
    }

    public float GetPerHasGone()
    {
        int count = path.Count;
        if (count == 0) return 1;
        return (count - (maxMoveStep - curMoveStep)) * 1.0f / count;
    }

    private bool FindPath(Vector3Int start, Vector3Int end)
    {
        StartPosition = start;
        EndPosition = end;
        if (curMoveStep >= maxMoveStep || (path != null && path.Count == 0))
        {
            curMoveStep = 0;
        }

        IsMoving = aStar.FindPath(StartPosition.ZToZero(), EndPosition.ZToZero());
        path = aStar.Path;

        pathRenderer.LineRendererGenPath(
            foundPath: IsMoving,
            worldPath: mapIns.CellToWorld(path),
            t: GetPerHasGone(), // (count - (maxMoveStep - CurrentMoveStep)) * 1.0f / count
            position: transform.position,
            target: mapIns.CellToWorld(EndPosition)
            );
        return IsMoving;
    }

    private void MoveFinish()
    {
        Vector3Int currentCell = mapIns.WorldToCell(transform.position).ZToZero();
        if (!WayPoint.Binding())
        {
            if (breathFirstSearch.GetNearestCell(currentCell, out Vector3Int result))
            {
                FindPath(currentCell, result);
                curMoveStep = 0;
            }
        }
    }

    public bool StartMove(Vector3Int start, Vector3Int end)
    {
        if (IsMoving == false)
        {
            WayPoint.Unbinding();
        }
        bool foundPath = FindPath(start, end);
        if (foundPath)
        {

            Anim.Play(AnimState.Walking);

        }
        return foundPath;
    }

    public void StartMove(JSONObject r_move)
    {
        fixedMove.StartMove(r_move);
    }

    public List<float> GetTime()
    {
        return Offset.GetTime(GetMovePath(), transform.position);
    }


    public List<Vector3Int> GetMovePath()
    {
        return Offset.TruncatePath(path);
    }

    #region Dead & On destroy
    private void Dead()
    {
        if (!IsMoving)
        {
            WayPoint.Unbinding();
        }
    }

    private void OnDestroy()
    {
        Dead();
    }
    #endregion

    #region Animation
    public float maxAngular = 5;
    public void RotateToTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position) * maxAngular;
        transform.forward += direction * Time.deltaTime;
    }
    #endregion

}