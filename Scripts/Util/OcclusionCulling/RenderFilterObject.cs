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
        RenderFilterManager.Instance.BecomeVisible(navAgent.Info);
    }

    private void OnBecameInvisible()
    {
        RenderFilterManager.Instance.BecomeInvisible(navAgent.Info);
    }
}
