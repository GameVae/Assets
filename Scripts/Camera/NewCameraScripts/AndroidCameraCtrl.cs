using UnityEngine;
using UnityEngine.UI;
using EnumCollect;
using Generic.Contants;
using Generic.Singleton;

public class AndroidCameraCtrl : MonoBehaviour
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
    [SerializeField]
    private Transform cameraObject;
    [SerializeField]
    private Vector3 defaultPosition;

    private Vector3 tempPos;

    private Touch touchOne;
    private Touch touchTwo;

    private Text txtSwitchCamera;
    [SerializeField]
    private EnumCameraType currentCameraType = EnumCameraType.Zoom;

    // camera fields
    private bool isLockedAxis;
    private float vRotTarget;
    private float hRotTarget;

    private float vRotation;
    private float hRotation;

    private Vector2 moveForce;
    private Vector3 verticalDir;
    private Vector3 horizontalDir;
    private Vector3 move;


    [Header("Range of rotation")]
    public Transform VerticalAxis;
    public Transform HorizontalAxis;
    public Vector2 WrapRotationValue;

    [Header("Camera Offsets")]
    public float ZoomSpeed;
    public float MaxZoomIn = 10.0f;
    public float MaxZoomOut = 60.0f;
    [Space]
    [Range(1f, 100.0f)]
    public float DragSpeed = 20f;
    [Range(1f, 180.0f)]
    public float VerticalRotSpeed = 70.0f;
    [Range(1f, 360.0f)]
    public float HorizontalRotSpeed = 360.0f * 0.55f;
    [Range(0.0f, 360.0f)]
    public float DegreePerInch = 360.0f;

    [Space]
    public Button BtnResetCamera;
    public Button BtnSwitchCamera;




    void Awake()
    {
        thisCamera = GetComponentInChildren<Camera>();
        BtnResetCamera.onClick.AddListener(() => ResetCameraRotate());
        BtnSwitchCamera.onClick.AddListener(() => SwitchCameraType(currentCameraType));

        txtSwitchCamera = BtnSwitchCamera.GetComponentInChildren<Text>();
        txtSwitchCamera.text = currentCameraType.ToString();

        vRotation = vRotTarget = 90.0f;
        hRotation = hRotTarget = 0.0f;
        cameraObject = transform.root;
    }
    private void Start()
    {
        //debugger = Debugger.instance;
    }

    private void /*Update() // */FixedUpdate()
    {
#if UNITY_ANDROID
        MoveCameraAndroid();
        RotateCamera();
        UnlockAxis();
#endif
#if UNITY_EDITOR
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");


        if (Mathf.Abs(v) > Mathf.Abs(h))
        {
            h = 0.0f;
        }
        else
        {
            v = 0.0f;
        }
        if (!isLockedAxis)
        {
            hRotTarget = (hRotTarget + h * Singleton.Instance<GConstants>().PixelDependencyDevice * DegreePerInch);
        }
        vRotTarget = Mathf.Clamp(vRotTarget - v * Singleton.Instance<GConstants>().PixelDependencyDevice * DegreePerInch,
                                 WrapRotationValue.x,
                                 WrapRotationValue.y);

        RotateCamera();
        // LockAxis();
        UnlockAxis();
        
        #region MOVE ON EDITOR
        //verticalDir = HorizontalAxis.up;
        //horizontalDir = VerticalAxis.right;
        //horizontalDir.y = 0; verticalDir.y = 0;

        //move = (horizontalDir * Input.GetAxis("Horizontal") * Singleton.Instance<GConstants>().PixelDependencyDevice * DragSpeed)
        //    + (verticalDir * Input.GetAxis("Vertical") * Singleton.Instance<GConstants>().PixelDependencyDevice * DragSpeed);
        //cameraObject.Translate(move);
        #endregion
        // check out of range
        checkLeftBot();
        checkRightBot();
        checkLeftTop();
        checkRightTop();
#endif
    }

    private void SwitchCameraType(EnumCameraType enumCamera)
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

    private void MoveCameraAndroid()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                Move();
            }
            else if (Input.touchCount == 2)
            {
                /* 
                 * Zoom: far range to zoom in, near range to zoom out;
                 * Rotate
                 */
                switch (currentCameraType)
                {
                    case EnumCameraType.Zoom:
                        ZoomHandle();
                        break;
                    case EnumCameraType.Rotate:
                        CalculateRotation();
                        break;
                    default:
                        break;
                }
            }
            checkLeftBot();
            checkRightBot();
            checkLeftTop();
            checkRightTop();
           // cameraObject.position = new Vector3(transform.position.x, 12, transform.position.z);
        }
    }

    private void ResetCameraRotate()
    {
        // transform.rotation = Quaternion.Euler(resetCamera);
        hRotation = hRotTarget = 0.0f;
        vRotation = vRotTarget = 90.0f;
        VerticalAxis.transform.localRotation = Quaternion.Euler(vRotTarget, 0.0f, 0.0f);
        HorizontalAxis.transform.localRotation = Quaternion.Euler(0.0f, hRotTarget, 0.0f);
        // cameraObject.position = defaultPosition;
    }

    #region Camera Gestures
    private void Move()
    {
        touchOne = Input.GetTouch(0);

        verticalDir = Vector3.ProjectOnPlane(HorizontalAxis.up, Vector3.up);
        horizontalDir = Vector3.ProjectOnPlane(VerticalAxis.right, Vector3.up);

        moveForce = -touchOne.deltaPosition;

        move = (horizontalDir * moveForce.x + verticalDir * moveForce.y)
                * DragSpeed
                * Singleton.Instance<GConstants>().PixelDependencyDevice;


        cameraObject.Translate(move);
        // debugger.Log("Delta position:" + move + "camera pos: " + cameraObject.transform.position + " Dpi unit: " + Singleton.Instance<GConstants>().PixelDependencyDevice);
    }

    private void ZoomHandle()
    {
        touchOne = Input.GetTouch(0);
        touchTwo = Input.GetTouch(1);

        Vector2 preOnePos = touchOne.position - touchOne.deltaPosition;
        Vector2 preTwoPos = touchTwo.position - touchTwo.deltaPosition;

        float deltaPreMag = (preOnePos - preTwoPos).magnitude;
        float deltaTouchMag = (touchOne.position - touchTwo.position).magnitude;

        float zoomForce = deltaTouchMag - deltaPreMag;
        if (zoomForce != 0)
        {
            // apply zoom force
            if (!thisCamera.orthographic)
            {
                thisCamera.fieldOfView = Mathf.Clamp(zoomForce * ZoomSpeed * Singleton.Instance<GConstants>().PixelDependencyDevice + thisCamera.fieldOfView,
                                        MaxZoomIn,
                                        MaxZoomOut);
            }
        }
    }
    private void CalculateRotation()
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
        if (!isLockedAxis)
        {
            hRotTarget = (hRotTarget + h * Singleton.Instance<GConstants>().PixelDependencyDevice * DegreePerInch);
        }
        vRotTarget = Mathf.Clamp(vRotTarget - v * Singleton.Instance<GConstants>().PixelDependencyDevice * DegreePerInch,
                                 WrapRotationValue.x,
                                 WrapRotationValue.y);
        //LockAxis();
    }

    private void RotateCamera()
    {
        if (vRotation != vRotTarget)
        {
            vRotation = Mathf.MoveTowards(vRotation, vRotTarget, Time.deltaTime * VerticalRotSpeed);
            // debugger.Log("V rotatio target: " + vRotTarget + " | V rotation: " + vRotation + " Is locked axis: " + isLockedAxis);

            // VerticalAxis.transform.localRotation = Quaternion.Euler(vRotation, 0.0f, 0.0f);
            VerticalAxis.transform.Rotate(new Vector3(-vRotation * DragSpeed, 0, 0));
            VerticalAxis.transform.eulerAngles = new Vector3(VerticalAxis.transform.eulerAngles.x, VerticalAxis.transform.eulerAngles.y, 0);

        }
        if (hRotation != hRotTarget)
        {
            hRotation = Mathf.MoveTowards(hRotation, hRotTarget, Time.deltaTime * HorizontalRotSpeed);
            // debugger.Log("H rotation target: " + hRotTarget + " | H rotation: " + hRotation + "Is locked axis: " + isLockedAxis);

            // HorizontalAxis.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -hRotation);

            HorizontalAxis.transform.Rotate(new Vector3(0, -hRotation * DragSpeed, 0));
            HorizontalAxis.transform.eulerAngles = new Vector3(HorizontalAxis.transform.eulerAngles.x, HorizontalAxis.transform.eulerAngles.y, 0);

        }
        //VerticalAxis.transform.Rotate(new Vector3(-vRotation * DragSpeed, 0, 0));
        //HorizontalAxis.transform.Rotate(new Vector3(0, -hRotation * DragSpeed, 0));
        //VerticalAxis.transform.eulerAngles = new Vector3(VerticalAxis.transform.eulerAngles.x, VerticalAxis.transform.eulerAngles.y, 0);
        //HorizontalAxis.transform.eulerAngles = new Vector3(HorizontalAxis.transform.eulerAngles.x, HorizontalAxis.transform.eulerAngles.y, 0);
    }
    #endregion

    private void UnlockAxis()
    {
        if (vRotation >= (WrapRotationValue.y - 2.0f) && isLockedAxis)
        {
            isLockedAxis = false;
        }
    }
    private void LockAxis()
    {

        // 2.0f is deviation allowed
        if (vRotTarget <= (WrapRotationValue.y - 2.0f) && !isLockedAxis)
        {
            hRotation %= 360.0f;

            hRotTarget = 0.0f;

            isLockedAxis = true;
        }
    }
    private void checkLeftTop()
    {
        //tempPos.y = thisCamera.transform.position.y;
        tempPos.y = cameraObject.position.y;
        //Gizmos.DrawSphere(thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)), 1);//left top
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).x < 0)
        {
            //tempPos.x = (thisCamera.transform.position.x - thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).x);
            //tempPos.z = thisCamera.transform.position.z;
            //thisCamera.transform.position = tempPos;

            // new
            tempPos.x = (cameraObject.position.x - thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).x);
            tempPos.z = cameraObject.position.z;
            cameraObject.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).x > terrainWidth)
        {
            //tempPos.x = thisCamera.transform.position.x - (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).x - terrainWidth);
            //tempPos.z = thisCamera.transform.position.z;
            //thisCamera.transform.position = tempPos;

            // new
            tempPos.x = cameraObject.position.x - (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).x - terrainWidth);
            tempPos.z = cameraObject.position.z;
            cameraObject.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).z < 0)
        {
            //tempPos.x = thisCamera.transform.position.x;
            //tempPos.z = thisCamera.transform.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).z);
            //thisCamera.transform.position = tempPos;

            // new
            tempPos.x = cameraObject.position.x;
            tempPos.z = cameraObject.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).z);
            cameraObject.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).z > terrainHeight)
        {
            //tempPos.x = thisCamera.transform.position.x;
            //tempPos.z = thisCamera.transform.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).z - terrainHeight);
            //thisCamera.transform.position = tempPos;

            // new
            tempPos.x = cameraObject.position.x;
            tempPos.z = cameraObject.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).z - terrainHeight);
            cameraObject.position = tempPos;
        }
    }

    private void checkRightTop()
    {
        //tempPos.y = thisCamera.transform.position.y;

        tempPos.y = cameraObject.position.y;

        //Gizmos.DrawSphere(thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)), 1);// right top
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).x < 0)
        {
            //tempPos.x = (thisCamera.transform.position.x - thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).x );
            //tempPos.z = thisCamera.transform.position.z;
            //thisCamera.transform.position = tempPos;

            // new
            tempPos.x = (cameraObject.position.x - thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).x);
            tempPos.z = cameraObject.position.z;
            cameraObject.position = tempPos;

        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).x > terrainWidth)
        {
            //tempPos.x = thisCamera.transform.position.x - (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).x - terrainWidth);
            //tempPos.z = thisCamera.transform.position.z;
            //thisCamera.transform.position = tempPos;

            // new
            tempPos.x = cameraObject.position.x - (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).x - terrainWidth);
            tempPos.z = cameraObject.position.z;
            cameraObject.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).z < 0)
        {
            //tempPos.x = thisCamera.transform.position.x;
            //tempPos.z = thisCamera.transform.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).z);
            //thisCamera.transform.position = tempPos;

            // new
            tempPos.x = cameraObject.position.x;
            tempPos.z = cameraObject.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).z);
            cameraObject.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).z > terrainHeight)
        {
            //tempPos.x = thisCamera.transform.position.x;
            //tempPos.z = thisCamera.transform.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).z - terrainHeight);
            //thisCamera.transform.position = tempPos;

            // new 
            tempPos.x = cameraObject.position.x;
            tempPos.z = cameraObject.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).z - terrainHeight);
            cameraObject.position = tempPos;
        }
    }

    private void checkRightBot()
    {
        tempPos.y = cameraObject.position.y;
        //Gizmos.DrawSphere(thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)), 1);//right bot
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).x < 0)
        {
            tempPos.x = (cameraObject.position.x - thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).x);
            tempPos.z = cameraObject.position.z;
            cameraObject.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).x > terrainWidth)
        {
            tempPos.x = cameraObject.position.x - (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).x - terrainWidth);
            tempPos.z = cameraObject.position.z;
            cameraObject.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).z < 0)
        {
            tempPos.x = cameraObject.position.x;
            tempPos.z = cameraObject.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).z);
            cameraObject.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).z > terrainHeight)
        {
            tempPos.x = cameraObject.position.x;
            tempPos.z = cameraObject.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).z - terrainHeight);
            cameraObject.position = tempPos;
        }

    }

    private void checkLeftBot()
    {
        tempPos.y = cameraObject.position.y;
        //Gizmos.DrawSphere(thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)), 1);//left bot
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).x < 0)
        {
            tempPos.x = (cameraObject.position.x - thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).x);
            tempPos.z = cameraObject.position.z;
            cameraObject.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).x > terrainWidth)
        {
            tempPos.x = cameraObject.position.x - (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).x - terrainWidth);
            tempPos.z = cameraObject.position.z;
            cameraObject.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).z < 0)
        {
            tempPos.x = cameraObject.position.x;
            tempPos.z = cameraObject.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).z);
            cameraObject.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).z > terrainHeight)
        {
            tempPos.x = cameraObject.position.x;
            tempPos.z = cameraObject.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).z - terrainHeight);
            cameraObject.position = tempPos;
        }

    }

    void OnDrawGizmos()
    {
        thisCamera = GetComponentInChildren<Camera>();
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)), 1);//left top
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)), 1);//left bot
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)), 1);//right bot
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)), 1);// right top
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(thisCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 2 * thisCamera.orthographicSize)), 1);// center
    }
#endif

#if UNITY_EDITOR
    [ContextMenu("Get rotation properties")]
    public void GetProperties()
    {
        if(thisCamera == null)
        {
            thisCamera = GetComponentInChildren<Camera>();
        }
        Debug.Log("Camera quaternion: " + thisCamera.transform.rotation);
        Debug.Log("Camera Euler:      " + thisCamera.transform.rotation.eulerAngles);
        Debug.Log("Camera Local Quaternion: " + thisCamera.transform.localRotation);
        Debug.Log("Camera Local Euler: " + thisCamera.transform.localEulerAngles);
    }
#endif
}

