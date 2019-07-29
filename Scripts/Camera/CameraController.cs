using EnumCollect;
using Generic.CustomInput;
using Generic.Singleton;
using UI;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    private float direction;
    private float targetFov;
    private Vector3 velocity;
    private Vector3 accelerate;
    private CameraGesture gestureType;

    private UnityAction cameraChanged;

    private UnityEventSystem eventSystem;
    private NestedCondition swipeConditions;
    private Connection conn;

    [SerializeField] private CameraOption option;
    public CameraBlindInsideMap CameraBinding;

    public Connection Conn
    {
        get { return conn ?? (conn = Singleton.Instance<Connection>()); }
    }
    public Camera TargetCamera
    {
        get
        {
            return CameraBinding.TargetCamera;
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
    public event UnityAction CameraChanged
    {
        add { cameraChanged += value; }
        remove { cameraChanged -= value; }
    }

    public CameraGesture Gesture
    {
        get { return gestureType; }
    }

    public void Awake()
    {
        CameraChanged += CameraBinding.CalculateBound;
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
#if (UNITY_ANDROID && !UNITY_EDITOR) 
        CameraGestureHandle();
#else
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
        AlignCamera(worldPoint);
        cameraChanged?.Invoke();
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
        {
            //CameraBinding.CalculateBound();
            //cameraChanged?.Invoke();
        }
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
        //Vector3 accelerate = new Vector3(CrossInput.Axises.x, 0, CrossInput.Axises.y);

        accelerate.x = CrossInput.Axises.x;
        accelerate.z = CrossInput.Axises.y;

        velocity += (accelerate * direction) / Time.deltaTime;
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
        if (velocity != Vector3.zero)
        {
            Vector3 position = TargetCamera.transform.position;
            position += velocity * Time.deltaTime;
            TargetCamera.transform.position = position;

            cameraChanged?.Invoke();
        }
    }

    private void VelocityValueUpdate()
    {
        if (velocity.magnitude > option.SwipeMinSpeed)
            velocity -= velocity.normalized * option.SwipeMaxSpeed * Time.deltaTime;
        else
            velocity = Vector3.zero;

    }
    #endregion

    #region Camera Zoom In Out
    private void FovValueUpdate()
    {
        if (TargetCamera.fieldOfView != targetFov)
        {
            TargetCamera.fieldOfView = Mathf.SmoothStep(TargetCamera.fieldOfView, targetFov, option.ZoomSmoothValue);
            cameraChanged?.Invoke();
        }
    }
    #endregion
    #endregion

#if UNITY_EDITOR
    public Vector3Int Position;
    [ContextMenu("Set Camera")]
    public void Set()
    {
        Set(Position);
    }
#endif
}

