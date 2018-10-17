using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        new Vector3Int(-1,-2, 0),
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
        new Vector3Int( 0, 0, 0),   // center               
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

    public uint TotalRow;
    public uint TotalCol;
   
    public Grid grid;
    public Tilemap map;
    public CursorPos cursorPos;
    public Camera cameraRaycaster;
      
    public static BuiltCellContainer Instance   { get; private set; }
    public Dictionary<uint, uint>    BuiltCells { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance);
        BuiltCells = new Dictionary<uint, uint>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(
                cameraRaycaster.ScreenPointToRay(Input.mousePosition),
                out RaycastHit hitInfo,
                int.MaxValue))
            {
                Vector3Int selectCell = grid.WorldToCell(hitInfo.point);
                uint selectCellIndex = Convert2DTo1D(selectCell.x, selectCell.y);
                if (BuiltCells.ContainsKey(selectCellIndex))
                {
                    if (Convert1DTo2D(BuiltCells[selectCellIndex], out Vector3Int result))
                    {
                        cursorPos.updateCursor(grid.CellToWorld(result));
                    }
                }
                else
                {
                    cursorPos.updateCursor(hitInfo.point);
                }
            }
        }
    }
    private uint Convert2DTo1D(int row, int col)
    {
        return (uint)row * TotalCol + (uint)col;
    }
    private bool Convert1DTo2D(uint index, out Vector3Int result)
    {
        result = Vector3Int.zero;
        if (index < 0) return false;
        result.x = (int)(index / TotalCol);
        result.y = (int)(index % TotalCol);
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
            //map.SetTile(temp, null);
        }
        if(isExpand)
        {
            pattern = bound.CellPosision.y % 2 == 0 ? HexaPattern11 : HexaPattern21;
            for (int i = 0; i < pattern.Length; i++)
            {
                Vector3Int temp = cellPos + pattern[i];
                uint cellIndex = Convert2DTo1D(temp.x, temp.y);
                BuiltCells.Add(cellIndex, centerIndex);
                map.SetTile(temp, null);
            }
        }
    }
}
