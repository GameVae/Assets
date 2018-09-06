using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour {

    private void FixedUpdate()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moveCam = new Vector3(Time.deltaTime* h, Time.deltaTime * v, 0);
        transform.Translate(moveCam);
    }
}
