using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class CameraControl : MonoBehaviour
{

    private Camera thisCamera;
    [SerializeField]
    private int terrainWidth;
    [SerializeField]
    private int terrainHeight;


    private Vector3 dragOrigin;
    private Vector3 pos, move, rotate, rotateR, posRightMouse, tempPos;
    [SerializeField]
    private Vector3 resetCamera = new Vector3(90, 270, 0);

    [Range(0.1f, 2f)]
    public float DragSpeed = 1f;
    [Range(2f, 20f)]
    public float RotateSpeed = 5f;

    [Space]
    public Button BtnResetCamera;

    [Header("Terrain Data")]
    public Terrain TerrainObj;
    [Space]
    public Vector3 topLeft, topRight, bottomRight, bottomLeft;
    [Space]
    public CursorPos CursorPos;

    void Awake()
    {
#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_EDITOR
        thisCamera = GetComponent<Camera>();
        BtnResetCamera.onClick.AddListener(() => { resetCameraRotate(); });
#endif
    }

    private void Update()
    {
        moveCameraEditor();
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("transform.localEulerAngles: " + transform.localEulerAngles);
        }

    }

    private void moveCameraEditor()
    {
#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
        }
        else if (Input.GetMouseButton(1) && !Input.GetMouseButton(0))
        {
            if (transform.rotation.eulerAngles.x > 80 && transform.rotation.eulerAngles.x < 100)
            {
                posRightMouse = thisCamera.ScreenToViewportPoint(Input.mousePosition);
                rotateR = new Vector3(0, 0, -posRightMouse.x * RotateSpeed);
                transform.Rotate(rotateR);
            }
        }

        if (!Input.GetMouseButton(0)) return;

        pos = thisCamera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);

        if (Input.GetMouseButton(0) && Input.GetMouseButton(1))
        {
            rotate = new Vector3(-pos.y * DragSpeed, pos.x * DragSpeed, 0);
            transform.Rotate(rotate);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
        else
        {
            if (!CameraStatus.instance.InvertBool)
                move = new Vector3(-pos.x * DragSpeed, -pos.y * DragSpeed, 0);
            else
                move = new Vector3(pos.x * DragSpeed, pos.y * DragSpeed, 0);

            transform.Translate(move, Space.Self);
            transform.position = new Vector3(transform.position.x, 20, transform.position.z);

        }
#endif
        checkLeftTop();
        checkRightTop();
        checkRightBot();
        checkLeftBot();
    }

    private void resetCameraRotate()
    {
#if UNITY_STANDALONE || UNITY_STANDALONE_WIN || UNITY_EDITOR
        transform.rotation = Quaternion.Euler(resetCamera);
        transform.position = new Vector3(CursorPos.transform.position.x, 20, CursorPos.transform.position.z);
#endif
    }

    private void checkLeftTop()
    {
        Debug.Log("Check top left");
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
        Debug.Log("Check top right");

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
        Debug.Log("Check bot left");

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
        Debug.Log("Check bot right");

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

    void OnDrawGizmos()
    {
        thisCamera = GetComponent<Camera>();
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

}
