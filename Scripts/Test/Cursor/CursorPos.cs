using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class PositionCursor
{
    public Text KingdomTxt;
    public Text PositionXTxt;
    public Text PositionYTxt;

    public void SetPosTxt(string cellX, string cellY)
    {
        PositionXTxt.text = "X: " + (int.Parse(cellX) - 5);
        PositionYTxt.text = "Y: " + (int.Parse(cellY) - 5);
    }
    public void SetKingdomTxt(string kingdom)
    {
        KingdomTxt.text = kingdom;
    }
}
public class CursorPos : MonoBehaviour
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Vector3Int cursorCellPosition;

    private Vector3 tempTransform;
    [Space]
    public Vector3Int MapPosition;
    [Space]
    public PositionCursor PositionCursor;

    Ray ray;
    RaycastHit hit;
    private void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    if (!(EventSystem.current.IsPointerOverGameObject() && EventSystem.current.currentSelectedGameObject != null))
        //    {
        //        //Vector3 inputMouse = Input.mousePosition;
        //        //updateMousePos(inputMouse);
        //        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //        if (Physics.Raycast(ray, out hit))
        //        {
        //            updateCursor(hit.point);
        //        }
        //    }
        //}
    }

    public void updateCursor(Vector3 hitPoint)
    {
        cursorCellPosition = grid.WorldToCell(hitPoint);
        tempTransform = grid.CellToWorld(cursorCellPosition);
        tempTransform.y = 1;

        // GetMapPosition(cursorCellPosition);
        if ((MapPosition.x >= 0 && MapPosition.x <= 512) && (MapPosition.y >= 0 && MapPosition.y <= 512))
        {
            transform.position = tempTransform;
        }
    }

    public void GetMapPosition(Vector3Int cursorCellPos)
    {
        MapPosition.x = cursorCellPos.x - 5;
        MapPosition.y = cursorCellPos.y - 5;
        //PositionCursor.SetPosTxt(MapPosition.x+"", MapPosition.y+"");
        PositionCursor.SetPosTxt(cursorCellPos.x.ToString(), cursorCellPos.y.ToString());
    }

    public Vector3 CellToWorldPoint(Vector3Int cellPos)
    {
        return grid.CellToWorld(cellPos);
    }

    public Vector3Int GetCurrentCell()
    {
        return grid.WorldToCell(transform.position);
    }
}
