using UnityEngine;
using UnityEngine.UI;
using EnumCollect;


public class CameraControlAndroid : MonoBehaviour
{
#if UNITY_ANDROID
    #region Utils
    private Debugger debugger;
    #endregion
    private Camera thisCamera;
    [SerializeField]
    private int terrainWidth;
    [SerializeField]
    private int terrainHeight;

    private bool directionChosen;
    private Vector2 move;
    private Vector2 startPos, direction;
    private Vector3 dragOrigin;
    //private Vector3 pos, rotate, rotateR, posRightMouse, tempPos;
    private Touch touchOne;
    private Touch touchTwo;
    private Transform cameraObject;

    private Text txtSwitchCamera;
    [SerializeField]
    private EnumCameraType currentCameraType = EnumCameraType.Zoom;
    [SerializeField]
    private Vector3 resetCamera = new Vector3(90, 270, 0);

    // camera rotation fields
    private float vRotation;
    private float hRotation;

    [Header("Range of rotation")]
    public Vector2 WrapVertical;
    public Vector2 WrapHorizontal;
    public GameObject VerticalAxis;
    public GameObject HorizontalAxis;

    [Space]
    [Range(0.01f, 2f)]
    public float DragSpeed = 1f;
    [Range(1f, 20f)]
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
    public float MaxZoomOut = 60.0f;



    void Awake()
    {
        thisCamera = GetComponent<Camera>();
        BtnResetCamera.onClick.AddListener(() => resetCameraRotate());
        BtnSwitchCamera.onClick.AddListener(() => switchCameraType(currentCameraType));
        txtSwitchCamera = BtnSwitchCamera.GetComponentInChildren<Text>();
        txtSwitchCamera.text = currentCameraType.ToString();

        vRotation = 90.0f;
        hRotation = 0.0f;
        cameraObject = transform.root;
    }
    private void Start()
    {
        debugger = Debugger.instance;
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
            Move();
            //moveTouch();

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
                    ZoomHandle();
                    break;
                case EnumCameraType.Rotate:
                    RotateHandle();
                    break;
                default:
                    break;
            }
        }
    }

    private void resetCameraRotate()
    {
        // transform.rotation = Quaternion.Euler(resetCamera);
        hRotation = 0.0f;
        vRotation = 90.0f;
        VerticalAxis.transform.localRotation = Quaternion.Euler(vRotation, 0.0f, 0.0f);
        HorizontalAxis.transform.localRotation = Quaternion.Euler(0.0f, hRotation, 0.0f);
    }

    private void moveTouch()
    {
        touchOne = Input.GetTouch(0);
        switch (touchOne.phase)
        {
            case TouchPhase.Began:
                startPos = touchOne.position;
                //directionChosen = false;
                break;
            case TouchPhase.Moved:
                direction = touchOne.position - startPos;
                if (!CameraStatus.instance.InvertBool)
                    move = new Vector3(-direction.x * DragSpeed, -direction.y * DragSpeed);
                else
                    move = new Vector3(direction.x * DragSpeed, direction.y * DragSpeed);

                cameraObject.Translate(move, Space.Self);
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
    private void Move()
    {
        touchOne = Input.GetTouch(0);
        direction = touchOne.deltaPosition;
        move = new Vector3(direction.x, 0.0f, direction.y);
        cameraObject.Translate(move);
        debugger.Log("Delta position:" + move + "camera pos: " + cameraObject.transform.position);
    }

    private void ZoomHandle()
    {
        touchOne = Input.GetTouch(0);
        touchTwo = Input.GetTouch(1);

        Vector2 preOnePos = touchOne.position - touchOne.deltaPosition;
        Vector2 preTwoPos = touchTwo.position - touchTwo.deltaPosition;

        float deltaPreMag = (preOnePos - preTwoPos).magnitude;
        float deltaTouchMag = (touchOne.position - touchTwo.position).magnitude;

        float deltaMagDiff = deltaTouchMag - deltaPreMag;
        if (deltaMagDiff != 0)
        {
            // apply zoom force
            if (!thisCamera.orthographic)
            {
                thisCamera.fieldOfView = Mathf.Clamp(deltaMagDiff * ZoomSpeed * Time.deltaTime + thisCamera.fieldOfView, MaxZoomIn, MaxZoomOut);
            }
        }
    }
    private void RotateHandle()
    {
        touchOne = Input.GetTouch(0);
        touchTwo = Input.GetTouch(1);
        Vector2 deltaTouchPos = touchOne.deltaPosition.magnitude > touchTwo.deltaPosition.magnitude ?
                                    touchOne.deltaPosition : touchTwo.deltaPosition;
        float h, v;
        h = deltaTouchPos.x;
        v = deltaTouchPos.y;

        if (Mathf.Abs(v) > Mathf.Abs(h))
        {
            h = 0.0f;
        }
        else
        {
            v = 0.0f;
        }
        vRotation = Mathf.Clamp(vRotation - v * RotateSpeed, WrapVertical.x, WrapVertical.y);
        hRotation = Mathf.Clamp(hRotation + h * RotateSpeed, WrapHorizontal.x, WrapHorizontal.y);

        VerticalAxis.transform.localRotation = Quaternion.Euler(vRotation, 0.0f, 0.0f);
        //HorizontalAxis.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -hRotation);
        HorizontalAxis.transform.localRotation = Quaternion.Euler(0.0f, hRotation, 0.0f);
    }

    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        if (q.w != 1.0f)
        {
            Debug.Log(q.w);
        }
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, WrapVertical.x, WrapVertical.y);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
    #endregion
#endif
}
