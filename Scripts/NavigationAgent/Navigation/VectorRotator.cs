using UnityEngine;

public sealed class VectorRotator : MonoBehaviour
{
    public Vector3 Target;
    public float Angular = 5;
    public bool IsBlock;

    private void FixedUpdate()
    {
        if(!IsBlock)
        {
            Vector3 direction = (Target - transform.position) * Angular;
            transform.forward += direction * Time.deltaTime;
        }
    }
}
