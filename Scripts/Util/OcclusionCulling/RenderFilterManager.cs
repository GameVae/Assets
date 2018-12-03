using System.Collections.Generic;
using UnityEngine;

public class RenderFilterManager
{
    private static RenderFilterManager instance = new RenderFilterManager();
    private List<CellInfomation> visibleObjects;

    public static RenderFilterManager Instance { get { return instance; } }
    public List<CellInfomation> VisibleObjects { get { return visibleObjects; } }

    private RenderFilterManager()
    {
        visibleObjects = new List<CellInfomation>();
    }

    public void BecomeVisible(CellInfomation value)
    {
        if (!visibleObjects.Contains(value))
        {
            visibleObjects.Add(value);
        }
    }

    public void BecomeInvisible(CellInfomation value)
    {
        int index = visibleObjects.IndexOf(value);
        if (index >= 0)
        {
            visibleObjects.RemoveAt(index);

        }
    }
}
