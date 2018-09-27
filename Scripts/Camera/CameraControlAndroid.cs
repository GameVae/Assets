using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum EnumCameraType
{
    Zoom,
    Rotate,
    Panning
}

public class CameraControlAndroid : MonoBehaviour
{
#if UNITY_ANDROID

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
    private Touch touch;
    
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

                    break;
                case EnumCameraType.Rotate:

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

   
#endif
}
