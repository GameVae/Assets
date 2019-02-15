using Generic.Singleton;
using System.Collections.Generic;


public class RenderFilterManager : ISingleton
{
    private List<CellInfo> visibleObjects;

    public List<CellInfo> VisibleObjects { get { return visibleObjects; } }

    private RenderFilterManager()
    {
        visibleObjects = new List<CellInfo>();
    }

    public void BecomeVisible(CellInfo value)
    {
        if (!visibleObjects.Contains(value))
        {
            visibleObjects.Add(value);
        }
    }

    public void BecomeInvisible(CellInfo value)
    {
        int index = visibleObjects.IndexOf(value);
        if (index >= 0)
        {
            visibleObjects.RemoveAt(index);
        }
    }
}
