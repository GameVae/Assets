using Generic.Singleton;
using Map;
using static NodeManagerFactory;

public class WayPointManager<T> : ISingleton
    where T : WayPoint
{
    private NodeType type = 0;
    public NodeType Type
    {
        get { return type; }
    }

    private WayPointManager()
    {
        type = 0;
    }

    private IWayPointManager manager;
   
    private IWayPointManager CreateIns()
    {
        switch (type)
        {
            case NodeType.Single:
                return new SingleWayPointManager();
            case NodeType.Range:
                return new RangeWayPointManager();
            default:
                return null;
        }
    }

    public IWayPointManager Manager
    {
        get { return manager ?? (manager = CreateIns()); }
    }

    public void SetType(NodeType nodeType)
    {
        if (type == 0)
            type = nodeType;
    }
}
