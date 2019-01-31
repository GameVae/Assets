using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRenderFitSize : MonoBehaviour
{
    public RenderTexture Texture;
    private void Awake()
    {
        Texture.width = Screen.width;
        Texture.height = Screen.height;
    }
}
