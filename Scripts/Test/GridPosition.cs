using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class GridPosition : MonoBehaviour
{
    //private Tilemap tileMap;
    //[SerializeField]
    //private Camera cameraThis;
    //private void Start()
    //{
    //    tileMap = GetComponentInChildren<Tilemap>();

    //}

    //void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Vector3 WorldPoint = cameraThis.WorldToScreenPoint(Input.mousePosition);

    //        //Debug.Log("mouse :" + Input.mousePosition);

    //        //Debug.Log("WorldPoint :" + WorldPoint);

    //        //Debug.Log("gridPosition :" + tileMap.WorldToLocal(WorldPoint));

    //        //Vector3 localPos = tileMap.WorldToLocal(WorldPoint);

    //        Vector3 pos = tileMap.WorldToCell(WorldPoint);
    //        Debug.Log("pos :" + pos);
    //    }
    //}
    [SerializeField]
    private Grid grid;

    //private void Awake()
    //{
    //    grid = transform.parent.GetComponentInParent<Grid>();
    //}
    void Awake()
    {
        //Vector3Int cellPosition = grid.WorldToCell(transform.position);
        //Debug.Log(cellPosition);
        //transform.position = grid.CellToWorld(cellPosition);
        grid = GetComponentInParent<Grid>();
    }
   
    private Vector3 getWorldPos(Vector3Int cellPos)
    {
        Vector3 worldPos = grid.WorldToCell(cellPos);
        return worldPos;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 inputMouse = Input.mousePosition;
            inputMouse.z = Camera.main.transform.transform.position.y;
            Vector3Int WorldPoint = Vector3Int.FloorToInt(Camera.main.ScreenToWorldPoint(inputMouse));

            //Debug.Log("inputMouse: " + inputMouse.z);
            //Debug.Log("WorldPoint: "+ WorldPoint);
            //Debug.Log("getWorldPos: " + getWorldPos(WorldPoint));
        }
    }
}
