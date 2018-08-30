using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class Rotator : MonoBehaviour
{
    public float Speed;
}
class RotatorSystem : ComponentSystem
{
    struct ComponentsEntiti
    {
        public Rotator Rotator;
        public Transform Transform;
    }
    float deltaTime;
    protected override void OnStartRunning()
    {
        
        base.OnStartRunning();
        deltaTime = Time.deltaTime;
    }

    protected override void OnUpdate()
    {
        
        foreach (var e in GetEntities<ComponentsEntiti>())
        {
            e.Transform.Rotate(0, e.Rotator.Speed * deltaTime, 0f);
          }
        //throw new System.NotImplementedException();
    }
}