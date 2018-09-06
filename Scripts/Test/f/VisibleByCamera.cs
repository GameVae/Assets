using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibleByCamera : MonoBehaviour {

    private void OnBecameVisible()
    {
        enabled = true;
        transform.gameObject.AddComponent<MeshCollider>();
    }
    private void OnBecameInvisible()
    {
        enabled = false;
        gameObject.SetActive(false);
       // Destroy(transform.gameObject.GetComponent<MeshCollider>());
    }
    private void Update()
    {
        OnBecameVisible();
        OnBecameInvisible();
    }
}
