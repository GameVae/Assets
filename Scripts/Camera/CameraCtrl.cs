using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    [Header("Offset")]
    public bool mouseWithMouse = true;

    public float zoomSpeed;
    public float maxZoomIn;
    public float maxZoomOut;
    public float rotateSensitive;
    public float movingSpeed;
    public float borderThinkness;

    public Rect mapRect;
    

    private bool isOrthographic;
    private bool isReCalulateCam;

    private float orthoHeight;
    private float preFov;
    private float desireFov;

    private Vector3 prePosition;
    private Quaternion preRotation;

    private Camera cam;
    private CameraBound camBound;

    private float testT;
    
    private void Awake()
    {
        cam = GetComponent<Camera>();
        isOrthographic = cam.orthographic;
        if (isOrthographic)
        {
            orthoHeight = cam.orthographicSize;
        }

    }
    private void Start()
    {
        camBound = new CameraBound() { Cam = cam };

        prePosition = cam.transform.position;
        preRotation = cam.transform.rotation;
        preFov = cam.orthographic ? cam.orthographicSize : cam.fieldOfView;
        desireFov = preFov;
    }
    private void Update()
    {
#if UNITY_ANDROID
        if (Input.touchCount >= 4)
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Escape))
#endif
        {
            RefreshCam();
        }

        prePosition = cam.transform.position;
        preRotation = cam.transform.rotation;
        preFov = cam.orthographic ? cam.orthographicSize : cam.fieldOfView;

        MoveHandle();
        ZoomHandle();
        RotateHandle();
        if (isReCalulateCam)
        {
            isReCalulateCam = camBound.ReCalulateBound();
            if (CheckOutSide(camBound))
            {
                cam.transform.position = prePosition;
                cam.transform.rotation = preRotation;
                if (!cam.orthographic)
                {
                    cam.fieldOfView = preFov;
                }
                else
                {
                    cam.orthographicSize = preFov;
                }
                if (desireFov != preFov)
                {
                    if (preFov < maxZoomOut)
                    {
                        Vector3 desirePosition = camBound.GetDesirePosition(cam.transform.position, desireFov);
                        cam.transform.position = desirePosition;
                        if (cam.orthographic)
                        { cam.fieldOfView = desireFov; }
                        else
                        { cam.orthographicSize = desireFov; }
                    }
                }
                isReCalulateCam = camBound.ReCalulateBound();
            }
        }
    }
    private void PerspectiveZoom(Vector2 scrollForce)
    {
        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView + (-zoomSpeed * scrollForce.y), maxZoomIn, maxZoomOut);
    }
    private void OrthoGraphicZoom(Vector2 scrollForce)
    {
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + (-zoomSpeed * scrollForce.y), maxZoomIn, maxZoomOut);
    }
    private void ZoomHandle()
    {
        Vector2 scrollForce = Vector2.zero;

#if UNITY_ANDROID
        if (Input.touchCount == 2)
        {
            Touch one = Input.GetTouch(0);
            Touch two = Input.GetTouch(1);

            Vector2 preOnePos = one.position - one.deltaPosition;
            Vector2 preTwoPos = two.position - two.deltaPosition;

            float deltaPreMag = (preOnePos - preTwoPos).magnitude;
            float deltaTouchMag = (one.position - two.position).magnitude;

            float deltaMagDiff = deltaTouchMag - deltaPreMag;
            scrollForce.y = deltaMagDiff;
        }
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
        scrollForce = Input.mouseScrollDelta;
#endif

        if (scrollForce.magnitude > 0)
        {
            if (isOrthographic)
            {
                preFov = cam.orthographicSize;
                OrthoGraphicZoom(scrollForce);
                desireFov = cam.orthographicSize;
            }
            else
            {
                preFov = cam.fieldOfView;
                PerspectiveZoom(scrollForce);
                desireFov = cam.fieldOfView;
            }
            isReCalulateCam = true;
        }
    }
    private void RotateHandle()
    {
        float deltaX = 0.0f;
        float deltaY = 0.0f;
#if UNITY_ANDROID
        if (Input.touchCount == 3)
        {
            Touch one = Input.GetTouch(0);
            deltaX = one.deltaPosition.x;
            deltaY = one.deltaPosition.y;
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
        if (Input.GetMouseButton(1))
        {
            deltaX = Input.GetAxis("Mouse X");
            deltaY = Input.GetAxis("Mouse Y");
#endif
            cam.transform.Rotate(-deltaY * rotateSensitive, deltaX * rotateSensitive, 0.0f);
            isReCalulateCam = true;
        }
    }
    private void MoveHandle()
    {
        Vector3 newPosition = cam.transform.position;
        float deltaTime = Time.deltaTime;
        isReCalulateCam = false;
        Vector2 mousPos = Vector2.zero;
        float ha = 0.0f;
        float va = 0.0f;
#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            mousPos = Input.GetTouch(0).position;
        }
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
        ha = Input.GetAxis("Horizontal");
        va = Input.GetAxis("Vertical");
        mousPos = Input.mousePosition;
#endif
        if (mouseWithMouse)
        {
#if UNITY_ANDROID
            if (Input.touchCount == 1)
            {
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
                {
#endif
                if (mousPos.x >= Screen.width - borderThinkness)
                {
                    newPosition.x += movingSpeed * deltaTime;
                }
                if (mousPos.x <= borderThinkness)
                {
                    newPosition.x -= movingSpeed * deltaTime;
                }
                if (mousPos.y >= Screen.height - borderThinkness)
                {
                    newPosition.z += movingSpeed * deltaTime;
                }
                if (mousPos.y <= borderThinkness)
                {
                    newPosition.z -= movingSpeed * deltaTime;
                }
                if (newPosition != prePosition)
                {
                    cam.transform.position = newPosition;
                    isReCalulateCam = true;
                    return;
                }
            }
        }
#if UNITY_ANDROID
        if (Input.touchCount == 1)
        {
            Vector2 deltaPos = Input.GetTouch(0).deltaPosition;
            ha = (Mathf.Abs(deltaPos.x) > Mathf.Abs(deltaPos.y)) ? - deltaPos.x : 0.0f;
            va = (Mathf.Abs(deltaPos.x) < Mathf.Abs(deltaPos.y)) ? - deltaPos.y : 0.0f;
            if (Mathf.Abs(ha) > 0.0f)
            {
                newPosition.x += ha * deltaTime;
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
            if (Mathf.Abs(ha) > 0.0f)
            {            
                newPosition.x += movingSpeed * ha * deltaTime;
#endif    
                cam.transform.position = newPosition;
                isReCalulateCam = true;
            }
#if UNITY_ANDROID
            else if (Mathf.Abs(va) > 0.0f)
            {
                newPosition.z += va * deltaTime;
#elif UNITY_EDITOR || UNITY_STANDALONE_WIN
            else if (Mathf.Abs(va) > 0.0f)
            {
                newPosition.z += movingSpeed * va * deltaTime;
#endif
                cam.transform.position = newPosition;
                isReCalulateCam = true;
            }
        }
    }
    private void RefreshCam()
    {
        cam.transform.position = new Vector3(0, 40, 0);
        cam.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private bool CheckOutSide(CameraBound bound)
    {
        Vector2 topLeft, topRight, bottomLeft, bottomRight;
        topLeft = new Vector2(bound.TopLeft.x, bound.TopLeft.z);
        topRight = new Vector2(bound.TopRight.x, bound.TopRight.z);
        bottomLeft = new Vector2(bound.BottomLeft.x, bound.BottomLeft.z);
        bottomRight = new Vector2(bound.BottomRight.x, bound.BottomRight.z);

        if (mapRect.Contains(topLeft) &&
            mapRect.Contains(topRight) &&
            mapRect.Contains(bottomLeft) &&
            mapRect.Contains(bottomRight))
        {
            return false;
        }
#if UNITY_EDITOR
        Debug.Log("Out of range");
#endif
        return true;
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (cam != null)
        {
            Vector3 topRight = camBound.TopLeft;
            topRight.x += camBound.Width;
            Vector3 bottomRight = camBound.TopLeft;
            bottomRight.z -= camBound.Height;
            bottomRight.x += camBound.Width;
            Vector3 bottomLeft = camBound.TopLeft;
            bottomLeft.z -= camBound.Height;

            Gizmos.DrawLine(camBound.TopLeft, topRight);
            Gizmos.DrawLine(camBound.TopLeft, bottomLeft);
            Gizmos.DrawLine(bottomLeft, bottomRight);
            Gizmos.DrawLine(bottomRight, topRight);

            Gizmos.DrawRay(camBound.TopLeft, Vector3.right * camBound.Width);
            Gizmos.DrawRay(camBound.TopLeft, Vector3.back * camBound.Height);
            Gizmos.DrawRay(cam.transform.position, Vector3.down * 100.0f);

            Gizmos.DrawRay(cam.transform.position, cam.transform.forward * 100.0f);

            Gizmos.DrawSphere(camBound.TopLeft, 1);
            Gizmos.DrawSphere(topRight, 1);
            Gizmos.DrawSphere(bottomRight, 1);
            Gizmos.DrawSphere(bottomLeft, 1);
            Gizmos.DrawSphere(camBound.Center, 0.5f);

        }
    }
#endif
}
