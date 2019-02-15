using System.Collections.Generic;
using System.Diagnostics;
using Animation;
using EnumCollect;
using Generic.Contants;
using Generic.Singleton;
using Map;
using PathFinding;
using UnityEngine;


[RequireComponent(typeof(LineRenderer), typeof(WayPoint))]
public class NavAgent : MonoBehaviour
{
    private bool isLineAnimation;
    private float animLineCounter;

    private HexMap mapIns;
    private AStartAlgorithm aStar;
    private List<Vector3Int> path;
    private BreathFirstSearch breathFirstSearch;

    private Gradient colors;
    private GradientColorKey[] graColorKeys;
    private GradientAlphaKey[] graAlKeys;
    private LineRenderer movingLineRenderer;

    public MoveOffset Offset;

    [SerializeField] private int maxSearchLevel;
    private int maxMoveStep;
    private float speed;

    public List<Vector3Int> OffsetMovement;
    public int CurrentOffsetStep;
    public bool IsOffsetMoving { get; private set; }
    public WayPoint WayPoint { get; private set; }

    public int CurrentMoveStep { get; private set; }
    public bool IsMoving { get; private set; }

    public Vector3Int EndCell { get; private set; }
    public Vector3Int StartCell { get; private set; }
    public Vector3Int CurrentCell
    {
        get
        {
            if (mapIns != null)
                return mapIns.WorldToCell(transform.position);
            return Vector3Int.zero;
        }
    }


    public AnimatorController Anim;

    #region Initalize
    private void Awake()
    {
        LineRendererInit();

        OffsetInit();
    }

    private void LineRendererInit()
    {

        movingLineRenderer = GetComponent<LineRenderer>();
        movingLineRenderer.allowOcclusionWhenDynamic = true;

        graAlKeys = new GradientAlphaKey[2];
        graColorKeys = new GradientColorKey[2];

        graColorKeys[0].color = Color.red;
        graColorKeys[0].time = 0.0f;
        graColorKeys[1].color = Color.blue;
        graColorKeys[1].time = 1.0f;

        graAlKeys[0].alpha = 100.0f;
        graAlKeys[0].time = 0.0f;
        graAlKeys[1].alpha = 100.0f;
        graAlKeys[1].time = 1.0f;

        colors = new Gradient()
        {
            mode = GradientMode.Fixed,
            alphaKeys = graAlKeys,
            colorKeys = graColorKeys
        };
    }

    private void OffsetInit()
    {
        maxMoveStep = Offset.MaxMoveStep;
    }

    private void Start()
    {
        mapIns = Singleton.Instance<HexMap>();
        WayPoint = GetComponent<WayPoint>();
        breathFirstSearch = Singleton.Instance<BreathFirstSearch>();

        aStar = new AStartAlgorithm()
        {
            HexMap = mapIns,
            MaxLevel = maxSearchLevel,
        };

        MoveFinish();
    }
    #endregion

    private void Update()
    {
        //print("delta time: " + Time.deltaTime);
        MoveToTarget();

        MoveFollowOffset();

        AnimationLine();
    }

    private void InitLineRenderer()
    {
        if (IsMoving)
        {
            RefreshLineRenderer();
            CalculateGradiantColor();
        }
        else
        {
            DrawNotFoundPath();
        }
    }

    private float elapsedTime = 0.0f;
    private void AStarMoveToTarget()
    {
        if (IsMoving)
        {
            elapsedTime += Time.deltaTime;
            if (path.Count > 0)
            {
                Vector3 currentTarget = mapIns.CellToWorld(path[path.Count - 1]);
                transform.position = Vector3.MoveTowards(
                    current: transform.position,
                    target: currentTarget,
                    maxDistanceDelta: Time.deltaTime * speed);
                LookAtTarget(currentTarget);

                if ((transform.position - currentTarget).magnitude <= GConstants.TINY_VALUE)
                {

                    // print("elapsed: " + elapsedTime + "- elapsed: " + stopwatch.Elapsed + " reached: " + path[path.Count - 1]);
                    elapsedTime = 0.0f;

                    path.RemoveAt(path.Count - 1);
                    RemoveLineRendererWayPoint();

                }
            }
            else
            {
                elapsedTime = 0.0f;
                IsMoving = false;
                MoveFinish();
                Anim.Stop(AnimState.Walking);
            }
        }
    }

    private void MoveToTarget()
    {
        if (maxMoveStep - 1 < CurrentMoveStep || (path != null && path.Count == 0))
        {
            path?.Clear();
        }
        AStarMoveToTarget();
    }

    #region Draw Line
    private void RefreshLineRenderer()
    {
        if (path != null)
        {
            int count = path.Count;
            movingLineRenderer.positionCount = count;
            for (int i = 0; i < count; i++)
            {
                movingLineRenderer.SetPosition(count - i - 1, mapIns.CellToWorld(path[count - i - 1]).AddY(0.2f));
            }
        }
    }

