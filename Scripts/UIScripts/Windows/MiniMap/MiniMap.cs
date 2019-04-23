using Generic.Contants;
using Generic.CustomInput;
using Generic.Pooling;
using Generic.Singleton;
using System.Collections.Generic;
using UI.Composites;
using UnityEngine;
using static NodeManagerProvider;

public class MiniMap : BaseWindow
{
    [SerializeField] private float delayCloseMiniMap;

    private int openAtFrame;
    private bool isOpened;
    private bool isClosing;
    private float closeCounter;
    private Vector2 uiMapSize;
    private Vector2 pxPerNode;

    private CrossInput crossInput;
    private Vector3Int selectedPosition;
    private NestedCondition selectCondition;

    private List<ConstructIcon> catcher;
    private Pooling<ConstructIcon> iconPooling;
    private NodeManagerProvider managerProvider;
    private RangeWayPointManager constructNodeManager;

    public RectTransform MiniMapImage;
    public NavigateIcon MapSelectIcon;

    public CursorPos Cursor;
    public SelectableComp OpenButton;
    public CameraController CameraCtrl;
    public ConstructIcon ConstructIconPrefab;

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

    private void Awake()
    {
        OpenButton.OnClickEvents += Open;

        iconPooling = new Pooling<ConstructIcon>(CreateIcon, 10);
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
            Vector3Int clientPos = selectedPosition.ToClientPosition();
            MoveCameraToCell(clientPos);
            Cursor.updateCursor(Cursor.CellToWorldPoint(clientPos));
        }

        isOpened = false;
        ReleaseIcon();

        base.Close();
    }

    private void SetNavigateIcon(Vector2 local)
    {
        if (!TrySetIconOnConstruct(local))
        {
            selectedPosition = To3IntFrom(local);
          
            if (SetIconPosition(selectedPosition))
            {
                StartClose();
            }
        }
        else
        {
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

    public override void Open()
    {
        base.Open();
        Load();

        Vector3Int serPos = Cursor.GetCurrentCell().ToSerPosition();
        SetIconPosition(serPos);
        openAtFrame = Time.frameCount;
        isOpened = true;
    }

    protected override void Init()
    {
        Initalize();
    }

    public override void Load(params object[] input)
    {
        SetupBuildingIcon();
    }

    /// <summary>
    /// convert from ser position to minimap position
    /// </summary>
    /// <param name="serPos">ser position</param>
    /// <returns></returns>
    private Vector3 ToLocalFrom(Vector3Int serPos)
    {
        Vector3 result = Vector3.zero;
        result.x = pxPerNode.x * serPos.x;
        result.y = pxPerNode.x * serPos.y;
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="local">local position on mini map image</param>
    /// <returns>Server position</returns>
    private Vector3Int To3IntFrom(Vector2 local)
    {
        Vector3Int result = Vector3Int.zero;
        result.x = (int)(local.x / pxPerNode.x);
        result.y = (int)(local.y / pxPerNode.y);
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serPos">server position</param>
    /// <returns></returns>
    private bool SetIconPosition(Vector3Int serPos)
    {
        Vector3Int clientPos = serPos.ToClientPosition();
        if (Constants.IsValidCell(clientPos.x, clientPos.y))
        {
            Vector3 localPos = ToLocalFrom(serPos);
            MapSelectIcon.SetPosition(localPos);
            return true;
        }
        return false;
    }

    private void OnSelectConstructIcon(ConstructIcon icon)
    {
        SetIconPosition(icon.Center);
    }

    private bool TrySetIconOnConstruct(Vector2 local)
    {
        for (int i = 0; i < Catcher.Count; i++)
        {
            if (Catcher[i].IsPointerOver(local))
            {
                OnSelectConstructIcon(Catcher[i]);
                selectedPosition = Catcher[i].Center;
                return true;
            }
        }
        return false;
    }

    #region Initalize
    private void InitSelectCondition()
    {
        selectCondition = new NestedCondition();
        selectCondition.Conditions += delegate 
        {
            return CrossInput.IsPointerUp;
        };
        selectCondition.Conditions += delegate
        {
            return Window.activeInHierarchy;
        };

        selectCondition.Conditions += delegate
         {
             return openAtFrame != Time.frameCount;
         };
    }

    private void SetupBuildingIcon()
    {
        List<Vector3Int> constructPositions = ConstructNodeManager.Centers;

        for (int i = 0; i < constructPositions.Count; i++)
        {
            Vector3Int serPos = constructPositions[i].ToSerPosition();
            Vector3 position = ToLocalFrom(serPos);

            ConstructIcon icon = iconPooling.GetItem();
            icon.SetHoldingPositions(ConstructNodeManager, serPos);
            icon.transform.localPosition = position;

            Catcher.Add(icon);
            icon.gameObject.SetActive(true);
        }
    }

    private void Initalize()
    {
        InitSelectCondition();

        uiMapSize = MiniMapImage.rect.size;

        pxPerNode.x = uiMapSize.x / (Constants.TOTAL_COL - 10);
        pxPerNode.y = uiMapSize.y / (Constants.TOTAL_ROW - 10);
    }
    #endregion

    #region Construct icon pooling
    private ConstructIcon CreateIcon(int id)
    {
        ConstructIcon build = Instantiate(ConstructIconPrefab, MiniMapImage);
        build.FirstSetup(id);
        return build;
    }

    private void ReleaseIcon()
    {
        for (int i = Catcher.Count - 1; i >= 0; i--)
        {
            iconPooling.Release(Catcher[i]);
            Catcher.RemoveAt(i);
        }
    }
    #endregion
}

