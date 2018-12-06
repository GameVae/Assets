using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public enum SelectPointType
    {
        Auto,
        DoubleClick
    }

    private bool onMiniMap;
    private bool isClosing;
    private float closeCounter;

    private Vector3Int selectedCell;
    private Vector3Int preSelectedCell;

    public float DelayCloseMiniMap;
    public SelectPointType SelectType;
    public Button MapBtn;
    public Button DescriptionBtn;

    public GameObject Panel;
    public GameObject LevelDescription;

    public CursorPos cursor;
    [Header("Dimention map base on grid")]
    public int Width;
    public int Height;
    public RectTransform MiniMapImage;
    public NavigateIcon MapSelectIcon;    
    public GameObject BuildingIcon;

    public Rect MiniMapRect { get; private set; }

    private void OnEnable()
    {

        GetComponentInChildren<Button>().onClick.AddListener(ManualClose);

        MapBtn.onClick.AddListener(ShowMiniMap);
        
        DescriptionBtn.onClick.AddListener(ShowMiniMap);
        DescriptionBtn.onClick.AddListener(ShowDescription);

        LevelDescription.gameObject.SetActive(false);
        Panel.gameObject.SetActive(false);

        float miniMapWidth = (MiniMapImage.anchorMax.x - MiniMapImage.anchorMin.x) * Screen.width;
        float miniMapHeight = (MiniMapImage.anchorMax.y - MiniMapImage.anchorMin.y) * Screen.height;
        float xPos = MiniMapImage.anchorMin.x * Screen.width;
        float yPos = MiniMapImage.anchorMin.y * Screen.height;
        MiniMapRect = new Rect(xPos, yPos, miniMapWidth, miniMapHeight);

        ResetSelectedCell();
    }

    private void Start()
    {
        SetupBuildingIcon();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (onMiniMap)
            {
                SetNavigateIcon(Input.mousePosition);
            }
        }

        if(isClosing)
        {
            closeCounter += Time.deltaTime;
            if(closeCounter >= DelayCloseMiniMap)
            {

                Close();
            }
        }
    }
    
    private void Close()
    {
        if (isClosing)
        {
            onMiniMap = false;
            isClosing = false;
            MapSelectIcon.Disable();
            MoveCameraToCell(selectedCell);
            Panel.gameObject.SetActive(false);
            cursor.updateCursor(cursor.CellToWorldPoint(selectedCell));

            // refresh 
            ResetSelectedCell();
        }
    }

    private void ManualClose()
    {
        isClosing = false;
        onMiniMap = false;
        MapSelectIcon.Disable();
        Panel.gameObject.SetActive(false);
        ResetSelectedCell();
    }
   
    private bool TrySetNavOnBuild(Rect area, out Vector3Int cell)
    {
        List<Vector3Int> buildingCell = CellInfoManager.Instance.BuildingCell;
        for (int i = 0; i < buildingCell.Count ; i++)
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
        List<Vector3Int> buildingCell = CellInfoManager.Instance.BuildingCell;
        for (int i = 0; i < buildingCell.Count; i++)
        {
            Vector3 miniMapPos = CellToMiniMap(buildingCell[i]);
            GameObject build = Instantiate(BuildingIcon, MiniMapImage);
            build.SetActive(true);
            build.GetComponent<RectTransform>().position = miniMapPos;
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
            if (SelectType == SelectPointType.Auto)
            {
                StartClose();
            }
            else if (SelectType == SelectPointType.DoubleClick)
            {
                if (preSelectedCell == selectedCell)
                {
                    Close();
                }
                else if ((MapSelectIcon.Rectangle.Contains(CellToMiniMap(selectedCell)) &&
                    MapSelectIcon.Rectangle.Contains(CellToMiniMap(preSelectedCell))))
                {                   
                    Close();
                }
                preSelectedCell = selectedCell;
            }
        }
    }

    private void ResetSelectedCell()
    {
        selectedCell = Vector3Int.zero;
        preSelectedCell = Vector3Int.zero;
    }

    private void MoveCameraToCell(Vector3Int cell)
    {
        Vector3 worldPoint = Vector3.zero;
        worldPoint = cursor.CellToWorldPoint(cell);
        worldPoint.y = Camera.main.transform.position.y;
        Camera.main.transform.position = worldPoint;
    }

    private void ShowMiniMap()
    {
        if (onMiniMap) return;
        onMiniMap = true;
        LevelDescription.gameObject.SetActive(false);
        Panel.gameObject.SetActive(true);
        MapSelectIcon.SetPosition(CellToMiniMap(cursor.GetCurrentCell()));
    }

    private void StartClose()
    {
        isClosing = true;
        closeCounter = 0.0f;
    }

    public Vector3 CellToMiniMap(Vector3Int cellPos)
    {
        Vector3 result = Vector3.zero;
        result.x = cellPos.x * (MiniMapRect.width / Width) + MiniMapRect.x;
        result.y = cellPos.y * (MiniMapRect.height / Height) + MiniMapRect.y;
        return result;
    }

    private void ShowDescription()
    {
        if (!LevelDescription.activeInHierarchy)
        {
            LevelDescription.gameObject.SetActive(true);
        }
        else
        {
            LevelDescription.gameObject.SetActive(false);
        }
    }
}
