﻿using Generic.Singleton;
using UnityEngine;

public class CameraBlindInsideMap : MonoBehaviour
{
    [Header("Target Camera")]
    public Camera TargetCamera;
    [Header("Map Dimension World Unit")]
    public float MapWidth;
    public float MapHeight;

    private float frustumWidth;
    private float frustumHeight;
    private Vector3 center;
    private Vector3[] conners;

    private float minZ;
    private float minX;

    public Vector3[] Conners
    {
        get
        {
            return conners;
        }
    }
    public Vector3 Center
    {
        get
        {
            return center;
        }
    }

    private void ClampCameraPosition()
    {
        Vector3 position = TargetCamera.transform.position;
        position.x = Mathf.Clamp(position.x, -minX + frustumWidth * 0.5f, MapWidth - frustumWidth - minX);
        position.z = Mathf.Clamp(position.z, -minZ + frustumHeight * 0.5f, MapHeight - frustumHeight - minZ);
        TargetCamera.transform.position = position;
    }

    public void CalculateBound()
    {
        if (TargetCamera.CalculateFrustumOnPlane(ref conners, ref center, ref frustumWidth, ref frustumHeight))
        {
            float height = TargetCamera.transform.position.y;
            minZ = height / Mathf.Tan(Mathf.Deg2Rad * (TargetCamera.transform.eulerAngles.x));
            minX = height / Mathf.Tan(Mathf.Deg2Rad * (TargetCamera.transform.eulerAngles.y));
            minX = float.IsInfinity(minX) ? 0 : minX;
            minZ = float.IsInfinity(minZ) ? 0 : minZ;
        }
        ClampCameraPosition();
    }

#if  UNITY_EDITOR
    public void OnDrawGizmos()
    {
        if(conners == null)
        {
            conners = new Vector3[4];
        }
        if (conners != null)
        {
            CalculateBound();
            Gizmos.color = Color.blue;

            for (int i = 0; i < conners.Length; i++)
            {
                Gizmos.DrawSphere(conners[i], 1);
            }
        }
    }

    [ContextMenu("Log Conners")]
    public void LogConners()
    {
        CalculateBound();
        for (int i = 0; i < conners.Length; i++)
        {
            Debug.Log(Singleton.Instance<HexMap>().WorldToCell(conners[i]) +  " - " + conners[i]);
        }
        Debug.Log("center " + center);
    }
#endif
}

