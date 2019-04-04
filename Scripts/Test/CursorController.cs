using Generic.CustomInput;
using Generic.Singleton;
using UI;
using UnityEngine;
using UnityEngine.Events;

public class CursorController : MonoBehaviour
{
    private NestedCondition selectConditions;
    private UnityAction selectedCallback;

    private UnityEventSystem eventSystem;
    private CrossInput crossInput;

    private Popup popupIns;
    private TowerNodeManager towerPositions;

    public HexMap MapIns;
    public CursorPos Cursor;
    public CameraController CameraController;
    private Camera CameraRaycaster;

    public event UnityAction SelectedCallback
    {
        add     { selectedCallback += value; }
        remove  { selectedCallback -= value; }
    }
    public Vector3Int SelectedPosition
    {
        get; private set;
    }

    private void Awake()
    {
        popupIns = Singleton.Instance<Popup>();
        eventSystem = Singleton.Instance<UnityEventSystem>();
        crossInput = eventSystem.CrossInput;

        selectConditions = new NestedCondition();
        selectConditions.Conditions +=
            delegate
            {
                return crossInput.IsTouch && !eventSystem.IsPointerDownOverUI;
            };
        selectConditions.Conditions +=
            delegate
            {
                return CameraController.Gesture == EnumCollect.CameraGesture.None;
            };

        SelectedCallback += UpdateCursor;
        CameraRaycaster = CameraController.TargetCamera;
    }

    private void Start()
    {
        towerPositions = Singleton.Instance<GlobalNodeManager>().TowerNode;
    }

    private void Update()
    {
        if (selectConditions.Evaluate())
        {
            Vector3 mousePos = Input.mousePosition;

            bool raycastHitted = Physics.Raycast(
                CameraRaycaster.ScreenPointToRay(mousePos),
                out RaycastHit hitInfo,
                CameraRaycaster.farClipPlane);

            if (raycastHitted)
            {
                SelectedPosition = MapIns.WorldToCell(hitInfo.point).ZToZero();

                DetermineSelectedOnTower();
                DetermineSelectedOnRSS(hitInfo);

                selectedCallback?.Invoke();
            }
        }
    }

    private void UpdateCursor()
    {
        Cursor.PositionCursor.SetPosTxt(SelectedPosition.x.ToString(), SelectedPosition.y.ToString());
        Cursor.updateCursor(MapIns.CellToWorld(SelectedPosition));
    }
    private void DetermineSelectedOnTower()
    {
        if (towerPositions.GetInfo(SelectedPosition, out NodeInfo result))
        {
            SelectedPosition = MapIns.WorldToCell(result.GameObject.transform.position).ZToZero();
        }
    }
    private void DetermineSelectedOnRSS(RaycastHit hitInfo)
    {
        bool isRss = LayerMask.NameToLayer("RSS") == hitInfo.transform.gameObject.layer;
        if (isRss)
        {
            hitInfo.transform.GetComponent<NaturalResource>()?.OpenPopup(popupIns);
        }
    }


    public Vector3Int Position;
    [ContextMenu("Set Position")]
    public void SetPosition()
    {
        Cursor.updateCursor(MapIns.CellToWorld(Position));
    }
}
