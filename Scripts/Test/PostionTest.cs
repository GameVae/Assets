using UnityEngine;

[ExecuteInEditMode]
public class PostionTest : MonoBehaviour
{
    public Vector3Int CellPos;
    [SerializeField]
    private Grid grid;

    private void Awake()
    {
        if(grid == null)
        grid = FindObjectOfType<Grid>();
    }
    void Start()
    {
        UpdateCellPosition();
    }
    public void UpdateCellPosition()
    {
        Vector3 transformVec = grid.CellToWorld(CellPos);
        transformVec.y = 0.5f;

        transform.position = transformVec;
    }
}
