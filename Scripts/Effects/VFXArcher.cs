using Generic.Pooling;
using UnityEngine;

public class VFXArcher : MonoBehaviour, IPoolable
{
    [SerializeField] private ParticleSystem parArrow;
    public Transform target;

    public int ManagedId
    {
        get;
        private set;
    }

    [ContextMenu("Attack target")]
    public void AttackTarget()
    {
        ParticleSystem.VelocityOverLifetimeModule vel = parArrow.velocityOverLifetime;
        Vector3 targetOnPlane = target.position;
        targetOnPlane.y = transform.position.y;

        transform.LookAt(targetOnPlane);

        float dis = Vector3.Distance(targetOnPlane, transform.position);
        vel.yMultiplier = dis;
    }

    public void Stop()
    {
        if (parArrow.isPlaying)
        {
            parArrow.Stop();
            //parArrow.Clear(true);
            Debugger.Log("stop particle sys");
        }
    }

    public void Attack(Transform other)
    {
       
        if (other != null)
        {
            target = other;
            AttackTarget();
            parArrow.Play();
        }
        else
        {
            Debugger.Log("target null or parArrow is playing " + other);
        }
    }

    [ContextMenu("Attack")]
    public void Attack()
    {
        if (target != null && parArrow.isStopped)
        {
            AttackTarget();
            parArrow.Play();
        }
    }

    public void FirstSetup(int insId)
    {
        ManagedId = insId;
    }

    public void Dispose()
    {
        Stop();
        target = null;
    }
}
