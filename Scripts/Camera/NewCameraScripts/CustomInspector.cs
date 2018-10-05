using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AndroidCameraCtrl))]
public class CustomInspector : Editor
{
    private bool showRotation;
    private Vector3 hEulerAngle;
    private Vector3 vEulerAngle;

    private SerializedObject obj;
    private AndroidCameraCtrl cameraCtrl;

    private Camera camera;
    private Transform horizontalAxis;
    private Transform verticalAxis;

    private string showPropertiesValue;
    public void OnEnable()
    {
        cameraCtrl = (AndroidCameraCtrl)target;
        obj = new SerializedObject(cameraCtrl);

        camera = cameraCtrl.GetComponentInChildren<Camera>();

        horizontalAxis = cameraCtrl.HorizontalAxis;
        verticalAxis = cameraCtrl.VerticalAxis;
    }

    private string ShowPropertiesValue()
    {
        showPropertiesValue = "=== CAMERA ===";
        showPropertiesValue += "\nCamera World Quaternion: " + camera.transform.rotation
                            + "\nCamera Local Quaternion: " + camera.transform.localRotation
                            + "\nCamera World Euler       : " + camera.transform.eulerAngles
                            + "\nCamera Local Euler        : " + camera.transform.localEulerAngles
                            + "\n=== HORIZONTAL AXIS ===\n"
                            + "\nH Axis World Quaternion:" + horizontalAxis.rotation
                            + "\nH Axis Local Quaternion:" + horizontalAxis.localRotation
                            + "\nH Axis World Euler     :" + horizontalAxis.eulerAngles
                            + "\nH Axis Local Euler     :" + horizontalAxis.localEulerAngles
                            + "\n=== VERTICAL AXIS ===\n"
                            + "\nV Axis World Quaternion:" + verticalAxis.rotation
                            + "\nV Axis Local Quaternion:" + verticalAxis.localRotation
                            + "\nV Axis World Euler     :" + verticalAxis.eulerAngles
                            + "\nv Axis Local Euler     :" + verticalAxis.localEulerAngles;

        return showPropertiesValue;
    }
    public override void OnInspectorGUI()
    {
        //DrawDefaultInspector();
        // button
        GUILayout.BeginVertical();

        GUILayout.TextArea(ShowPropertiesValue(), GUILayout.ExpandHeight(true));
        if (GUILayout.Button("Get properties"))
        {
            cameraCtrl.GetProperties();
        }
        GUILayout.EndVertical();
        // input fields
        showRotation = EditorGUILayout.Foldout(showRotation,"Rotation");
        if (showRotation)
        {
            // input value
            hEulerAngle = EditorGUILayout.Vector3Field("Horizontal Euler Angle", hEulerAngle);
            vEulerAngle = EditorGUILayout.Vector3Field("Vertical Euler Angle", vEulerAngle);

            // button set properties
            GUILayout.BeginHorizontal("Set properties button",GUILayout.ExpandHeight(false),GUILayout.MaxHeight(50.0f));
            if (GUILayout.Button("Set Horizontal",GUILayout.MaxWidth(100.0f)))
            {
                horizontalAxis.transform.rotation = Quaternion.Euler(hEulerAngle);
               
            }
            if (GUILayout.Button("Set Vertical", GUILayout.MaxWidth(100.0f)))
            {
                verticalAxis.transform.rotation = Quaternion.Euler(vEulerAngle);
            }
            if (GUILayout.Button("Reset Rotation", GUILayout.MaxWidth(100.0f)))
            {
                horizontalAxis.transform.localRotation = Quaternion.Euler(0.0f,0.0f,0.0f);
                verticalAxis.transform.localRotation = Quaternion.Euler(90.0f,0.0f,0.0f);
            }
            GUILayout.EndHorizontal();
        }
    }
}
