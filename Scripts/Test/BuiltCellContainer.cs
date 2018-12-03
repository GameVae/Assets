using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuiltCellContainer : MonoBehaviour
{
    // y round
    private readonly Vector3Int[] HexaPattern1 = new Vector3Int[]
    {
        new Vector3Int( 0,-1, 0),   // bot right   
        new Vector3Int( 0, 1, 0),   // top right 
        new Vector3Int(-1,-1, 0),   // top left
        new Vector3Int(-1, 1, 0),   // bot right
        new Vector3Int( 1, 0, 0),   // center right
        new Vector3Int(-1, 0, 0),   // center left       
        new Vector3Int( 0, 0, 0),   // center               
    };
    private readonly Vector3Int[] HexaPattern11 = new Vector3Int[]
    {
        new Vector3Int( 0, 2, 0),
        new Vector3Int( 0,-2, 0),
        new Vector3Int( 2, 0, 0),
        new Vector3Int(-2, 0, 0),
        new Vector3Int( 1, 1, 0),
        new Vector3Int( 1,-2, 0),
        new Vector3Int( 1,-1, 0),
        new Vector3Int( 1, 2, 0),
        new Vector3Int(-1, 2, 0),
        new Vector3Int(-2, 1, 0),
        new Vector3Int(-2,-1, 0),
        // new Vector3Int(-1,-2, 0),
    };
    // y odd
    private readonly Vector3Int[] HexaPattern2 = new Vector3Int[]
    {
        new Vector3Int( 1,-1, 0),   // top right   
        new Vector3Int( 1, 1, 0),   // bot right 
        new Vector3Int( 0,-1, 0),   // top left
        new Vector3Int( 0, 1, 0),   // bot right
        new Vector3Int( 1, 0, 0),   // center right
        new Vector3Int(-1, 0, 0),   // center left       
        // new Vector3Int( 0, 0, 0),   // center               
    };
    private readonly Vector3Int[] HexaPattern21 = new Vector3Int[]
    {
        new Vector3Int( 0, 2, 0),               
        new Vector3Int( 0,-2, 0),
        new Vector3Int(-2, 0, 0),
        new Vector3Int( 2, 0, 0),
        new Vector3Int(-1, 1, 0),
        new Vector3Int(-1, 2, 0),
        new Vector3Int( 1, 2, 0),
        new Vector3Int( 2, 1, 0),
        new Vector3Int(-1,-1, 0),
        new Vector3Int(-1,-2, 0),
        new Vector3Int( 2,-1, 0),
        new Vector3Int( 1,-2, 0),
    };

    private EventSystem eventSystem;

    public uint TotalRow;
    public uint TotalCol;
   
    public Grid grid;
    public Tilemap map;
    public CursorPos cursorPos;
    public Camera cameraRaycaster;
   
    public static BuiltCellContainer    Instance        { get; private set; }
    public Dictionary<uint, uint>       BuiltCells      { get; private set; }
    public List<uint>                   BuildingIndex   { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
        BuiltCells = new Dictionary<uint, uint>();
        BuildingIndex = new List<uint>();
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;

            bool raycastHitted = Physics.Raycast(
                cameraRaycaster.ScreenPointToRay(mousePos),
                out RaycastHit hitInfo,
                int.MaxValue);
            bool onClickUI = eventSystem.IsPointerOverGameObject();

            if (raycastHitted && !onClickUI)
            {
                Vector3Int selectCell = grid.WorldToCell(hitInfo.point);
                if (TrySetIndexOnBuild(selectCell,out Vector3Int result))
                {
                    cursorPos.updateCursor(grid.CellToWorld(result));
                }
                else
                {
                    cursorPos.updateCursor(hitInfo.point);
                }
            }
        }
    }

    public uint Convert2DTo1D(int row, int col)
    {
        return (uint)row * TotalCol + (uint)col;
    }

    /// <summary>
    /// Return position on grid
    /// </summary>
    /// <param name="index">Value in 1 dimention</param>
    /// <param name="result">Position on grid if this return true </param>
    /// <returns></returns>
    public bool Convert1DTo2D(uint index, out Vector3Int result)
    {
        result = Vector3Int.zero;
        if (index < 0) return false;
        result.x = (int)(index % TotalCol);
        result.y = (int)(index / TotalCol);
        return true;
    }

    public void AddBuiltCell(BuildingBound bound,bool isExpand)
    {
        uint centerIndex = Convert2DTo1D(bound.CellPosision.x, bound.CellPosision.y);        
        Vector3Int cellPos = bound.CellPosision;

        Vector3Int[] pattern = bound.CellPosision.y % 2 == 0 ? HexaPattern1 : HexaPattern2;
        for (int i = 0; i < pattern.Length; i++)
        {
            Vector3Int temp = cellPos + pattern[i];
            uint cellIndex = Convert2DTo1D(temp.x, temp.y);
            BuiltCells.Add(cellIndex, centerIndex);
        }
        if(isExpand)
        {
            pattern = bound.CellPosision.y % 2 == 0 ? HexaPattern11 : HexaPattern21;
            for (int i = 0; i < pattern.Length; i++)
            {
                Vector3Int temp = cellPos + pattern[i];
                uint cellIndex = Convert2DTo1D(temp.x, temp.y);
                BuiltCells.Add(cellIndex, centerIndex);
                //map.SetTile(temp, null);
            }
        }
        if (!BuildingIndex.Contains(centerIndex)) BuildingIndex.Add(centerIndex);
    }

    public bool TrySetIndexOnBuild(Vector3Int cellIndex,out Vector3Int cellResult)
    {
        uint selectCellIndex = Convert2DTo1D(cellIndex.x, cellIndex.y);
        cellResult = Vector3Int.zero;
        if (BuiltCells.ContainsKey(selectCellIndex))
        {
            if (Convert1DTo2D(BuiltCells[selectCellIndex], out Vector3Int result))
            {
                cellResult = result;
                return true;
            }
        }
        return false;
    }
}
