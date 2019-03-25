using Generic.CustomInput;
using Generic.Singleton;
using UI;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    private UnityEventSystem eventSystem;
    private CrossInput crossInput;

    public HexMap Map;
    public CursorPos Cursor;
    public Camera CameraRaycaster;

    private void Awake()
    {
        eventSystem = Singleton.Instance<UnityEventSystem>();
        crossInput = eventSystem.CrossInput;
    }

    private void Update()
    {
        if (crossInput.IsTouch &&
            !eventSystem.IsPointerDownOverUI)
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
