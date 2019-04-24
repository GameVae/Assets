using Map;
using static NodeManagerFactory;
public class ConstructWayPoint : RangeWayPoint
{
    private RangeWayPointManager manager;
    protected override IWayPointManager Manager
    {
        get
        {
            return manager ?? (manager = 
                NodeManagerFactory.GetManager<ConstructWayPoint>(NodeType.Range)
                as RangeWayPointManager);
        }
    }
}
