using EnumCollect;
using Generic.CustomInput;
using Generic.Singleton;
using UI;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float direction;
    private float targetFov;
    private CameraGesture gestureType;

    private CrossInput crossInput;
    private UnityEventSystem eventSystem;
    private NestedCondition swipeConditions;

    public Vector3 Velocity;
    public CameraOption Option;
    public CameraBlindInsideMap CameraBlinding;

    public Camera TargetCamera;
    public Connection Conn;

    public CameraGesture Gesture
    {
        get { return gestureType; }
    }

    private void Start()
    {
        eventSystem = Singleton.Instance<UnityEventSystem>();
        Conn = Singleton.Instance<Connection>();
        crossInput = eventSystem.CrossInput;

        SetStartupPosition();
        targetFov = Option.DefaultFov;
        direction = 1;


        swipeConditions = new NestedCondition();
        swipeConditions.Conditions += delegate
        {
            return crossInput.CurrentState == CrossInput.PointerState.Swipe && !eventSystem.IsPointerDownOverUI;
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
        worldPoint.y = Option.Height;         // const height
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
            value: targetFov + crossInput.ZoomValue().Wrap(-Option.MaxZoomValue, Option.MaxZoomValue),
            min: Option.FovClampValue.x,
            max: Option.FovClampValue.y);
    }

    private void SwipeHandle()
    {
        Velocity += (new Vector3(crossInput.Axises.x, 0, crossInput.Axises.y) * direction) / Time.deltaTime;
        Velocity = Velocity.Truncate(Option.SwipeMaxSpeed);
    }

    private void DetermineGesture()
    {
        switch (crossInput.TouchCount)
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
        targetFov = Option.DefaultFov;
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
        position += Velocity * Time.deltaTime;
        TargetCamera.transform.position = position;
    }

    private void VelocityValueUpdate()
    {
        if (Velocity.magnitude > Option.SwipeMinSpeed)
            Velocity -= Velocity.normalized * Option.SwipeMaxSpeed * Time.deltaTime;
        else Velocity = Vector3.zero;

    }
    #endregion

    #region Camera Zoom In Out
    private void FovValueUpdate()
    {
        TargetCamera.fieldOfView = Mathf.SmoothStep(TargetCamera.fieldOfView, targetFov, Option.ZoomSmoothValue);
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

