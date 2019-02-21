using UnityEngine;

[System.Serializable]
public sealed class CameraOption
{
    public float Height;
    [Header("Swipe")]
    public float SwipeMinSpeed;
    public float SwipeMaxSpeed;

    [Header("Zoom")]
    public float MaxZoomValue;
    public float ZoomSmoothValue;

    [Header("Fov")]
    public float Default;
    public Vector2 FovClampValue;
}
