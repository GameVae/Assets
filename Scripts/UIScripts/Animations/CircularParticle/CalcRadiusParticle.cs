using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalcRadiusParticle : MonoBehaviour {
    private float width;
    private ParticleSystem.ShapeModule shapeModule;
    private RectTransform rectTransform;

    void Start () {
        shapeModule = gameObject.GetComponentInChildren<ParticleSystem>().shape;
        rectTransform = gameObject.GetComponent<RectTransform>();

        width = rectTransform.rect.width;
        shapeModule.radius = width / 2.225f;
    }
	
}
