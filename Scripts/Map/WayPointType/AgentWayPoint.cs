using Map;
using static NodeManagerFactory;

public class AgentWayPoint : SingleWayPoint
{
    private SingleWayPointManager manager;
    protected override IWayPointManager Manager
    {
        get
        {
            return manager ??
                (manager = NodeManagerFactory.GetManager<AgentWayPoint>(NodeType.Single) 
                as SingleWayPointManager);
        }
    }
}
