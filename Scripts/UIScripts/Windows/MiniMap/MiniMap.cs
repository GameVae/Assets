using Generic.Contants;
using Generic.CustomInput;
using Generic.Singleton;
using System.Collections.Generic;
using UI.Composites;
using UI.Widget;
using UnityEngine;
using static NodeManagerProvider;

public class MiniMap : BaseWindow
{
    [SerializeField] private float delayCloseMiniMap;
    private bool isClosing;
    private float closeCounter;

    private Vector3Int selectedCell;
    private CrossInput crossInput;
    private NestedCondition selectCondition;

    public SelectableComp OpenButton;

    public CameraController CameraCtrl;
    public CursorPos cursor;

    public RectTransform MiniMapImage;
    public NavigateIcon MapSelectIcon;
    public RectTransform BuildingIcon;
    public Camera UICamera;

    private RangeWayPointManager constructNodeManager;
    private NodeManagerProvider managerProvider;

    public NodeManagerProvider NodeManagerProvider
    {
        get
        {
            return managerProvider ?? (managerProvider = Singleton.Instance<NodeManagerProvider>());
        }
    }
    public RangeWayPointManager ConstructNodeManager
    {
        get
        {
            return constructNodeManager ?? (constructNodeManager = NodeManagerProvider.
                GetManager<ConstructWayPoint>(NodeType.Range) as RangeWayPointManager);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        OpenButton.OnClickEvents += Open;
    }

    protected override void Start()
    {
        base.Start();
        selectCondition = new NestedCondition();
        crossInput = Singleton.Instance<CrossInput>();

        InitSelectCondition();
    }

    protected override void Update()
    {
        if (!isClosing)
        {
            if (selectCondition.Evaluate())
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle
                    (MiniMapImage, crossInput.Position, null, out Vector2 local);
                //Debugger.Log("local: " + local);
                //Debugger.Log("input pos: " + crossInput.Position);
                SetNavigateIcon(local);
            }
        }
        else
        {
            closeCounter += Time.deltaTime;
            if (closeCounter >= delayCloseMiniMap)
            {
                Close();
            }
        }
    }

    public override void Close()
    {
        if (isClosing)
        {
            isClosing = false;
            MoveCameraToCell(selectedCell);
            cursor.updateCursor(cursor.CellToWorldPoint(selectedCell));
        }
        base.Close();
    }

    private bool TrySetNavOnBuild(Rect area, out Vector3Int cell)
    {
        List<Vector3Int> buildingCell = ConstructNodeManager.Centers;

        for (int i = 0; i < buildingCell.Count; i++)
        {
            Vector3 miniMapPos = CellToMiniMap(buildingCell[i]);
            if (area.Contains((Vector2)miniMapPos))
            {
                cell = buildingCell[i];
                return true;
            }
        }
        cell = Vector3Int.one * -1;
        return false;
    }

    private Vector3Int GetCellOnMiniMap(Vector3 position)
    {
        Vector3Int result = Vector3Int.zero;
        Vector2 realMiniMapSize = MiniMapImage.Size();
        result.x = (int)(position.x * (Constants.TOTAL_COL / realMiniMapSize.x));
        result.y = (int)(position.y * (Constants.TOTAL_ROW / realMiniMapSize.y));
        return result;
    }

    private void SetupBuildingIcon()
    {
        List<Vector3Int> buildingCell = ConstructNodeManager.Centers;

        for (int i = 0; i < buildingCell.Count; i++)
        {
            Vector3 miniMapPos = CellToMiniMap(buildingCell[i]);
            RectTransform build = Instantiate(BuildingIcon, MiniMapImage);
            build.localPosition = miniMapPos;
            build.gameObject.SetActive(true);
        }
    }

    private void SetNavigateIcon(Vector3 position)
    {
        if (MiniMapImage.rect.Contains(position))
        {
            Vector3Int currentSelectCell = GetCellOnMiniMap(position);
            MapSelectIcon.SetPosition(position);

            if (TrySetNavOnBuild(MapSelectIcon.Rectangle, out Vector3Int cell))
            {
                selectedCell = cell;
                MapSelectIcon.SetPosition(CellToMiniMap(selectedCell));
            }
            else
            {
                selectedCell = currentSelectCell;
            }
            StartClose();
        }
    }

    private void MoveCameraToCell(Vector3Int cell)
    {
        CameraCtrl.Set(cell);
    }

    private void StartClose()
    {
        isClosing = true;
        closeCounter = 0.0f;
    }

    public Vector3 CellToMiniMap(Vector3Int cellPos)
    {
        Vector3 result = Vector3.zero;

        Vector2 realMiniMapSize = MiniMapImage.Size();

        result.x = (int)realMiniMapSize.x / Constants.TOTAL_COL;
        result.y = (int)realMiniMapSize.y / Constants.TOTAL_ROW;

        result.x *= cellPos.x;
        result.y *= cellPos.y;

        return result;
    }

    public override void Open()
    {
        base.Open();
        MapSelectIcon.SetPosition(CellToMiniMap(cursor.GetCurrentCell()));
    }

    protected override void Init()
    {
        SetupBuildingIcon();
    }

    public override void Load(params object[] input)
    {
        //throw new System.NotImplementedException();
    }

    private void InitSelectCondition()
    {
        selectCondition.Conditions += delegate { return crossInput.IsPointerUp; };
        selectCondition.Conditions += delegate
        {
            return Window.activeInHierarchy;
        };
    }

    [ContextMenu("Mini Map Size")]
    public void GetSize()
    {
        Debugger.Log("Size :" + MiniMapImage.Size());
        Debugger.Log("rect: " + MiniMapImage.rect);
    }
}

