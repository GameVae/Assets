using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour
{
    private EventSystem eventSystem;
   
    public Grid grid;
    public CursorPos cursorPos;
    public Camera cameraRaycaster;
  
    private void Awake()
    {
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
                if(CellInfoManager.Instance.GetCellInfo(selectCell.ZToZero(),out CellInfomation result))
                {
                    cursorPos.updateCursor(result.GameObject.transform.position);
                }
                else
                {
                    cursorPos.updateCursor(hitInfo.point);
                }
            }
        }
    }
}
