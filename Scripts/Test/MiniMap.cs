using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public CursorPos cursor;
    public Button MapBtn;
    public GameObject Panel;
    [Header("Dimention map base on grid")]
    public int Width;
    public int Height;
    public RectTransform MiniMapImage;
    public NavigateIcon MapSelectIcon;
    public Rect MiniMapRect;
    public GameObject BuildingIcon;

    private List<uint> buildingIndex;
    private bool onMiniMap;

    private Vector3Int selectedCell;
    private Vector3Int preSelectedCell;

    private void OnEnable()
    {

        GetComponentInChildren<Button>().onClick.AddListener(Close);
        MapBtn.onClick.AddListener(ShowMiniMap);
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
    }
    
    private void Close()
    {
        MapSelectIcon.Disable();
        onMiniMap = false;
        Panel.gameObject.SetActive(false);
    }
   
    private bool TrySetNavOnBuild(Rect area, out Vector3Int cell)
    {
        cell = Vector3Int.zero;
        if (buildingIndex == null)
        {
            buildingIndex = BuiltCellContainer.Instance.BuildingIndex;
        }
        for (int i = 0; i < buildingIndex.Count; i++)
        {
            bool boolResult = BuiltCellContainer.Instance.Convert1DTo2D(buildingIndex[i], out Vector3Int result);
            if (boolResult)
            {
                Vector3 miniMapPos = CellToMiniMap(result);
                if (area.Contains((Vector2)miniMapPos))
                {
                    cell = result;
                    return BuiltCellContainer.Instance.TrySetIndexOnBuild(result);
                }
            }
        }
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
        if (buildingIndex == null)
        {
            buildingIndex = BuiltCellContainer.Instance.BuildingIndex;
        }
        Debug.Log(buildingIndex.Count);
        for (int i = 0; i < buildingIndex.Count; i++)
        {
            bool boolResult = BuiltCellContainer.Instance.Convert1DTo2D(buildingIndex[i], out Vector3Int result);
            if (boolResult)
            {
                Vector3 miniMapPos = CellToMiniMap(result);
                GameObject build = Instantiate(BuildingIcon, MiniMapImage);
                build.SetActive(true);
                build.GetComponent<RectTransform>().position = miniMapPos;

            }
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
                MapSelectIcon.SetPosition(CellToMiniMap(cell));
                selectedCell = cell;
                MapSelectIcon.SetPosition(CellToMiniMap(selectedCell));
            }
            else
            {
                selectedCell = currentSelectCell + new Vector3Int(5, 5, 0);
            }
            if (preSelectedCell == selectedCell)
            {
                MoveCameraToCell(selectedCell);
                ResetSelectedCell();
                Close();
            }
            else if ((MapSelectIcon.Rectangle.Contains(CellToMiniMap(selectedCell)) &&
                MapSelectIcon.Rectangle.Contains(CellToMiniMap(preSelectedCell))))
            {
                Vector3 worldPoint = cursor.CellToWorldPoint(selectedCell);
                cursor.updateCursor(worldPoint);
                MoveCameraToCell(selectedCell);
                ResetSelectedCell();
                Close();
            }
            preSelectedCell = selectedCell;
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
        Panel.gameObject.SetActive(true);
    }

    public Vector3 CellToMiniMap(Vector3Int cellPos)
    {
        Vector3 result = Vector3.zero;
        result.x = cellPos.x * (MiniMapRect.width / Width) + MiniMapRect.x;
        result.y = cellPos.y * (MiniMapRect.height / Height) + MiniMapRect.y;
        return result;
    }
}
