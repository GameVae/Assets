using Generic.Singleton;
using System.Collections.Generic;
using UI.Widget;
using UnityEngine;

public class MiniMap : BaseWindow
{
    private bool isClosing;
    private float closeCounter;

    private Vector3Int selectedCell;
    private Vector3Int preSelectedCell;

    public float DelayCloseMiniMap;
    public GUIOnOffSwitch MapBtn;
    public GUIOnOffSwitch DescriptionBtn;

    public CameraPosition CamPosition;
    public CursorPos cursor;
    [Header("Dimention map base on grid")]
    public int Width;
    public int Height;
    public RectTransform MiniMapImage;
    public NavigateIcon MapSelectIcon;
    public RectTransform BuildingIcon;

    public Rect MiniMapRect { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        MapBtn.OnClick.AddListener(Open);
    }

    protected override void Start()
    {
        base.Start();

        float miniMapWidth = (MiniMapImage.anchorMax.x - MiniMapImage.anchorMin.x) * Screen.width;
        float miniMapHeight = (MiniMapImage.anchorMax.y - MiniMapImage.anchorMin.y) * Screen.height;
        float xPos = MiniMapImage.anchorMin.x * Screen.width;
        float yPos = MiniMapImage.anchorMin.y * Screen.height;

        MiniMapRect = new Rect(xPos, yPos, miniMapWidth, miniMapHeight);
    }


    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0) && MiniMapImage.gameObject.activeInHierarchy)
        {
            SetNavigateIcon(Input.mousePosition);
        }

        if (isClosing)
        {
            closeCounter += Time.deltaTime;
            if (closeCounter >= DelayCloseMiniMap)
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
        List<Vector3Int> buildingCell = Singleton.Instance<CellInfoManager>().BaseCell;

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

    private Vector3Int GetCellOnMiniMap(Vector3 mousePos)
    {
        Vector3Int result = Vector3Int.zero;
        result.x = (int)(((mousePos.x - MiniMapRect.x) / MiniMapRect.width) * Width);
        result.y = (int)(((mousePos.y - MiniMapRect.y) / MiniMapRect.height) * Height);
        return result;
    }

    private void SetupBuildingIcon()
    {
        List<Vector3Int> buildingCell = Singleton.Instance<CellInfoManager>().BaseCell;

        for (int i = 0; i < buildingCell.Count; i++)
        {
            Vector3 miniMapPos = CellToMiniMap(buildingCell[i]);
            RectTransform build = Instantiate(BuildingIcon, MiniMapImage);
            build.localPosition = miniMapPos;
            build.gameObject.SetActive(true);
        }
    }

    private void SetNavigateIcon(Vector3 mousePos)
    {
        if (MiniMapRect.Contains(mousePos))
        {
            Vector3Int currentSelectCell = GetCellOnMiniMap(mousePos);
            MapSelectIcon.SetPosition(CellToMiniMap(currentSelectCell));

            if (TrySetNavOnBuild(MapSelectIcon.Rectangle, out Vector3Int cell))
            {
                selectedCell = cell;
                MapSelectIcon.SetPosition(CellToMiniMap(selectedCell));
            }
            else
            {
                selectedCell = currentSelectCell + new Vector3Int(5, 5, 0);
            }
            StartClose();
        }
    }

    private void ResetSelectedCell()
    {
        selectedCell = Vector3Int.zero;
        preSelectedCell = Vector3Int.zero;
    }

    private void MoveCameraToCell(Vector3Int cell)
    {
        CamPosition.Set(cell);
    }

    private void StartClose()
    {
        isClosing = true;
        closeCounter = 0.0f;
    }

    public Vector3 CellToMiniMap(Vector3Int cellPos)
    {
        Vector3 result = Vector3.zero;
        result.x = cellPos.x * (MiniMapRect.width / Width);
        result.y = cellPos.y * (MiniMapRect.height / Height);
        result -= (Vector3)MiniMapImage.RealSize() / 2; // pivot 0.5 - 0.5
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
        throw new System.NotImplementedException();
    }
}
