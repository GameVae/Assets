using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testEffect : MonoBehaviour {
    private ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem = transform.GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        
    }
}
