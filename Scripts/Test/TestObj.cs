using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObj : MonoBehaviour {
    Ray ray;

    void Update()
    {
        RaycastHit hit;
        if (Input.touchCount>0)
        {
            ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hit))
            {
                string name = hit.transform.gameObject.name;
                if (name == "Cube")
                    Debug.Log("hit Cube");
            }

            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.Log("mouse: " + Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                string name = hit.transform.gameObject.name;
                if (name == "Cube")
                    Debug.Log("hit Cube");
            }
        }
        if (Input.GetButtonDown("Fire1"))
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            Debug.Log("mouse: " + Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                string name = hit.transform.gameObject.name;
                if (name == "Cube")
                    Debug.Log("hit Cube");
            }
        }
    }
}