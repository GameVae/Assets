using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorPos : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Vector3Int cursorCellPosition;
    public Vector3Int MapPosition;

    

    void Awake()
    {
        grid = GetComponentInParent<Grid>();
    }
   
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 inputMouse = Input.mousePosition;
            updateMousePos(inputMouse);
        }
    }

    private void updateMousePos(Vector3 pos)
    {
        Vector3 inputMouse = Input.mousePosition;
        inputMouse.z = Camera.main.transform.transform.position.y;

        Vector3 WorldPoint = Camera.main.ScreenToWorldPoint(inputMouse);
        cursorCellPosition = grid.WorldToCell(WorldPoint);

        Vector3 tempTransform = grid.CellToWorld(cursorCellPosition);
        tempTransform.y = 1;

        transform.position = tempTransform;
        GetMapPosition(cursorCellPosition);
    }

    public void GetMapPosition(Vector3Int cursorCellPos)
    {
        MapPosition.x = cursorCellPos.x - 5;
        MapPosition.y = cursorCellPos.y - 5;
       
    }
}
