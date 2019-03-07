using UnityEngine;


public enum ProjectionDir
{
    Dedault,
    Forward,
    Right,
    Up,
}

public class LookAt : MonoBehaviour
{
    public Transform Target;
    public Transform GameObject;
    public ProjectionDir ProjectionDir;

    private void Update()
    {
        if (Target == null || GameObject == null) return;
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
