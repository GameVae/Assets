using EnumCollect;
using Generic.CustomInput;
using Generic.Singleton;
using UI;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float direction;
    private float targetFov;
    private Vector3 velocity;
    private CameraGesture gestureType;

    private UnityEventSystem eventSystem;
    private NestedCondition swipeConditions;
    private Connection conn;

    [SerializeField] private CameraOption option;
    public CameraBlindInsideMap CameraBlinding;

    public Connection Conn
    {
        get { return conn ?? (conn = Singleton.Instance<Connection>()); }
    }
    public Camera TargetCamera
    {
        get
        {
            return CameraBlinding.TargetCamera;
        }
    }
    public CrossInput CrossInput
    {
        get { return EventSystem.CrossInput; }
    }
    public UnityEventSystem EventSystem
    {
        get
        {
            return eventSystem ?? (eventSystem = Singleton.Instance<UnityEventSystem>());
        }
    }

    public CameraGesture Gesture
    {
        get { return gestureType; }
    }

    private void Start()
    {
        SetStartupPosition();
        targetFov = option.DefaultFov;
        direction = -1;

        swipeConditions = new NestedCondition();
        swipeConditions.Conditions += delegate
        {
            return CrossInput.CurrentState == CrossInput.PointerState.Swipe && 
            !EventSystem.IsPointerDownOverUI;
        };
    }

    private void Update()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        CameraGestureHandle();
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
        StanaloneGestureHandle();
#endif
        ValueUpdate();
    }

    private void StanaloneGestureHandle()
    {
        ZoomHandle();
        if (swipeConditions.Evaluate())
        {
            SwipeHandle();
        }
    }

    #region Camera Set Position
    /// <summary>
    /// Set position in world unit, that position is seen as center
    /// </summary>
    /// <param name="cell">Cell in Real map 522 - 522</param>
    public void Set(Vector3Int cell)
    {
        Vector3 worldPoint = Singleton.Instance<HexMap>().CellToWorld(cell);
        worldPoint.y = option.Height;         // const height
        TargetCamera.transform.position = worldPoint;
        CameraBlinding.CalculateBound();
        AlignCamera(worldPoint);
    }

    private void SetStartupPosition()
    {
        Vector3Int cellIndex = Conn.Sync.CurrentMainBase.Position.Parse3Int().ToClientPosition();
        Set(cellIndex);
    }

    /// <summary>
    /// </summary>
    /// <param name="worldPoint">position of camera </param>
    /// <returns>Set camera position without change height</returns>
    private Vector3 HaftCrossLineViewFustum(Vector3 worldPoint)
    {
        if (Physics.Raycast(
            origin: TargetCamera.transform.position,
            direction: TargetCamera.transform.forward,
            maxDistance: TargetCamera.farClipPlane,
            hitInfo: out RaycastHit hitInfo))
        {
            return hitInfo.point - worldPoint;
        }
        return Vector3.zero;
    }

    private void AlignCamera(Vector3 worldPoint)
    {
        float h = worldPoint.y;
        worldPoint -= HaftCrossLineViewFustum(worldPoint);
        worldPoint.y = h;
        TargetCamera.transform.position = worldPoint;
    }
    #endregion

    #region Camera Gesture
    private void CameraGestureHandle()
    {
        DetermineGesture();
        switch (gestureType)
        {
            case CameraGesture.Zoom:
                ZoomHandle(); break;
            case CameraGesture.Swipe:
                SwipeHandle(); break;
        }
        if (gestureType == CameraGesture.Zoom || gestureType == CameraGesture.Rotate)
            CameraBlinding.CalculateBound();
    }

    private void ZoomHandle()
    {
        targetFov = Mathf.Clamp(
            value: targetFov + CrossInput.ZoomValue().Wrap(-option.MaxZoomValue, option.MaxZoomValue),
            min: option.FovClampValue.x,
            max: option.FovClampValue.y);
    }

    private void SwipeHandle()
    {
        velocity += (new Vector3(CrossInput.Axises.x, 0, CrossInput.Axises.y) * direction) / Time.deltaTime;
        velocity = velocity.Truncate(option.SwipeMaxSpeed);
    }

    private void DetermineGesture()
    {
        switch (CrossInput.TouchCount)
        {
            case 1: DetermineSwipe(ref gestureType); break;
            case 2: DetermineZoomAndRotate(ref gestureType); break;
            default:
                gestureType = CameraGesture.None;
                break;
        }
    }

    private void DetermineZoomAndRotate(ref CameraGesture type)
    {
        type = CameraGesture.Zoom;
    }

    private void DetermineSwipe(ref CameraGesture type)
    {
        //if (type == CameraGesture.Touch && CrossInput.Axises.magnitude / Time.deltaTime >= Option.SwipeMinSpeed)
        //if (type == CameraGesture.Touch && swipeConditions.Evaluate())
        if (swipeConditions.Evaluate())
            type = CameraGesture.Swipe;
        else type = CameraGesture.None;
        // Debugger.Log("swipe vel: " + CrossInput.Axises.magnitude / Time.deltaTime + " axises: " + CrossInput.Axises);
    }

    public void InvertSwipeDirection()
    {
        direction *= -1;
    }

    public void ResetCamera()
    {
        targetFov = option.DefaultFov;
    }
    #endregion

    #region Value update
    private void ValueUpdate()
    {
        FovValueUpdate();

        PositionValueUpdate();

        VelocityValueUpdate();

    }

    #region  Camera Move

    private void PositionValueUpdate()
    {
        Vector3 position = TargetCamera.transform.position;
        position += velocity * Time.deltaTime;
        TargetCamera.transform.position = position;
    }

    private void VelocityValueUpdate()
    {
        if (velocity.magnitude > option.SwipeMinSpeed)
            velocity -= velocity.normalized * option.SwipeMaxSpeed * Time.deltaTime;
        else velocity = Vector3.zero;

    }
    #endregion

    #region Camera Zoom In Out
    private void FovValueUpdate()
    {
        TargetCamera.fieldOfView = Mathf.SmoothStep(TargetCamera.fieldOfView, targetFov, option.ZoomSmoothValue);
    }
    #endregion
    #endregion

    public Vector3Int Position;
    [ContextMenu("Set Camera")]
    public void Set()
    {
        Set(Position);
    }
}

