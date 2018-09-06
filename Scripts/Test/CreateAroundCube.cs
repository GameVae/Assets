using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateAroundCube : MonoBehaviour
{
    private PostionTest postionTest;
    private Grid grid;

    public GameObject refabsObj;

    void Start()
    {
        postionTest = GetComponent<PostionTest>();
        grid = GetComponentInParent<Grid>();

        createTestCube(postionTest.CellPos);

    }

    private void createTestCube(Vector3Int cellPos)
    {
        Vector3Int vector3Int1 = new Vector3Int(cellPos.x, cellPos.y - 1, 0);
        Vector3Int vector3Int2 = new Vector3Int(cellPos.x - 1, cellPos.y, 0);
        Vector3Int vector3Int3 = new Vector3Int(cellPos.x, cellPos.y + 1, 0);
        
        Vector3Int vector3Int4 = new Vector3Int(cellPos.x + 1, cellPos.y - 1, 0);
        Vector3Int vector3Int5 = new Vector3Int(cellPos.x + 1, cellPos.y, 0);
        Vector3Int vector3Int6 = new Vector3Int(cellPos.x + 1, cellPos.y + 1, 0);

        Instantiate(refabsObj, grid.CellToWorld(vector3Int1), Quaternion.identity);
        Instantiate(refabsObj, grid.CellToWorld(vector3Int2), Quaternion.identity);
        Instantiate(refabsObj, grid.CellToWorld(vector3Int3), Quaternion.identity);
        Instantiate(refabsObj, grid.CellToWorld(vector3Int4), Quaternion.identity);
        Instantiate(refabsObj, grid.CellToWorld(vector3Int5), Quaternion.identity);
        Instantiate(refabsObj, grid.CellToWorld(vector3Int6), Quaternion.identity);
    }

}
