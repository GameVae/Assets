using Map;
using Generic.Singleton;
using System.Collections.Generic;

public class NodeManagerFactory : MonoSingle<NodeManagerFactory>
{
    public enum NodeType
    {
        Single = 1,
        Range
    }

    private Dictionary<int, IWayPointManager> catcher;

    private Dictionary<int, IWayPointManager> Catcher
    {
        get
        {
            return catcher ?? (catcher = new Dictionary<int, IWayPointManager>());
        }
    }

    private IWayPointManager GetFromCatcher<T>(int hashCode)
    {
        Catcher.TryGetValue(hashCode, out IWayPointManager value);
        return value;
    }

    private IWayPointManager CreateNew<T>(NodeType nodeType)
        where T : WayPoint
    {
        switch (nodeType)
        {
            case NodeType.Single:
                {
                    WayPointManager<T> node = Singleton.Instance<WayPointManager<T>>();
                    node.SetType(NodeType.Single);
                    return node.Manager;
                }
            case NodeType.Range:
                {
                    WayPointManager<T> node = Singleton.Instance<WayPointManager<T>>();
                    node.SetType(NodeType.Range);
                    return node.Manager;
                }
        }

        return null;
    }

    public IWayPointManager GetManager<T>(NodeType nodeType)
        where T : WayPoint
    {
        int hashCode = typeof(WayPointManager<T>).GetHashCode();

        IWayPointManager result = GetFromCatcher<T>(hashCode);
        if (result == null)
        {
            result = CreateNew<T>(nodeType);
            if (result != null)
            {
                Catcher.Add(hashCode, result);
            }
        }
        return result;
    }

}
