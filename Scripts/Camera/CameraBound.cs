using UnityEngine;

public class CameraBound
{
    private float width;
    private float height;
    private Vector3 topLeft;
    private Vector3 topRight;
    private Vector3 bottomLeft;
    private Vector3 bottomRight;
    private Vector3 center;
    private Camera cam;
    public Camera Cam
    {
        get { return cam; }
        set
        {
            cam = value;
            ReCalulateBound();
        }
    }
    public float Width
    {
        get { return width; }
    }
    public float Height
    {
        get { return height; }
    }
    public Vector3 TopLeft
    {
        get { return topLeft; }
    }
    public Vector3 TopRight
    {
        get { return topRight; }
    }
    public Vector3 BottomRight
    {
        get { return bottomRight; }
    }
    public Vector3 BottomLeft
    {
        get { return bottomLeft; }
    }
    public Vector3 Center
    {
        get
        {
            return center;
        }
    }

    public bool ReCalulateBound()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(
            origin: cam.transform.position,
            direction: cam.transform.forward,
            layerMask: LayerMask.GetMask("Map"),
            maxDistance: cam.farClipPlane,
            hitInfo: out hitInfo))
        {
            if (!cam.orthographic)
            {
                float distance = Vector3.Distance(cam.transform.position, hitInfo.point);
                // calculate camera bound
                height = 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
                width = height * cam.aspect;
            }
            else
            {
                height = cam.orthographicSize * 2.0f;
                width = height * cam.aspect;
            }

            // 90 degree
            // 4 conners
            topLeft = hitInfo.point;
            topLeft.x -= Width * 0.5f;
            topLeft.z += height * 0.5f;
            // ------------------------
            topRight = hitInfo.point;
            topRight.x += width * 0.5f;
            topRight.z += height * 0.5f;
            // ------------------------
            bottomLeft = hitInfo.point;
            bottomLeft.x -= width * 0.5f;
            bottomLeft.z -= height * 0.5f;
            // ------------------------
            bottomRight = hitInfo.point;
            bottomRight.x += width * 0.5f;
            bottomRight.z -= height * 0.5f;

            // oblique
            center = hitInfo.point;
            Vector3 desireDir = Vector3.zero;
            if (Physics.Raycast(
                origin: cam.transform.position,
                direction: Vector3.down,
                layerMask: LayerMask.GetMask("Map"),
                maxDistance: cam.farClipPlane,
                hitInfo: out hitInfo))
            {
                desireDir = center - hitInfo.point;
            }

            topLeft += desireDir;
            topRight += desireDir;
            bottomLeft += desireDir;
            bottomRight += desireDir;
            return false;
        }
        else
        {
            Debug.LogWarning("distance = 0");
            return true;
        }
    }

    public Vector3 GetDesirePosition(Vector3 currentPos,float desireFov)
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(
            origin: cam.transform.position,
            direction: cam.transform.forward,
            layerMask: LayerMask.GetMask("Map"),
            maxDistance: cam.farClipPlane,
            hitInfo: out hitInfo))
        {
            float distance = Vector3.Distance(cam.transform.position, hitInfo.point);
            float frustumHeight = 2.0f * distance * Mathf.Tan(desireFov * 0.5f * Mathf.Deg2Rad);
            float frustumWidth = frustumHeight * cam.aspect;

            float curH = 2.0f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float curW = curH * cam.aspect;

            float deltaWidth = Mathf.Abs(curW - frustumWidth);
            float deltaHeight = Mathf.Abs(curH - frustumHeight);

            int xDir = (currentPos.x < 0) ? 1 : -1;
            int yDir = (currentPos.z < 0) ? 1 : -1;

            currentPos.x += xDir * deltaWidth;
            currentPos.z += yDir * deltaHeight;
            return currentPos;
        }
        return Vector3.zero;
    }
    public CameraBound() { }
}
