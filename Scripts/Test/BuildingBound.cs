using UnityEngine;

public class BuildingBound : MonoBehaviour
{
    public bool IsExpand;
    public Vector3Int CellPosision;

    public Grid grid;

    private void Awake()
    {
        transform.position = grid.CellToWorld(CellPosision);
    }
    private void Start()
    {
        BuiltCellContainer.Instance.AddBuiltCell(this, IsExpand);
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