    private void RemoveLineRendererWayPoint()
    {
        CurrentMoveStep++;
        CalculateGradiantColor();
        if (CurrentMoveStep >= maxMoveStep)
        {
            movingLineRenderer.positionCount = 0;
            return;
        }
        int count = movingLineRenderer.positionCount;
        movingLineRenderer.positionCount = (count > 0) ? count - 1 : 0;
    }

    private void CalculateGradiantColor()
    {
        if (path.Count != 0)
        {
            float time = (path.Count - (maxMoveStep - CurrentMoveStep)) * 1.0f / path.Count;
            graColorKeys[0].time = time;
            graAlKeys[0].time = graColorKeys[0].time;
            if (time <= 0)
            {
                graColorKeys[0].color = Color.blue;
            }
            else
            {
                graColorKeys[0].color = Color.red;
            }
        }
        else
        {
            graColorKeys[0].time = 1.0f;
            graColorKeys[0].color = Color.red;
            graAlKeys[0].time = graColorKeys[0].time;
        }

        colors.SetKeys(graColorKeys, graAlKeys);
        movingLineRenderer.colorGradient = colors;
    }

    private void DrawNotFoundPath()
    {
        movingLineRenderer.positionCount = 2;
        movingLineRenderer.SetPosition(0, transform.position);
        movingLineRenderer.SetPosition(1, mapIns.CellToWorld(EndCell));
        CalculateGradiantColor();
        isLineAnimation = true;
    }

    private void AnimationLine()
    {
        if (isLineAnimation)
        {
            animLineCounter += Time.deltaTime;
            if (animLineCounter >= 0.25f)
            {
                isLineAnimation = false;
                animLineCounter = 0.0f;
                movingLineRenderer.positionCount = 0;
            }
        }
    }
    #endregion

    private bool FindPath(Vector3Int start, Vector3Int end)
    {
        StartCell = start;
        EndCell = end;
        if (CurrentMoveStep >= maxMoveStep || (path != null && path.Count == 0))
        {
            CurrentMoveStep = 0;
        }

        IsMoving = aStar.FindPath(StartCell.ZToZero(), EndCell.ZToZero());
        path = aStar.Path;
        InitLineRenderer();
        return IsMoving;
    }

    private void MoveFinish()
    {
        stopwatch.Stop();
        //print("elapsed time: " + stopwatch.Elapsed);

        Vector3Int currentCell = mapIns.WorldToCell(transform.position).ZToZero();
        if (!WayPoint.Binding())
        {
            if (breathFirstSearch.GetNearestCell(currentCell, out Vector3Int result))
            {
                FindPath(currentCell, result);
                CurrentMoveStep = 0;
            }
        }
    }

    private Stopwatch stopwatch = new Stopwatch();
    public bool StartMove(Vector3Int start, Vector3Int end)
    {
        if (IsMoving == false)
        {
            WayPoint.Unbinding();
        }
        bool foundPath = FindPath(start, end);
        if (foundPath)
        {
            speed = Offset.GetSpeed(path, transform.position);
            // print("distance: " + " speed: " + speed + " total time: " + path.Count * Offset.AverageMoveTime);
            stopwatch.Restart();

            Anim.Play(AnimState.Walking);

        }
        return foundPath;
    }

    public List<Vector3Int> GetMovePath()
    {
        return Offset.TruncatePath(path);
    }

    #region Move follow offset
    private void MoveFollowOffset()
    {
        if (IsOffsetMoving)
        {
            if (CurrentOffsetStep < OffsetMovement.Count)
            {
                Vector3 currentTarget = mapIns.CellToWorld(OffsetMovement[CurrentOffsetStep]);
                transform.position = Vector3.MoveTowards(
                    current: transform.position,
                    target: currentTarget,
                    maxDistanceDelta: Time.deltaTime * speed);

                if ((transform.position - currentTarget).magnitude <= GConstants.TINY_VALUE)
                {
                    CurrentOffsetStep++;
                }
            }
            else
            {
                IsOffsetMoving = false;
                MoveFinish();
            }
        }
    }

    public void StartOffsetMove()
    {
        CurrentOffsetStep = 0;
        IsOffsetMoving = true;
        if (OffsetMovement.Count > 0)
        {
            WayPoint.Unbinding();
            // transform.position = map.CellToWorld(OffsetMovement[0]);
        }
    }
    #endregion

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

    #region not use
    private void GreedyMoveToTarget()
    {
        if (IsMoving)
        {
            if (path.Count > 0)
            {
                Vector3 currentWayPoint = mapIns.CellToWorld(path[0]);
                transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, Time.deltaTime * speed);
                if ((currentWayPoint - transform.position).magnitude <= 0.1f)
                {
                    path.RemoveAt(0);
                    RemoveLineRendererWayPoint();
                }
            }
            else
            {
                IsMoving = false;
            }
        }
    }
    #endregion

    #region Animation
    public float maxAngular = 5;
    private void LookAtTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position) * maxAngular;
        transform.forward += direction * Time.deltaTime;
    }
    #endregion

}