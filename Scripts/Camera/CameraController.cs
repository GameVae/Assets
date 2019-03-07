using EnumCollect;
using Generic.CustomInput;
using Generic.Singleton;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float direction;
    private float targetFov;
    private CameraGesture gestureType;

    public Vector3 Velocity;

    public CameraBlindInsideMap CameraBlinding;
    public CameraOption Option;

    public Camera TargetCamera;
    public Connection Conn;
    public CrossInput CrossInput;
    public NavAgentController AgentCtrl;

    public CameraGesture Gesture
    {
        get { return gestureType; }
    }

    private void Start()
    {
        Conn = Singleton.Instance<Connection>();
        CrossInput = Singleton.Instance<CrossInput>();

        AgentCtrl = Singleton.Instance<NavAgentController>();
        AgentCtrl.AddMoveCondition(IsTouch);

        SetStartupPosition();
        targetFov = Option.DefaultFov;
        direction = 1;
    }

    private void Update()
    {
        CameraGestureHandle();
#if UNITY_EDITOR
        ZoomHandle();
        if (Input.GetMouseButton(0) && CrossInput.SwipeDirection != Vector2.zero)
        {
            SwipeHandle();
        }
#endif
        ValueUpdate();
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
        Vector3Int cellIndex = Conn.Sync.CurrentMainBase.Position.Parse3Int() + new Vector3Int(5, 5, 0);
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
            value: targetFov + CrossInput.ZoomValue().Wrap(-Option.MaxZoomValue, Option.MaxZoomValue),
            min: Option.FovClampValue.x,
            max: Option.FovClampValue.y);
    }

    private void SwipeHandle()
    {
        Velocity += (new Vector3(CrossInput.Axises.x, 0, CrossInput.Axises.y) * direction) / Time.deltaTime;
        Velocity = Velocity.Truncate(Option.SwipeMaxSpeed);
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
        if (type == CameraGesture.None)
            type = CameraGesture.Touch;
        if (type == CameraGesture.Touch && CrossInput.Axises.magnitude / Time.deltaTime >= Option.SwipeMinSpeed)
            type = CameraGesture.Swipe;
        // Debugger.Log("swipe vel: " + CrossInput.Axises.magnitude / Time.deltaTime + " axises: " + CrossInput.Axises);
    }

    //  TO DO: test
    public void IncreaseZoom()
    {
        Option.SwipeMaxSpeed += 1f;
        //    Debugger.Log(Option.ZoomSmoothValue);
    }

    public void DecreaseZoom()
    {
        Option.SwipeMaxSpeed -= 1f;
        //Debugger.Log(Option.ZoomSmoothValue);
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

    public bool IsTouch()
    {
#if UNITY_EDITOR
        return CrossInput.Axises.magnitude / Time.deltaTime <= Option.SwipeMinSpeed;
#endif
#if !UNITY_EDITOR && UNITY_ANDROID
        return gestureType == CameraGesture.Touch;
#endif
    }
}

