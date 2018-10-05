using UnityEngine;
using UnityEngine.UI;
using EnumCollect;


public class CameraControlAndroid : MonoBehaviour
{
#if UNITY_ANDROID
    #region Utils
    private Debugger debugger = Debugger.debuger;
    #endregion
    private Camera thisCamera;
    [SerializeField]
    private int terrainWidth;
    [SerializeField]
    private int terrainHeight;

    private bool directionChosen;
    private float deltaPreMag, deltaTouchMag, deltaMagDiff;
    private Vector2 preOnePos, preTwoPos;
    private Vector2 move, startPos, direction,rotate, startPos1,startPos2,direction1,direction2;
    private Vector3 dragOrigin;
    private Touch touch, one, two;

    private Text txtSwitchCamera;
    [SerializeField]
    private EnumCameraType currentCameraType = EnumCameraType.Zoom;
    [SerializeField]
    private Vector3 resetCamera = new Vector3(90, 270, 0);

    [Range(0.1f, 2f)]
    public float DragSpeed = 1f;
    [Range(2f, 20f)]
    public float RotateSpeed = 5f;

    [Space]
    public Button BtnResetCamera;
    public Button BtnSwitchCamera;

    [Header("Terrain Data")]
    public Terrain TerrainObj;
    [Space]
    public Vector3 TopLeft, TopRight, BottomRight, BottomLeft;

    [Header("Camera Offsets")]
    public float ZoomSpeed;
    public float MaxZoomIn = 10.0f;
    public float MaxZoomOut = 120.0f;


    void Awake()
    {
        thisCamera = GetComponent<Camera>();
        BtnResetCamera.onClick.AddListener(() => resetCameraRotate());
        BtnSwitchCamera.onClick.AddListener(() => switchCameraType(currentCameraType));
        txtSwitchCamera = BtnSwitchCamera.GetComponentInChildren<Text>();
        txtSwitchCamera.text = currentCameraType.ToString();
    }

    private void Update()
    {
        moveCameraAndroid();
    }

    private void switchCameraType(EnumCameraType enumCamera)
    {
        switch (enumCamera)
        {
            case EnumCameraType.Zoom:
                currentCameraType = EnumCameraType.Rotate;
                break;
            case EnumCameraType.Rotate:
                currentCameraType = EnumCameraType.Panning;
                break;
            case EnumCameraType.Panning:
                currentCameraType = EnumCameraType.Zoom;
                break;
            default:
                break;
        }
        txtSwitchCamera.text = currentCameraType.ToString();
    }

    private void moveCameraAndroid()
    {
        if (Input.touchCount == 1)
        {
            moveTouch();
        }
        else if (Input.touchCount == 2)
        {
            /* 
             * Zoom: far range to zoom in, near range to zoom out;
             * Rotate
             * Panning: up/down for horizontal, left/right for vertical
             */
            switch (currentCameraType)
            {
                case EnumCameraType.Zoom:
                    zoomHandle();
                    break;
                case EnumCameraType.Rotate:
                    rotateHandle();
                    break;
                case EnumCameraType.Panning:

                    break;
                default:
                    break;
            }
        }
    }

    private void resetCameraRotate()
    {
        transform.rotation = Quaternion.Euler(resetCamera);
    }

    private void moveTouch()
    {
        touch = Input.GetTouch(0);
        switch (touch.phase)
        {
            case TouchPhase.Began:
                startPos = touch.position;
                //directionChosen = false;
                break;
            case TouchPhase.Moved:
                direction = touch.position - startPos;
                if (!CameraStatus.instance.InvertBool)
                    move = new Vector3(-direction.x * DragSpeed, -direction.y * DragSpeed);
                else
                    move = new Vector3(direction.x * DragSpeed, direction.y * DragSpeed);

                transform.Translate(move, Space.Self);
                break;

            case TouchPhase.Stationary:
                break;
            case TouchPhase.Ended:
                //  directionChosen = true;
                break;
            case TouchPhase.Canceled:
                break;
            default:
                break;
        }

    }

    #region Camera Gestures
    private void zoomHandle()
    {
        one = Input.GetTouch(0);
        two = Input.GetTouch(1);

        preOnePos = one.position - one.deltaPosition;
        preTwoPos = two.position - two.deltaPosition;

        deltaPreMag = (preOnePos - preTwoPos).magnitude;
        deltaTouchMag = (one.position - two.position).magnitude;

        deltaMagDiff = deltaTouchMag - deltaPreMag;
        if (deltaMagDiff != 0)
        {
            // apply zoom force
            thisCamera.fieldOfView = Mathf.Clamp(deltaMagDiff * ZoomSpeed * Time.deltaTime + thisCamera.fieldOfView, MaxZoomIn, MaxZoomOut);
        }
    }

    private void rotateHandle()
    {


        //touch = Input.GetTouch(1);
        //Vector2 deltaPos = touch.deltaPosition;
        //if (deltaPos.sqrMagnitude > 0)
        //{
        //    deltaPos *= Time.deltaTime * RotateSpeed;
        //    thisCamera.transform.Rotate(deltaPos.y, deltaPos.x, 0.0f);
        //    debugger.Log("Delta pos: " + deltaPos);
        //}
        one = Input.GetTouch(0);
        two = Input.GetTouch(1);

        switch (one.phase)
        {
            case TouchPhase.Began:
                startPos1 = one.position;
                break;
            case TouchPhase.Moved:
                break;
            case TouchPhase.Stationary:
                if (two.phase==TouchPhase.Moved)
                {
                    rotate = new Vector3(0, direction2.y*Time.deltaTime);
                }
                break;
            case TouchPhase.Ended:
                break;
            case TouchPhase.Canceled:
                break;
            default:
                break;
        }
        switch (two.phase)
        {
            case TouchPhase.Began:
                startPos2 = two.position;
                break;
            case TouchPhase.Moved:
                direction2 = two.position - startPos2;
                break;
            case TouchPhase.Stationary:
                break;
            case TouchPhase.Ended:
                break;
            case TouchPhase.Canceled:
                break;
            default:
                break;
        }
        transform.Rotate(rotate);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }
    #endregion
#endif
}
