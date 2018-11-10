using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(LineRenderer))]
public class NavAgent : MonoBehaviour
{
    public enum PathFindingType
    {
        AStar,
        Greedy,
    }

    private bool    isLineAnimation;
    private float   animLineCounter;

    private EventSystem         eventSystem;
    private HexMap              HexMap;     
    private GreedySearch        GreedyCalculator;
    private LineRenderer        movingLineRenderer;
    private AStartAlgorithm     AStarCalculator;   
    private List<Vector3Int>    path;

    private Gradient colors;
    private GradientColorKey[] graColorKeys;
    private GradientAlphaKey[] graAlKeys;

    public float Speed;
    public int MaxMoveStep;    
    public Camera CameraRaycaster;
    public PathFindingType SearchType;
       
    public int CurrentMoveStep      { get; private set; }
    public bool IsMoving            { get; private set; }
    public bool IsComparePath       { get; set; }
    public bool IsAutoMove          { get; set; }
    
    public Vector3Int EndCell       { get; private set; }
    public Vector3Int StartCell     { get; private set; }

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
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
            if (eventSystem.IsPointerOverGameObject()) return;

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

        AnimationLine();
    }

    private void FindPath()
    {
        if (CurrentMoveStep >= MaxMoveStep || (path != null && path.Count == 0))
        {
            CurrentMoveStep = 0;
        }
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
        if (IsMoving)
        {
            RefreshLineRenderer(SearchType);
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
                Vector3 currentTarget = HexMap.CellToWorld(path[path.Count - 1]);
                transform.position = Vector3.MoveTowards(
                    current: transform.position,
                    target: currentTarget,
                    maxDistanceDelta: Time.deltaTime * Speed);
                if ((transform.position - currentTarget).magnitude <= 0.1f)
                {
                    path.RemoveAt(path.Count - 1);
                    RemoveLineRendererWayPoint();
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
                    RemoveLineRendererWayPoint();
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
        if (MaxMoveStep - 1 < CurrentMoveStep && path != null) path.Clear();
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

    private void RefreshLineRenderer(PathFindingType type)
    {
        if (path != null)
        {
            int count = path.Count;
            movingLineRenderer.positionCount = count;
            for (int i = 0; i < count; i++)
            {
                if (type == PathFindingType.AStar)
                {
                    movingLineRenderer.SetPosition(count - i - 1, HexMap.CellToWorld(path[count - i - 1]));
                }
                else
                {
                    movingLineRenderer.SetPosition(count - i - 1, HexMap.CellToWorld(path[i]));
                }
            }
        }
    }

    private void RemoveLineRendererWayPoint()
    {
        CurrentMoveStep++;
        CalculateGradiantColor();
        if (CurrentMoveStep >= MaxMoveStep)
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
            float time = (float)(path.Count - (MaxMoveStep - CurrentMoveStep)) / (float)path.Count;
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
        movingLineRenderer.SetPosition(1, HexMap.CellToWorld(EndCell));
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

    public void ClearPath()
    {
        movingLineRenderer.positionCount = 0;
        CurrentMoveStep = MaxMoveStep;
    }
}