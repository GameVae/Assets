using Generic.Contants;
using Generic.CustomInput;
using Generic.Pooling;
using Generic.Singleton;
using System.Collections.Generic;
using UI.Composites;
using UI.Widget;
using UnityEngine;
using static NodeManagerProvider;

public class MiniMap : BaseWindow
{
    [SerializeField] private float delayCloseMiniMap;
    private bool isOpened;
    private bool isClosing;
    private float closeCounter;

    private Vector3Int selectedCell;
    private CrossInput crossInput;
    private NestedCondition selectCondition;

    public ConstructIcon ConstructIconPrefab;
    public SelectableComp OpenButton;

    public CameraController CameraCtrl;
    public CursorPos cursor;

    public RectTransform MiniMapImage;
    public NavigateIcon MapSelectIcon;
    public RectTransform BuildingIcon;
    public Camera UICamera;

    private Vector2Int uiMapSize;
    private Vector2Int pxPerNode;

    private Pooling<ConstructIcon> iconPooling;
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

    private List<ConstructIcon> catcher;
    private List<ConstructIcon> Catcher
    {
        get { return catcher ?? (catcher = new List<ConstructIcon>()); }
    }

    public CrossInput CrossInput
    {
        get
        {
            return crossInput ?? (crossInput = Singleton.Instance<CrossInput>());
        }
    }

    private void Awake()
    {
        OpenButton.OnClickEvents += Open;

        iconPooling = new Pooling<ConstructIcon>(CreateIcon, 10);
    }

    protected override void Start()
    {
        base.Start();
        selectCondition = new NestedCondition();

        InitSelectCondition();
    }

    protected override void Update()
    {
        if (isOpened)
        {
            if (!isClosing)
            {
                if (selectCondition.Evaluate())
                {
                    RectTransformUtility.ScreenPointToLocalPointInRectangle
                        (MiniMapImage, CrossInput.Position, null, out Vector2 local);
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
    }

    public override void Close()
    {
        if (isClosing)
        {
            isClosing = false;
            MoveCameraToCell(selectedCell);
            cursor.updateCursor(cursor.CellToWorldPoint(selectedCell));
        }

        isOpened = false;
        ReleaseIcon();

        base.Close();
    }

    private void SetupBuildingIcon()
    {
        List<Vector3Int> constructPositions = ConstructNodeManager.Centers;

        for (int i = 0; i < constructPositions.Count; i++)
        {
            Vector3 position = ToLocalFrom(constructPositions[i]);

            ConstructIcon icon = iconPooling.GetItem();
            icon.SetHoldingPositions(ConstructNodeManager, constructPositions[i]);
            (icon.transform as RectTransform).localPosition = position;

            Catcher.Add(icon);
            icon.gameObject.SetActive(true);
        }
    }

    private void SetNavigateIcon(Vector2 local)
    {
        if (!SetNavIconOnBuild(local))
        {
            selectedCell = To3IntFrom(local);
            if (SetNavIconPosition(selectedCell))
                StartClose();
        }
        else
            StartClose();
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

    public override void Open()
    {
        base.Open();
        Load();

        SetNavIconPosition(cursor.GetCurrentCell());
        isOpened = true;
    }

    protected override void Init()
    {
        InitalizeOffset();
    }

    public override void Load(params object[] input)
    {
        SetupBuildingIcon();
    }

    private void InitSelectCondition()
    {
        selectCondition.Conditions += delegate { return CrossInput.IsPointerUp; };
        selectCondition.Conditions += delegate
        {
            return Window.activeInHierarchy;
        };
    }

    private void OnSelectConstructIcon(ConstructIcon icon)
    {
        SetNavIconPosition(icon.Center);
    }

    private Vector3 ToLocalFrom(Vector3Int pos)
    {
        Vector3 result = Vector3.zero;
        result.x = pxPerNode.x * pos.x;
        result.y = pxPerNode.x * pos.y;
        return result;
    }

    private Vector3Int To3IntFrom(Vector2 pos)
    {
        Vector3Int result = Vector3Int.zero;
        result.x = (int)pos.x / pxPerNode.x;
        result.y = (int)pos.y / pxPerNode.x;
        return result;
    }

    private bool SetNavIconPosition(Vector3Int position)
    {
        if (Constants.IsValidCell(position.x, position.y))
        {
            Vector3 localPos = ToLocalFrom(position);
            MapSelectIcon.SetPosition(localPos);
            return true;
        }
        return false;
    }

    private ConstructIcon CreateIcon(int id)
    {
        ConstructIcon build = Instantiate(ConstructIconPrefab, MiniMapImage);
        build.FirstSetup(id);
        return build;
    }

    private void InitalizeOffset()
    {
        uiMapSize = new Vector2Int((int)MiniMapImage.rect.size.x, (int)MiniMapImage.rect.size.y);

        pxPerNode.x = uiMapSize.x / Constants.TOTAL_COL;
        pxPerNode.y = uiMapSize.y / Constants.TOTAL_ROW;
    }

    private bool SetNavIconOnBuild(Vector2 local)
    {
        for (int i = 0; i < Catcher.Count; i++)
        {
            if (Catcher[i].IsPointerOver(local))
            {
                OnSelectConstructIcon(Catcher[i]);
                selectedCell = Catcher[i].Center;
                return true;
            }
        }
        return false;
    }

    private void ReleaseIcon()
    {
        for (int i = Catcher.Count - 1; i >= 0; i--)
        {
            iconPooling.Release(Catcher[i]);
            Catcher.RemoveAt(i);
        }
    }
}

