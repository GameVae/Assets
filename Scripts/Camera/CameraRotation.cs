using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private Camera cam;
    private float hRotation;
    private float vRotation;

    public float RotationSpeed;
    public Vector2 WrapVertical;
    public Vector2 WrapHorizontal;
    public GameObject VerticalAxis;
    public GameObject HorizontalAxis;


    private void Awake()
    {
        cam = GetComponent<Camera>();
        vRotation = 90.0f;
        hRotation = 0.0f;
    }
    private void Update()
    {
        Rotation();
    }

    private void Rotation()
    {
        float h, v;
        h = Input.GetAxis("Mouse X");
        v = Input.GetAxis("Mouse Y");

        if (Mathf.Abs(v) > Mathf.Abs(h))
        {
            h = 0.0f;
        }
        else
        {
            v = 0.0f;
        }

        Debugger.instance.Log(h);
        vRotation = Mathf.Clamp(vRotation - v * RotationSpeed, WrapVertical.x, WrapVertical.y);
        hRotation = Mathf.Clamp(hRotation + h * RotationSpeed, WrapHorizontal.x, WrapHorizontal.y);

        VerticalAxis.transform.localRotation = Quaternion.Euler(vRotation, 0.0f, 0.0f);
        //HorizontalAxis.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, -hRotation);
        HorizontalAxis.transform.localRotation = Quaternion.Euler(0.0f, hRotation, 0.0f);
    }
}
