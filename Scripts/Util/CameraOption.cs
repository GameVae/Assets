using UnityEngine;

[System.Serializable]
public sealed class CameraOption
{
    [Header("Swipe")]
    public float SwipeDecelerate;
    public float SwipeMaxSpeed;
    public float Duration;

    [Header("Zoom")]
    public float ZoomSmoothValue;
    public float ZoomMaxSpeed;
    [Header("Fov")]
    public float Default;
    public Vector2 FovClampValue;
}
