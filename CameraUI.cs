using UnityEngine;
using UnityEngine.UI;

public sealed class CameraUI : MonoBehaviour
{
    public Camera TargetCamera;
    public RawImage RawImage;
    private void Awake()
    {
        RenderTexture texture = new RenderTexture(Screen.width, Screen.height, 1)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Bilinear
        };
        TargetCamera.targetTexture = texture;
        RawImage.texture = texture;
    }
}
