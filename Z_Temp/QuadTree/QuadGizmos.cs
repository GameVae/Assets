using DataTable;
using Generic.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadGizmos : MonoBehaviour
{
    public QuadNode RootNode;
    public JSONTable_RSSPosition RSSPosition;

    private HexMap mapIns;
    private List<QuadNode> allQuadNode;

    public HexMap MapIns
    {
        get
        {
            return mapIns ?? (mapIns = Singleton.Instance<HexMap>());
        }
    }

    public void Awake()
    {
        allQuadNode = new List<QuadNode>();

        RootNode = new QuadNode(0,0,null,
            new Vector3Int(5, 5, 0),
            new Vector3Int(517, 517, 0));

        int count = RSSPosition.Count;
        for (int i = 0; i < count; i++)
        {
            RootNode.Insert(RSSPosition.ReadOnlyRows[i].Position.Parse3Int().ToClientPosition());
        }

        RootNode.AllChildren(allQuadNode);
    }

    public void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            int count = allQuadNode.Count;

            for (int i = 0; i < count; i++)
            {
                DrawQuad(allQuadNode[i]);
            }
        }
    }

    private void DrawQuad(QuadNode node)
    {
        Vector3 botLeft = MapIns.CellToWorld(node.BotLeft);
        Vector3 topRight = MapIns.CellToWorld(node.TopRight);

        Vector3 botRight = new Vector3(topRight.x, topRight.y, botLeft.z);
        Vector3 topLeft = new Vector3(botLeft.x, botLeft.y, topRight.z);

        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }
}
