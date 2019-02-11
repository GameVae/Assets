using System.Collections.Generic;
using Generic.Singleton;
using PathFinding;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class NavAgent : MonoBehaviour
{
    private bool isLineAnimation;
    private float animLineCounter;
    
    private HexMap map;
    private AStartAlgorithm aStar;
    private List<Vector3Int> path;

    private Gradient colors;
    private GradientColorKey[] graColorKeys;
    private GradientAlphaKey[] graAlKeys;
    private LineRenderer movingLineRenderer;

    [SerializeField] private int maxSearchLevel;
    [SerializeField] private int maxMoveStep;
    [SerializeField] private float speed;

    public List<Vector3Int> OffsetMovement;
    public int CurrentOffsetStep;
    public bool IsOffsetMoving { get; private set; }

    public int CurrentMoveStep { get; private set; }
    public bool IsMoving { get; private set; }

    public Vector3Int EndCell   { get; private set; }
    public Vector3Int StartCell { get; private set; }
    public Vector3Int CurrentCell
    {
        get
        {
            if (map != null)
                return map.WorldToCell(transform.position);
            return Vector3Int.zero;
        }
    }
    public CellInfomation Info { get; private set; }

    private void Awake()
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

    private void Start()
    {
        map = Singleton.Instance<HexMap>();

        Info = new CellInfomation()
        {
            GameObject = gameObject,
            Id = CellInfoManager.ID(),
        };

        aStar = new AStartAlgorithm()
        {
            HexMap = map,
            MaxLevel = maxSearchLevel,
        };

        MoveFinish();
    }

    private void Update()
    {
        MoveToTarget();

        MoveFollowOffset();

        AnimationLine();
    }

    private void InitLineRenderer()
    {
        if (IsMoving)
        {
            RefreshLineRenderer();
        }
        else
        {
            DrawNotFoundPath();
        }
    }

    private void AStarMoveToTarget()
    {
        if (IsMoving)
        {
            if (path.Count > 0)
            {
                Vector3 currentTarget = map.CellToWorld(path[path.Count - 1]);
                transform.position = Vector3.MoveTowards(
                    current: transform.position,
                    target: currentTarget,
                    maxDistanceDelta: Time.deltaTime * speed);

                if ((transform.position - currentTarget).magnitude <= 0.1f)
                {
                    path.RemoveAt(path.Count - 1);
                    RemoveLineRendererWayPoint();
                }
            }
            else
            {
                IsMoving = false;
                MoveFinish();
            }
        }
    }

    private void GreedyMoveToTarget()
    {
        if (IsMoving)
        {
            if (path.Count > 0)
            {
                Vector3 currentWayPoint = map.CellToWorld(path[0]);
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
                movingLineRenderer.SetPosition(count - i - 1, map.CellToWorld(path[count - i - 1]).AddY(0.2f));
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
            float time = (float)(path.Count - (maxMoveStep - CurrentMoveStep)) / (float)path.Count;
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
        movingLineRenderer.SetPosition(1, map.CellToWorld(EndCell));
        CalculateGradiantColor();
        isLineAnimation = true;
    }

    private void AnimationLine()
    {
        if (isLineAnimation)
        {
            animLineCounter += Time.deltaTime;
            if (animLineCounter >= 0.3f)
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
        Vector3Int currentCell = map.WorldToCell(transform.position).ZToZero();
        if (!Singleton.Instance<CellInfoManager>().AddToDict(currentCell, Info))
        {
            if (BreathFirstSearch.Instance.GetNearestCell(currentCell, out Vector3Int result))
            {
                FindPath(currentCell, result);
                CurrentMoveStep = 0;
            }
        }
    }

    public void StartMove(Vector3Int start, Vector3Int end)
    {
        if (IsMoving == false)
        {
            Singleton.Instance<CellInfoManager>().RemoveDict(start);
        }
        FindPath(start, end);
    }

    private void MoveFollowOffset()
    {
        if (IsOffsetMoving)
        {
            if (CurrentOffsetStep < OffsetMovement.Count)
            {
                Vector3 currentTarget = map.CellToWorld(OffsetMovement[CurrentOffsetStep]);
                transform.position = Vector3.MoveTowards(
                    current: transform.position,
                    target: currentTarget,
                    maxDistanceDelta: Time.deltaTime * speed);

                if ((transform.position - currentTarget).magnitude <= 0.1f)
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

    public void StartMove()
    {
        CurrentOffsetStep = 0;
        IsOffsetMoving = true;
        if (OffsetMovement.Count > 0)
        {
            Singleton.Instance<CellInfoManager>().RemoveDict(map.WorldToCell(transform.position).ZToZero());
            transform.position = map.CellToWorld(OffsetMovement[0]);
        }
    }

    private void Dead()
    {
        if(!IsMoving)
        {
            Singleton.Instance<CellInfoManager>().RemoveDict(map.WorldToCell(transform.position).ZToZero());
        }
    }

    private void OnDestroy()
    {
        Dead();
    }

}