using UnityEngine;

public class BuildingBound : MonoBehaviour
{
    private Grid grid;
    private CellInfomation cellInfo;

    public bool IsExpand;
    public Vector3Int CellPosision;   

    private void Awake()
    {
        grid = FindObjectOfType<Grid>();
        transform.position = grid.CellToWorld(CellPosision);
        cellInfo = new CellInfomation()
        {
            GameObject = gameObject,
            Id = CellInfoManager.ID(),
        };
    }
    private void Start()
    {
        CellInfoManager.Instance.AddBuilding(grid.WorldToCell(transform.position).ZToZero(), cellInfo, IsExpand);
    }

    [ContextMenu("Set Position")]
    public void SetPosition()
    {
        transform.position = grid.CellToWorld(CellPosision);
    }

    [ContextMenu("Get Cell Position")]
    public void GetPosition()
    {
        CellPosision = grid.WorldToCell(transform.position);
    }
}
