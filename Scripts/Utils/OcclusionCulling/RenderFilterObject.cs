using Entities.Navigation;
using Generic.Singleton;
using UnityEngine;

public class RenderFilterObject : MonoBehaviour
{
    private NavRemote agent;

    private void Awake()
    {
        agent = GetComponent<NavRemote>();    
    }

    private void OnBecameVisible()
    {
        Singleton.Instance<RenderFilterManager>().BecomeVisible(agent.WayPoint.NodeInfo);
    }

    private void OnBecameInvisible()
    {
        Singleton.Instance<RenderFilterManager>().BecomeInvisible(agent.WayPoint.NodeInfo);
    }
}
