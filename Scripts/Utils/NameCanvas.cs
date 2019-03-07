using TMPro;
using UnityEngine;

public class NameCanvas : MonoBehaviour
{
    private TextMeshProUGUI objName;

    public Camera MainCam;
    private void Awake()
    {
        objName = GetComponentInChildren<TextMeshProUGUI>();
        objName.text = objName.transform.root.name;
    }

    private void Update()
    {
        objName.transform.parent.LookAt(Vector3.ProjectOnPlane(transform.position,MainCam.transform.forward));
    }
}
