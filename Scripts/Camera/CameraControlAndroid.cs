using UnityEngine;
using UnityEngine.UI;
using EnumCollect;


public class CameraControlAndroid : MonoBehaviour
{
    #region Utils
    private Debugger debugger;
    #endregion
    private Camera thisCamera;
    [SerializeField]
    private int terrainWidth;
    [SerializeField]
    private int terrainHeight;

    private bool directionChosen;
    private float deltaPreMag, deltaTouchMag, deltaMagDiff;
    private Vector2 preOnePos, preTwoPos;

    private Vector2 move, startPos, direction, rotate, startPos1, startPos2, direction1, direction2;
    private Vector3 dragOrigin;
    private Touch touch, one, two;
    private Vector3 tempPos;

    private Text txtSwitchCamera;
    [SerializeField]
    private EnumCameraType currentCameraType = EnumCameraType.Rotate;
    [SerializeField]
    private Vector3 resetCamera = new Vector3(90, 270, 0);

    [Range(0.1f, 2f)]
    public float DragSpeed = 1f;
    [Range(2f, 20f)]
    public float RotateSpeed = 5f;

    [Space]
    public Button BtnResetCamera;
    public Button BtnSwitchCamera;

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
    private void Start()
    {
        debugger = Debugger.instance;
    }
    private void Update()
    {
        moveCameraAndroid();
        if (Input.touchCount > 0)
        {
            checkLeftBot();
            checkRightBot();
            checkLeftTop();
            checkRightTop();
        }
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
            moveTouch();
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
                    zoomHandle();
                    break;
                case EnumCameraType.Rotate:
                    rotateHandle();
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
        move = Vector3.zero;
        direction = Vector3.zero;
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
                break;

            case TouchPhase.Stationary:
                move = Vector3.zero;
                break;
            case TouchPhase.Ended:
                //  directionChosen = true;
                direction = Vector3.zero;
                move = Vector3.zero;
                break;
            case TouchPhase.Canceled:
                direction = Vector3.zero;
                move = Vector3.zero;
                break;
            default:
                break;
        }
        transform.Translate(move, Space.Self);
        transform.position = new Vector3(transform.position.x, 20, transform.position.z);
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
            if (!thisCamera.orthographic)
            {
                thisCamera.fieldOfView = Mathf.Clamp(deltaMagDiff * ZoomSpeed * Constants.PixelDependencyDevice + thisCamera.fieldOfView,
                                        MaxZoomIn,
                                        MaxZoomOut);
            }
        }
    }
    private void zeroParam()
    {
        direction1 = Vector3.zero;
        direction2 = Vector3.zero;
        rotate = Vector3.zero;
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
                if (two.phase == TouchPhase.Moved)
                {
                    //rotate = new Vector3(0, direction2.y * Time.deltaTime, 0.0f);
                    rotate = two.deltaPosition;
                }
                break;
            case TouchPhase.Ended:
                zeroParam();
                break;
            case TouchPhase.Canceled:
                zeroParam();
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
                //direction2 = two.position - startPos2;
                direction2 = two.deltaPosition;
                break;
            case TouchPhase.Stationary:
                //if (one.phase == TouchPhase.Moved)
                //{
                //    rotate = new Vector3(0, direction1.y * Time.deltaTime, 0.0f);
                //}
                break;
            case TouchPhase.Ended:
                zeroParam();
                break;
            case TouchPhase.Canceled:
                zeroParam();
                break;
            default:
                break;
        }
        debugger.Log("current rotation: " + thisCamera.transform.rotation.eulerAngles + " | plus rotate: " + rotate);
        rotate = new Vector3(-rotate.y * DragSpeed, rotate.x * DragSpeed, 0);
        transform.Rotate(rotate);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        if (transform.localEulerAngles.x<30)
        {
            transform.localEulerAngles = new Vector3(30, transform.localEulerAngles.y, 0);
        }
        if (transform.localEulerAngles.x > 90)
        {
            transform.localEulerAngles = new Vector3(90, transform.localEulerAngles.y, 0);
        }
        //if (transform.rotation.eulerAngles.x > 80 && transform.rotation.eulerAngles.x < 100)
        //{
        //    transform.Rotate(rotate);
        //    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        //    transform.position = new Vector3(transform.position.x, 20, transform.position.z);
        //}

    }
    #endregion

    private void checkLeftTop()
    {
        tempPos.y = thisCamera.transform.position.y;
        //Gizmos.DrawSphere(thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)), 1);//left top
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).x < 0)
        {
            tempPos.x = (thisCamera.transform.position.x - thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).x);
            tempPos.z = thisCamera.transform.position.z;
            thisCamera.transform.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).x > terrainWidth)
        {
            tempPos.x = thisCamera.transform.position.x - (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).x - terrainWidth);
            tempPos.z = thisCamera.transform.position.z;
            thisCamera.transform.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).z < 0)
        {
            tempPos.x = thisCamera.transform.position.x;
            tempPos.z = thisCamera.transform.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).z);
            thisCamera.transform.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).z > terrainHeight)
        {
            tempPos.x = thisCamera.transform.position.x;
            tempPos.z = thisCamera.transform.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(0, 1, 2 * thisCamera.orthographicSize)).z - terrainHeight);
            thisCamera.transform.position = tempPos;
        }
    }

    private void checkRightTop()
    {

        tempPos.y = thisCamera.transform.position.y;
        //Gizmos.DrawSphere(thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)), 1);// right top
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).x < 0)
        {
            tempPos.x = (thisCamera.transform.position.x - thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).x);
            tempPos.z = thisCamera.transform.position.z;
            thisCamera.transform.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).x > terrainWidth)
        {
            tempPos.x = thisCamera.transform.position.x - (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).x - terrainWidth);
            tempPos.z = thisCamera.transform.position.z;
            thisCamera.transform.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).z < 0)
        {
            tempPos.x = thisCamera.transform.position.x;
            tempPos.z = thisCamera.transform.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).z);
            thisCamera.transform.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).z > terrainHeight)
        {
            tempPos.x = thisCamera.transform.position.x;
            tempPos.z = thisCamera.transform.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(1, 1, 2 * thisCamera.orthographicSize)).z - terrainHeight);
            thisCamera.transform.position = tempPos;
        }
    }

    private void checkRightBot()
    {

        tempPos.y = thisCamera.transform.position.y;
        //Gizmos.DrawSphere(thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)), 1);//right bot
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).x < 0)
        {
            tempPos.x = (thisCamera.transform.position.x - thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).x);
            tempPos.z = thisCamera.transform.position.z;
            thisCamera.transform.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).x > terrainWidth)
        {
            tempPos.x = thisCamera.transform.position.x - (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).x - terrainWidth);
            tempPos.z = thisCamera.transform.position.z;
            thisCamera.transform.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).z < 0)
        {
            tempPos.x = thisCamera.transform.position.x;
            tempPos.z = thisCamera.transform.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).z);
            thisCamera.transform.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).z > terrainHeight)
        {
            tempPos.x = thisCamera.transform.position.x;
            tempPos.z = thisCamera.transform.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(1, 0, 2 * thisCamera.orthographicSize)).z - terrainHeight);
            thisCamera.transform.position = tempPos;
        }

    }

    private void checkLeftBot()
    {

        tempPos.y = thisCamera.transform.position.y;
        //Gizmos.DrawSphere(thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)), 1);//left bot
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).x < 0)
        {
            tempPos.x = (thisCamera.transform.position.x - thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).x);
            tempPos.z = thisCamera.transform.position.z;
            thisCamera.transform.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).x > terrainWidth)
        {
            tempPos.x = thisCamera.transform.position.x - (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).x - terrainWidth);
            tempPos.z = thisCamera.transform.position.z;
            thisCamera.transform.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).z < 0)
        {
            tempPos.x = thisCamera.transform.position.x;
            tempPos.z = thisCamera.transform.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).z);
            thisCamera.transform.position = tempPos;
        }
        if (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).z > terrainHeight)
        {
            tempPos.x = thisCamera.transform.position.x;
            tempPos.z = thisCamera.transform.position.z - (thisCamera.ViewportToWorldPoint(new Vector3(0, 0, 2 * thisCamera.orthographicSize)).z - terrainHeight);
            thisCamera.transform.position = tempPos;
        }

    }

}
