using MultiThread;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class QuadNode
{
    private const int MAX_SIZE = 10;
    private const int MAX_DEPTH = 10;

    public readonly int Depth;
    public readonly int Id;
    public readonly QuadNode Parent;

    private Vector3Int center;
    private Vector3Int botLeft;
    private Vector3Int topRight;
    private bool isAsynCreateComplete;

    public Vector3Int Center
    {
        get { return center; }
    }
    public Vector3Int BotLeft
    {
        get
        {
            return botLeft;
        }
    }
    public Vector3Int TopRight
    {
        get
        {
            return topRight;
        }
    }
    public bool IsAsyncCreateComplete
    {
        get { return isAsynCreateComplete; }
        private set { isAsynCreateComplete = value; }
    }
    public ReadOnlyCollection<Vector3Int> Points
    {
        get
        {
            return points?.AsReadOnly();
        }
    }

    private QuadNode[] children;
    private List<Vector3Int> points;

    public QuadNode(
        int depth, int id,
        QuadNode parent,
        Vector3Int argBotLeft,
        Vector3Int argTopRight)
    {
        IsAsyncCreateComplete = true;

        Depth = depth;
        Id = 4 * id;
        Parent = parent;

        //Debug.Log("node id: " + Id);

        botLeft = argBotLeft;
        topRight = argTopRight;

        int centerX = Mathf.Abs(argBotLeft.x + argTopRight.x) / 2;
        int centerY = Mathf.Abs(argBotLeft.y + argTopRight.y) / 2;

        center = new Vector3Int(centerX, centerY, 0);
        points = new List<Vector3Int>();
    }

    private void CreateCallback(object obj)
    {
        //Debugger.Log("Async Start create tree");
        IsAsyncCreateComplete = false;
        IEnumerable<Vector3Int> vectors = obj as IEnumerable<Vector3Int>;
        foreach (Vector3Int vector3Int in vectors)
        {
            Insert(vector3Int);
        }
        IsAsyncCreateComplete = true;
        //Debugger.Log("Async create tree complete");

    }

    public void Clear()
    {
        points.Clear();

        if (children != null)
        {
            for (int i = 0; i < children.Length; i++)
            {
                if (children[i] != null)
                {
                    children[i].Clear();
                    children[i] = null;
                }
            }
        }
    }

    public void Split()
    {
        children = new QuadNode[4];

        children[0] = new QuadNode(Depth + 1, Id + 1, this, botLeft, Center); // bot-left quad

        children[1] = new QuadNode(Depth + 1, Id + 2, this,
            new Vector3Int(Center.x, botLeft.y, 0),
            new Vector3Int(topRight.x, Center.y, 0)); // bot-right quad

        children[2] = new QuadNode(Depth + 1, Id + 3, this, Center, topRight); // top-right quad

        children[3] = new QuadNode(Depth + 1, Id + 4, this,
            new Vector3Int(botLeft.x, Center.y, 0),
            new Vector3Int(Center.x, topRight.y, 0)); // top-left quad
    }

    public bool HasChild()
    {
        return children != null && children[0] != null;
    }

    public void Insert(Vector3Int point)
    {
        if (HasChild())
        {
            int index = GetIndex(point);
            if (index >= 0)
            {
                children[index].Insert(point);
                return;
            }
        }

        points.Add(point);
        int count = points.Count;
        if (count > MAX_SIZE && Depth < MAX_DEPTH)
        {
            Split();
            int i = count - 1;
            while (i >= 0)
            {
                Vector3Int temp = points[i];
                points.RemoveAt(i);
                int childIndex = GetIndex(temp);
                if (childIndex >= 0)
                {
                    children[childIndex].Insert(temp);
                }
                i--;
            }
        }
    }

    public void Merge()
    {
        if (Parent != null)
        {
            int siblingsObjectCount = 0;
            for (int i = 0; i < Parent.children.Length; i++)
            {
                siblingsObjectCount += Parent.children[i].Points.Count;
            }
            if (siblingsObjectCount == 0)
            {
                for (int i = 0; i < Parent.children.Length; i++)
                {
                    Parent.children[i] = null;
                }
                Parent.Merge();
            }
        }
    }

    public void Remove(Vector3Int point)
    {
        if (HasChild())
        {
            int index = GetIndex(point);
            if (index >= 0)
            {
                children[index].Remove(point);
            }
        }
        else
        {
            if (Points.Contains(point))
            {
                points.Remove(point);
                Merge();
            }
            else
            {
                Debugger.Log(point + " can't remove");
            }
        }
    }

    public int GetIndex(Vector3Int point)
    {
        for (int i = 0; i < children.Length; i++)
        {
            if (children[i].IsContains(point))
                return i;
        }
        return -1;
    }

    public bool IsContains(Vector3Int point)
    {
        return point.x >= botLeft.x && point.x <= topRight.x &&
            point.y >= botLeft.y && point.y <= topRight.y;
    }

    public QuadNode Retrieve(Vector3Int point)
    {
        if (HasChild())
        {
            int index = GetIndex(point);
            if (index >= 0)
            {
                return children[index].Retrieve(point);
            }
        }
        else if (IsContains(point))
        {
            return this;
        }
        return null;
    }

    public void AllChildren(List<QuadNode> nodes)
    {
        if (HasChild())
        {
            for (int i = 0; i < children.Length; i++)
            {
                children[i].AllChildren(nodes);
            }
        }
        else
        {
            nodes.Add(this);
        }
    }

    public void AsyncCreateTree(IEnumerable<Vector3Int> vectors)
    {
        MultiThreadHelper.ThreadInvoke(CreateCallback, vectors);
    }

    public void Log()
    {
        int count = points.Count;
        if (HasChild())
            for (int i = 0; i < children.Length; i++)
            {
                children[i].Log();
            }
    }
    public void LogPoints()
    {
        for (int i = 0; i < points.Count; i++)
        {
            Debug.Log(points[i]);
        }
    }
}
