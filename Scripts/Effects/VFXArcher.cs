using UnityEngine;

public class VFXArcher : MonoBehaviour
{
    [SerializeField] private ParticleSystem parArrow;
    private Transform target;

    [ContextMenu("Attack target")]
    public void AttackTarget()
    {
        ParticleSystem.VelocityOverLifetimeModule vel = parArrow.velocityOverLifetime;
        Vector3 targetOnPlane = target.position;
        targetOnPlane.y = transform.position.y;

        //transform.LookAt(targetOnPlane);

        float dis = Vector3.Distance(targetOnPlane, transform.position);
        vel.yMultiplier = dis * 0.5f;
    }

    public void Stop()
    {
        if (parArrow.isPlaying)
        {
            parArrow.Stop();
        }
    }

    public void Attack(Transform target)
    {
        Debugger.Log("attack " + target.position);
        if (parArrow.isStopped)
        {
            this.target = target;
            AttackTarget();
            parArrow.Play();
            Debugger.Log("play particle");
        }
    }
}
