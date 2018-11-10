using System.Collections.Generic;
using UnityEngine;

public class HexUIGenerator : MonoBehaviour
{

    private readonly Vector3Int[] HexaPatternEven1 = new Vector3Int[]
{
        new Vector3Int( 0,-1, 0),
        new Vector3Int( 0, 1, 0),
        new Vector3Int(-1,-1, 0),
        new Vector3Int(-1, 1, 0),
        new Vector3Int( 1, 0, 0),
        new Vector3Int(-1, 0, 0),
};
    private readonly Vector3Int[] HexaPatternOdd1 = new Vector3Int[]
{
        new Vector3Int( 1,-1, 0),
        new Vector3Int( 1, 1, 0),
        new Vector3Int( 0,-1, 0),
        new Vector3Int( 0, 1, 0),
        new Vector3Int( 1, 0, 0),
        new Vector3Int(-1, 0, 0),
};

    public int MaxLevel;

    [HideInInspector]
    public List<Transform> HexCellUIs;

    public Transform HexCellPrefab;
    public Camera MainCam;
    public Grid HexGrid;

    private List<int> openCell;
    private List<int> closedCell;

    private Vector3Int preCenterCell;
    private Vector3 preCamPosition;
    private Quaternion preCamRotation;

    private void Awake()
    {
        if (HexCellUIs == null) HexCellUIs = new List<Transform>();
        if (openCell == null) openCell = new List<int>();
        if (closedCell == null) closedCell = new List<int>();
    }

    private void Start()
    {
        Gen();
        preCamPosition = MainCam.transform.position;
        preCamRotation = MainCam.transform.rotation;
    }


    private void Update()
    {
        if(preCamPosition != MainCam.transform.position ||  preCamRotation != MainCam.transform.rotation)
        {
            if (Physics.Raycast(new Ray(MainCam.transform.position, MainCam.transform.forward), out RaycastHit hitInfo, 200.0f))
            {
                Vector3Int currentCenter = HexGrid.WorldToCell(hitInfo.point);
                currentCenter.z = 0;
                if (currentCenter != preCenterCell)
                {
                    FindDistance(ConvertToIndex(currentCenter.x, currentCenter.y));
                    ReOrderCellUIPosition();
                }
            }

            preCamPosition = MainCam.transform.position;
            preCamRotation = MainCam.transform.rotation;
        }
    }

    [ContextMenu("Gen map")]
    public void Gen()
    {
        if (HexCellUIs == null) HexCellUIs = new List<Transform>();
        if (openCell == null) openCell = new List<int>();
        if (closedCell == null) closedCell = new List<int>();

        if (Physics.Raycast(new Ray(MainCam.transform.position, MainCam.transform.forward), out RaycastHit hitInfo, float.MaxValue))
        {
            Vector3Int cellPos = HexGrid.WorldToCell(hitInfo.point);
            FindDistance(ConvertToIndex(cellPos.x, cellPos.y));
            CreateUIInstance();
        }
    }

    [ContextMenu("Clear")]
    private void Clear()
    {
        closedCell.Clear();
        openCell.Clear();
    }

    private void CreateUIInstance()
    {
        for (int i = 0; i < closedCell.Count; i++)
        {
            Vector3 pos = HexGrid.CellToWorld(ConvertToVector3Int(closedCell[i]));
            Transform ui = Instantiate(HexCellPrefab);
            ui.position = pos;
            ui.SetParent(transform);
            ui.gameObject.SetActive(true);
            HexCellUIs.Add(ui);
        }
    }

    private void FindDistance(int cellIndex)
    {
        preCenterCell = ConvertToVector3Int(cellIndex);
        Clear();
        openCell.Add(cellIndex);

        float start = Time.realtimeSinceStartup;
        CalculateDistance(level: 0);
    }

    private void CalculateDistance(int level)
    {
        if (openCell.Count == 0) return;
        int currentCellIndex = openCell[0];

        openCell.RemoveAt(0);
        closedCell.Add(currentCellIndex);

        if (level < MaxLevel)
        {
            Vector3Int center = ConvertToVector3Int(currentCellIndex);
            Vector3Int[] neighbours = GetNeigboursPosition(center);
            int index = -1;
            for (int i = 0; i < neighbours.Length; i++)
            {
                index = ConvertToIndex(neighbours[i].x, neighbours[i].y);
                if (!closedCell.Contains(index) && 
                    !openCell.Contains(index) && 
                    IsValidCell(neighbours[i].x, neighbours[i].y))
                {
                    openCell.Add(index);
                }
            }
        }
        CalculateDistance(level + 1);
    }

    private Vector3Int[] GetNeigboursPosition(Vector3Int cell)
    {
        List<Vector3Int> neighbours = new List<Vector3Int>();
        Vector3Int neighbour;
        Vector3Int[] pattern = (cell.y % 2 == 0) ? HexaPatternEven1 : HexaPatternOdd1;
        for (int i = 0; i < pattern.Length; i++)
        {
            neighbour = pattern[i] + cell;
            if (IsValidCell(neighbour.x, neighbour.y))
            {
                neighbours.Add(neighbour);
            }
        }
        return neighbours.ToArray();
    }

    private bool IsValidCell(int x, int y)
    {
        return x >= 5 && x <= 522 - 5 && y >= 5 && y <= 522 - 5;
    }

    private int ConvertToIndex(int x, int y)
    {
        return 522 * y + x;
    }

    private void ReOrderCellUIPosition()
    {
        for (int i = 0,j =0; j < closedCell.Count && i < HexCellUIs.Count; i++)
        {
            if (HexCellUIs[i] != null)
            {
                HexCellUIs[i].position = HexGrid.CellToWorld(ConvertToVector3Int(closedCell[j]));
                j++;
            }
        }
    }

    public Vector3Int ConvertToVector3Int(int index)
    {
        Vector3Int result = Vector3Int.zero;
        result.x = index % 522;
        result.y = index / 522;
        return result;
    }
}
