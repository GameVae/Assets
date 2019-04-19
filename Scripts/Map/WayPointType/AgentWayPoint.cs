using Map;
using static NodeManagerProvider;

public class AgentWayPoint : SingleWayPoint
{
    private SingleWayPointManager manager;
    protected override INodeManager Manager
    {
        get
        {
            return manager ??
                (manager = NodeManagerProvider.GetManager<AgentWayPoint>(NodeType.Single) 
                as SingleWayPointManager);
        }
    }
}
