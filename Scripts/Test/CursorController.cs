using UnityEngine;
using UnityEngine.EventSystems;

public class CursorController : MonoBehaviour
{
    private EventSystem eventSystem;

    public HexMap Map;
    public CursorPos Cursor;
    public Camera CameraRaycaster;
    public CameraController CameraCtrl;

    private void Awake()
    {
        eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        if (CameraCtrl.IsTouch() &&
            !eventSystem.IsPointerOverGameObject())
        {
            Vector3 mousePos = Input.mousePosition;

            bool raycastHitted = Physics.Raycast(
                CameraRaycaster.ScreenPointToRay(mousePos),
                out RaycastHit hitInfo,
                int.MaxValue);

            if (raycastHitted)
            {
                Vector3Int selectCell = Map.WorldToCell(hitInfo.point);
                #region old
                //if(Singleton.Instance<CellInfoManager>().GetCellInfo(selectCell.ZToZero(),out CellInfo result))
                //{
                //    selectCell = grid.WorldToCell(result.GameObject.transform.position);                    
                //    Cursor.updateCursor(result.GameObject.transform.position);
                //}
                //else
                //{
                //    Cursor.updateCursor(hitInfo.point);
                //}
                #endregion

                Cursor.PositionCursor.SetPosTxt(selectCell.x.ToString(), selectCell.y.ToString());
                Cursor.updateCursor(Map.CellToWorld(selectCell));
            }
        }
    }
}
