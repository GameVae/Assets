using Generic.Singleton;
using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Json;
using System.Collections.ObjectModel;
using DataTable.Row;
using Generic.Pooling;
using System;

public sealed class ResourceManager : MonoSingle<ResourceManager>
{
    private HexMap mapIns;
    private QuadNode rssPositionTree;
    private GameObject resourceContainer;
    private Pooling<NaturalResource> rssPooling;
    //private GameObject[] resourceTypes;
    //private GameObject[] flags;

    private bool isCreateComplete;
    private bool isWaiting;
    private bool treeInited;

    private int[] currentOverlapNodeId;
    private List<QuadNode> releaseNodes;
    private List<Vector3Int> waitForCreate;
    private Dictionary<int, QuadNode> createdNodes;
    private Dictionary<int, NaturalResource> resources;

    private List<QuadNode> ReleaseNodes
    {
        get
        {
            return releaseNodes ?? (releaseNodes = new List<QuadNode>());
        }
    }
    private List<Vector3Int> WaitForCreate
    {
        get
        {
            return waitForCreate ?? (waitForCreate = new List<Vector3Int>());
        }
    }
    private Pooling<NaturalResource> RssPooling
    {
        get
        {
            return rssPooling ?? (rssPooling = new Pooling<NaturalResource>(CreateRSS));
        }
    }
    private Dictionary<int, QuadNode> CreatedNodes
    {
        get
        {
            return createdNodes ?? (createdNodes = new Dictionary<int, QuadNode>());
        }
    }
    private Dictionary<int, NaturalResource> Resources
    {
        get
        {
            return resources ?? (resources = new Dictionary<int, NaturalResource>());
        }
    }

    public NaturalResource RssPrefab;
    public CameraController CameraController;
    public JSONTable_RSSPosition RSSPositionTable;

