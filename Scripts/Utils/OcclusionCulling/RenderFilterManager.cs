using Generic.Singleton;
using System.Collections.Generic;


public class RenderFilterManager : ISingleton
{
    private List<NodeInfo> visibleObjects;

    public List<NodeInfo> VisibleObjects { get { return visibleObjects; } }

    private RenderFilterManager()
    {
        visibleObjects = new List<NodeInfo>();
    }

    public void BecomeVisible(NodeInfo value)
    {
        if (!visibleObjects.Contains(value))
        {
            visibleObjects.Add(value);
        }
    }

    public void BecomeInvisible(NodeInfo value)
    {
        int index = visibleObjects.IndexOf(value);
        if (index >= 0)
        {
            visibleObjects.RemoveAt(index);
        }
    }
}
