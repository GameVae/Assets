using Map;
using static NodeManagerProvider;
public class ConstructWayPoint : RangeWayPoint
{
    private RangeWayPointManager manager;
    protected override INodeManager Manager
    {
        get
        {
            return manager ?? (manager = 
                NodeManagerProvider.GetManager<ConstructWayPoint>(NodeType.Range)
                as RangeWayPointManager);
        }
    }
}