    public HexMap MapIns
    {
        get { return mapIns ?? (mapIns = Singleton.Instance<HexMap>()); }
    }
    public QuadNode RSSPositionTree
    {
        get
        {
            return rssPositionTree ??
                (rssPositionTree = new QuadNode(0, 0,
                new Vector3Int(5, 5, 0),
                new Vector3Int(517, 517, 0)));
        }
    }
    public NaturalResource this[int id]
    {
        get
        {
            Resources.TryGetValue(id, out NaturalResource value);
            return value;
        }
        set
        {
            Resources[id] = value;
        }
    }
    public CameraBlindInsideMap CameraBinding
    {
        get
        {
            return CameraController.CameraBinding;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        #region old
        //resourceTypes = new GameObject[]
        //{
        //    Prefab.GetChild(0).gameObject,
        //    Prefab.GetChild(1).gameObject,
        //    Prefab.GetChild(2).gameObject,
        //    Prefab.GetChild(3).gameObject,
        //};
        //flags = new GameObject[]
        //{
        //    Prefab.GetChild(4).gameObject,
        //    Prefab.GetChild(5).gameObject,
        //    Prefab.GetChild(6).gameObject,
        //};
        #endregion

        isCreateComplete = true;
        isWaiting = false;

        resourceContainer = new GameObject("RESOURCE_CONTAINER");
        currentOverlapNodeId = new int[4] { -1, -1, -1, -1 };
    }

    private void Start()
    {
        // StartCoroutine(StartCreateRss());       
        CameraController.CameraChanged += CameraChanged;
    }

    #region Old
    //private IEnumerator StartCreateRss()
    //{
    //    AJPHelper.Operation oper = RSSPositionTable.Operation;
    //    while (!oper.IsDone)
    //        yield return null;

    //    int count = RSSPositionTable.ReadOnlyRows.Count;
    //    ReadOnlyCollection<RSS_PositionRow> rows = RSSPositionTable.ReadOnlyRows;

    //    int i = 0;

    //    while (i < count)
    //    {
    //        int id = i + 1;
    //        NaturalResource rs = GenResource((RssType)rows[i].RssType, Flag.Owner, id);
    //        rs.Initalize(id, this);
    //        Resources[id] = rs;

    //        i++;
    //        yield return null;
    //    }
    //    yield break;
    //}

    //private NaturalResource GenResource(RssType rssType, Flag group, int id)
    //{
    //    NaturalResource newGO = new GameObject("Resource" + rssType.ToString() + id).AddComponent<NaturalResource>();
    //    newGO.gameObject.layer = LayerMask.NameToLayer("RSS");

    //    //newGO.Id = id;
    //    Instantiate(resourceTypes[((int)rssType - 1)], newGO.transform);    // resource
    //    Instantiate(flags[(int)group], newGO.transform);                         // flag

    //    newGO.transform.SetParent(resourceContainer.transform);

    //    //BoxCollider colli = newGO.gameObject.AddComponent<BoxCollider>();
    //    //colli.center = new Vector3(0, 1, 0);
    //    //colli.size = new Vector3(2, 2, 2);
    //    //colli.isTrigger = true;

    //    return newGO;
    //}
    #endregion

    public bool IsRssAtPosition(Vector3Int position, out int id)
    {
        id = -1;
        foreach (KeyValuePair<int, NaturalResource> item in Resources)
        {
            Vector3Int rsPos = item.Value.Data.Position.Parse3Int().ToClientPosition();
            if (rsPos == position)
            {
                id = item.Key;
                return true;
            }
        }
        return false;
    }

    public bool ReturnRSS(NaturalResource rss)
    {
        int id = rss.Id;
        RssPooling.Release(rss);

        if (Resources.ContainsKey(id))
        {
            return Resources.Remove(id);
        }
        return false;
    }

    public bool ReturnRSS(int id)
    {
        if (Resources.ContainsKey(id))
        {
            RssPooling.Release(Resources[id]);
            //Debugger.Log("Released " + id);
            return Resources.Remove(id);
        }
        else
        {
            //Debugger.Log("release failure: " + id);
            return false;
        }
    }

    private void CameraChanged()
    {
        if (!isWaiting)
        {
            List<Vector3Int> conners = MapIns.WorldToCell(CameraBinding.Conners);
            bool isCreateNew = false;
            for (int i = 0; i < conners.Count; i++)
            {
                QuadNode overlapNode = RSSPositionTree.Retrieve(conners[i]);

                currentOverlapNodeId[i] = overlapNode != null ? overlapNode.Id : -1;
                if (overlapNode != null)
                {
                    isCreateNew = AddPositionForCreate(overlapNode) || isCreateNew;
                }
            }

            ReleaseRssOutside();
            if (isCreateNew)
            {
                StartCoroutine(WaitForCreateComplete());
            }
        }
    }

    private void ReleaseRssOutside()
    {
        ReleaseNodes.Clear();
        //Debugger.Log("Current overlap node: ");
        currentOverlapNodeId.Log(" ");
        foreach (int key in CreatedNodes.Keys)
        {
            if (!currentOverlapNodeId.IsContaint(key))
            {
                ReleaseNodes.Add(CreatedNodes[key]);
                //Debugger.Log("add to ReleaseNodes: " + key);
            }
        }

        for (int i = 0; i < ReleaseNodes.Count; i++)
        {
            CreatedNodes.Remove(ReleaseNodes[i].Id);
            ReleaseQuadNode(ReleaseNodes[i]);
        }
    }

    private void ReleaseQuadNode(QuadNode quadNode)
    {
        ReadOnlyCollection<Vector3Int> rssClientPosition = quadNode.Points;
        //Debugger.Log("release count " + rssClientPosition.Count);
        for (int i = 0; i < rssClientPosition.Count; i++)
        {
            try
            {
                //rssClientPosition.Log(" - ");
                RSS_PositionRow rss = RSSPositionTable.GetRssAt(rssClientPosition[i].ToSerPosition());
                int id = rss != null ? rss.ID : -1;
                ReturnRSS(id);
            }
            catch (Exception e)
            {
                Debugger.Log(e.ToString());
            }
        }
    }

    private bool AddPositionForCreate(QuadNode node)
    {
        if (!CreatedNodes.ContainsKey(node.Id))
        {
            ReadOnlyCollection<Vector3Int> positions = node.Points;
            for (int i = 0; i < positions.Count; i++)
            {
                WaitForCreate.Add(positions[i]);
            }
            CreatedNodes.Add(node.Id, node);
            return true;
        }
        return false;
    }

    private IEnumerator WaitForCreateComplete()
    {
        isWaiting = true;
        while (!isCreateComplete)
        {
            yield return null;
        }
        isWaiting = false;
        StartCoroutine(StartCreateRssWaiting());
        yield break;
    }

    private IEnumerator StartCreateRssWaiting()
    {
        AJPHelper.Operation oper = RSSPositionTable.Operation;
        while (!oper.IsDone)
            yield return null;

        if (!treeInited)
        {
            int rssCount = RSSPositionTable.Count;
            for (int iter = 0; iter < rssCount; iter++)
            {
                rssPositionTree.Insert
                    (RSSPositionTable.ReadOnlyRows[iter].Position.Parse3Int().ToClientPosition());
            }
            treeInited = true;
        }

        isCreateComplete = false;
        int count = WaitForCreate.Count;
        int i = count - 1;

        while (i >= 0 && !isCreateComplete)
        {
            RSS_PositionRow rssData = RSSPositionTable.GetRssAt(WaitForCreate[i].ToSerPosition());
            if (rssData != null &&
                !Resources.ContainsKey(rssData.ID))
            {

                /// Using pool object for natural resource
                //NaturalResource rs = GenResource((RssType)rssData.RssType, Flag.Owner, rssData.ID);
                NaturalResource rs = RssPooling.GetItem();
                rs.gameObject.name = "Resource" + rssData.RssType.ToString() + rssData.ID;

                Vector3 worldPos = MapIns.CellToWorld(rssData.Position.Parse3Int().ToClientPosition());
                rs.SetResourceData(rssData, Flag.Owner, worldPos);
                Resources[rssData.ID] = rs;
                rs.gameObject.SetActive(true);

                //Debugger.Log("Created " + rssData.ID);
            }

            WaitForCreate.RemoveAt(i);
            i--;
            yield return null;
        }
        isCreateComplete = true;
        yield break;
    }

    private NaturalResource CreateRSS(int insId)
    {
        NaturalResource rss = Instantiate(RssPrefab, resourceContainer.transform);
        rss.FirstSetup(insId);
        return rss;
    }
}
