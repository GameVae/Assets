using Generic.Contants;
using Generic.Singleton;
using System.Collections.Generic;
using UI.Widget;
using UnityEngine;

public class MiniMap : BaseWindow
{
    private bool isClosing;
    private float closeCounter;
    [SerializeField]
    private float DelayCloseMiniMap;

    private Vector3Int selectedCell;

    public GUIOnOffSwitch OpenBtn;

    public CameraController CameraCtrl;
    public CursorPos cursor;

    public RectTransform MiniMapImage;
    public NavigateIcon MapSelectIcon;
    public RectTransform BuildingIcon;
    public Camera UICamera;

    protected override void Awake()
    {
        base.Awake();
        OpenBtn.OnClick.AddListener(Open);

    }

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0) && MiniMapImage.gameObject.activeInHierarchy)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(MiniMapImage, Input.mousePosition, UICamera, out Vector2 local);
            SetNavigateIcon(local);
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
        List<Vector3Int> buildingCell = Singleton.Instance<GlobalNodeManager>().TowerPositions;

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
        result.x = (int)(position.x * ( Constants.TOTAL_COL / realMiniMapSize.x));
        result.y = (int)(position.y * (Constants.TOTAL_ROW / realMiniMapSize.y));
        return result;
    }

    private void SetupBuildingIcon()
    {
        List<Vector3Int> buildingCell = Singleton.Instance<GlobalNodeManager>().TowerPositions;

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
                selectedCell = currentSelectCell + new Vector3Int(5, 5, 0);
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
}
