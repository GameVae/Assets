using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private Camera thisCam;
    private void Awake()
    {
        thisCam = GetComponent<Camera>();
    }

    private void Update ()
    {
        if(Input.GetMouseButton(1))
        {
            thisCam.transform.Rotate(0, Input.GetAxis("Mouse X") * 2, 0);
        }
	}
}
