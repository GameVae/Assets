using EnumCollect;
using Generic.CustomInput;
using Generic.Singleton;
using System.Collections;
using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    private float targetFov;
    private CameraGesture gestureType;

    public Vector3 Velocity;

    public CameraBlindInsideMap CameraBlinding;
    public CameraOption Option;

    public Camera TargetCamera;
    public Connection Conn;
    public CrossInput CrossInput;



    private void Start()
    {
            Conn = Singleton.Instance<Connection>();
            CrossInput = Singleton.Instance<CrossInput>();

        SetStartupPosition();
        targetFov = Option.Default;
    }

    private void Update()
    {
        CameraGestureHandle();
#if UNITY_EDITOR
        //ZoomHandle();
        if (Input.GetMouseButton(0))
            SwipeHandle();
#endif
        ValueUpdate();
    }


    #region Camera Set Position
    /// <summary>
    /// Cell in Real map 522 - 522
    /// </summary>
    /// <param name="cell"></param>
    public void Set(Vector3Int cell)
    {
        Vector3 worldPoint = Singleton.Instance<HexMap>().CellToWorld(cell);
        worldPoint.y = Option.Height;         // const height
        TargetCamera.transform.position = worldPoint;

        AlignCamera(worldPoint);
    }

    private void SetStartupPosition()
    {
        Vector3Int cellIndex = Conn.Sync.CurrentMainBase.Position.Parse3Int() + new Vector3Int(5, 5, 0);
        Set(cellIndex);
    }

    /// <summary>
    /// </summary>
    /// <param name="projection">Z of camera </param>
    /// <returns></returns>
    private float HaftCrossLineViewFustum(float projection)
    {
        if (Physics.Raycast(
            origin: TargetCamera.transform.position,
            direction: TargetCamera.transform.forward,
            maxDistance: TargetCamera.farClipPlane,
            hitInfo: out RaycastHit hitInfo))
        {
            return hitInfo.point.z - projection;
        }
        return 0;
    }

    private void AlignCamera(Vector3 worldPoint)
    {
        worldPoint.z -= HaftCrossLineViewFustum(worldPoint.z);
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
        Velocity += new Vector3(CrossInput.Axises.x, 0, CrossInput.Axises.y) / Time.deltaTime;
        Velocity = Velocity.Truncate(Option.SwipeMaxSpeed);
    }

    private void DetermineGesture()
    {
        switch (CrossInput.TouchCount)
        {
            case 1: gestureType = CameraGesture.Swipe; break;
            case 2: DetermineZoomAndRotate(out gestureType); break;
            default:
                gestureType = CameraGesture.None;
                break;
        }
    }

    private void DetermineZoomAndRotate(out CameraGesture type)
    {
        type = CameraGesture.Zoom;
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
}

