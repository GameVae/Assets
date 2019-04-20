using Map;
using Generic.Singleton;
using System.Collections.Generic;

public class NodeManagerProvider : MonoSingle<NodeManagerProvider>
{
    public enum NodeType
    {
        Single = 1,
        Range
    }

    private Dictionary<int, IWayPointManager> catcher;
    private Dictionary<int,IWayPointManager> Catcher
    {
        get
        {
            return catcher ?? (catcher = new Dictionary<int, IWayPointManager>());
        }
    }

    public IWayPointManager GetManager<T>(NodeType nodeType)
        where T : WayPoint
    {
        int hashCode = typeof(WayPointManager<T>).GetHashCode();

        IWayPointManager result = GetFromCatcher<T>(hashCode);
        if (result == null)
        {
            result = CreateNew<T>(nodeType);
            if(result != null)
            {
                Catcher.Add(hashCode, result);
            }
        }
        return result;
    }

    private IWayPointManager GetFromCatcher<T>(int hashCode)
    {
        Catcher.TryGetValue(hashCode, out IWayPointManager value);
        return value;
    }

    private IWayPointManager CreateNew<T>(NodeType nodeType)
        where T : WayPoint
    {
        WayPointManager<T> node = Singleton.Instance<WayPointManager<T>>();

        switch (nodeType)
        {
            case NodeType.Single:
                {
                    node.SetType(NodeType.Single);
                    return node.Manager;
                }
            case NodeType.Range:
                {
                    node.SetType(NodeType.Range);
                    return node.Manager;
                }
        }
        return null;
    }
}
