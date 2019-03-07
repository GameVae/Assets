using EnumCollect;
using Generic.Singleton;
using ManualTable.Row;
using UnityEngine;

[RequireComponent(typeof(NavAgent))]
public sealed class NavRemote : MonoBehaviour
{
    private NavAgentController ctrl;
    private NavAgent thisAgent;

    public ListUpgrade Type;
    private void Awake()
    {
        thisAgent = GetComponent<NavAgent>();
        ctrl = Singleton.Instance<NavAgentController>();
    }

    private void Start()
    {
        ActiveNav();
    }
    public void ActiveNav()
    {
        ctrl.SwitchToAgent(thisAgent);
    }
}
