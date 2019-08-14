using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimingOffset : MonoBehaviour
{
    public Material Bird;
    public Vector2 Offset;
    public float Speed;
    public void Awake()
    {
        Bird.mainTextureOffset = Offset;
    }

    public void Update()
    {
        Offset.x = Mathf.Repeat(Offset.x + Time.deltaTime * Speed, 1.0f);
        Bird.mainTextureOffset = Offset;
    }
}
