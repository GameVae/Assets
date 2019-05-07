using UnityEngine;


public enum ProjectionDir
{
    Dedault,
    Forward,
    Right,
    Up,
}

[RequireComponent(typeof(MeshRenderer))]
public class LookAt : MonoBehaviour
{
    static int Count = 0;
    public Transform Target;
    public Transform GameObject;
    public ProjectionDir ProjectionDir;

    private bool active = false;

    private void LateUpdate()
    {
        if (active)
        {
            if (Target != null || GameObject != null)
            {
                Calulate();
            }
        }

    }

    private void OnBecameInvisible()
    {
        //Debugger.Log("Visiable: " + --Count);
        active = false;
    }

    private void OnBecameVisible()
    {
        //Debugger.Log("Visiable: " + ++Count);
        active = true;
    }

    private void Calulate()
    {
        switch (ProjectionDir)
        {
            case ProjectionDir.Dedault:
                GameObject.LookAt(Target);
                break;
            case ProjectionDir.Right:
                GameObject.LookAt(Vector3.ProjectOnPlane(GameObject.position, Target.right));
                break;
            case ProjectionDir.Forward:
                GameObject.LookAt(Vector3.ProjectOnPlane(GameObject.position, Target.forward));
                break;
            case ProjectionDir.Up:
                GameObject.LookAt(Vector3.ProjectOnPlane(GameObject.position, Target.up));
                break;
        }
    }
}
