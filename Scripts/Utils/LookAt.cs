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
    public Transform Target;
    public Transform GameObject;
    public ProjectionDir ProjectionDir;

    private bool active;

    private void LateUpdate()
    {
        if (active)
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

    private void OnBecameInvisible()
    {
        active = false;
    }

    private void OnBecameVisible()
    {
        active = true;
    }
}
