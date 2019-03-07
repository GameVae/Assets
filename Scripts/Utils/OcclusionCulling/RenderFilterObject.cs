using Generic.Singleton;
using UnityEngine;

public class RenderFilterObject : MonoBehaviour
{
    private NavAgent navAgent;

    private void Awake()
    {
        navAgent = GetComponent<NavAgent>();    
    }

    private void OnBecameVisible()
    {
        Singleton.Instance<RenderFilterManager>().BecomeVisible(navAgent.WayPoint.NodeInfo);
    }

    private void OnBecameInvisible()
    {
        Singleton.Instance<RenderFilterManager>().BecomeInvisible(navAgent.WayPoint.NodeInfo);
    }
}
