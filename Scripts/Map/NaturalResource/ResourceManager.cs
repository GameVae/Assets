using Generic.Singleton;
using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Json;
using System.Collections.ObjectModel;
using DataTable.Row;

public sealed class ResourceManager : MonoSingle<ResourceManager>
{
    private HexMap mapIns;
    private QuadNode rssPositionTree;
    private GameObject ResourceContainer;
    private GameObject[] resourceTypes;
    private GameObject[] flags;

    private bool isCreateComplete;
    private bool isWaiting;
    private bool treeInited;

    private Dictionary<int, NaturalResource> Resources;
    private List<int> createdNodes;
    private List<Vector3Int> waitForCreate;

    public JSONTable_RSSPosition RSSPositionTable;
    public CameraController CameraController;
    public CameraBlindInsideMap CameraBinding
    {
        get
        {
            return CameraController.CameraBinding;
        }
    }
    public Transform Prefab;

    public HexMap MapIns
    {
        get { return mapIns ?? (mapIns = Singleton.Instance<HexMap>()); }
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

    protected override void Awake()
    {
        base.Awake();

        resourceTypes = new GameObject[]
        {
            Prefab.GetChild(0).gameObject,
            Prefab.GetChild(1).gameObject,
            Prefab.GetChild(2).gameObject,
            Prefab.GetChild(3).gameObject,
        };
        flags = new GameObject[]
        {
            Prefab.GetChild(4).gameObject,
            Prefab.GetChild(5).gameObject,
            Prefab.GetChild(6).gameObject,
        };

        createdNodes = new List<int>();
        waitForCreate = new List<Vector3Int>();

        Resources = new Dictionary<int, NaturalResource>();
        ResourceContainer = new GameObject("RESOURCE_CONTAINER");
    }

    private void Start()
    {
        // StartCoroutine(StartCreateRss());
        isCreateComplete = true;
        isWaiting = false;

        CameraController.CameraChanged += CameraChanged;
    }

    private IEnumerator StartCreateRss()
    {
        AJPHelper.Operation oper = RSSPositionTable.Operation;
        while (!oper.IsDone)
            yield return null;

        int count = RSSPositionTable.ReadOnlyRows.Count;
        ReadOnlyCollection<RSS_PositionRow> rows = RSSPositionTable.ReadOnlyRows;

        int i = 0;

        while (i < count)
        {
            int id = i + 1;
            NaturalResource rs = GenResource((RssType)rows[i].RssType, Flag.Owner, id);
            rs.Initalize(id, this);
            Resources[id] = rs;

            i++;
            yield return null;
        }
        yield break;
    }

    private NaturalResource GenResource(RssType rssType, Flag group, int id)
    {
        NaturalResource newGO = new GameObject("Resource" + rssType.ToString() + id).AddComponent<NaturalResource>();
        newGO.gameObject.layer = LayerMask.NameToLayer("RSS");

        newGO.Id = id;
        Instantiate(resourceTypes[((int)rssType - 1)], newGO.transform);    // resource
        Instantiate(flags[(int)group], newGO.transform);                         // flag

        newGO.transform.SetParent(ResourceContainer.transform);

        //BoxCollider colli = newGO.gameObject.AddComponent<BoxCollider>();
        //colli.center = new Vector3(0, 1, 0);
        //colli.size = new Vector3(2, 2, 2);
        //colli.isTrigger = true;

        return newGO;
    }

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

    private void CameraChanged()
    {
        if (!isWaiting)
        {
            List<Vector3Int> conners = MapIns.WorldToCell(CameraBinding.Conners);
            bool isCreateNew = false;
            for (int i = 0; i < conners.Count; i++)
            {
                QuadNode overlapNode = RSSPositionTree.Retrieve(conners[i]);
                if (overlapNode != null)
                {
                    isCreateNew = isCreateNew || AddPositionForCreate(overlapNode);
                }
            }
            if (isCreateNew)
                StartCoroutine(WaitForCreateComplete());
        }
    }

    private bool AddPositionForCreate(QuadNode node)
    {
        if (!createdNodes.Contains(node.Id))
        {
            ReadOnlyCollection<Vector3Int> positions = node.Points;
            for (int i = 0; i < positions.Count; i++)
            {
                waitForCreate.Add(positions[i]);
            }
            createdNodes.Add(node.Id);
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
        int count = waitForCreate.Count;
        int i = count - 1;

        while (i >= 0 && !isCreateComplete)
        {
            RSS_PositionRow rssData = RSSPositionTable.GetRssAt(waitForCreate[i].ToSerPosition());
            if (rssData != null && 
                !Resources.ContainsKey(rssData.ID))
            {
                NaturalResource rs = GenResource((RssType)rssData.RssType, Flag.Owner, rssData.ID);
                rs.Initalize(rssData.ID, this);
                Resources[rssData.ID] = rs;

                // Debugger.Log("Created " + rssData.ID + " - " + rssData.Position);
            }

            waitForCreate.RemoveAt(i);
            i--;
            yield return null;
        }
        isCreateComplete = true;
        yield break;
    }

#if UNITY_EDITOR
    [ContextMenu("Gen Resource")]
    public void GenEmptyResource()
    {
        if (resourceTypes == null || resourceTypes.Length == 0)
        {
            resourceTypes = new GameObject[]
            {
            Prefab.GetChild(0).gameObject,
            Prefab.GetChild(1).gameObject,
            Prefab.GetChild(2).gameObject,
            Prefab.GetChild(3).gameObject,
            };
        }

        for (int i = 0; i < 10; i++)
        {
            NaturalResource newGO = new GameObject(i.ToString()).AddComponent<NaturalResource>();
            Instantiate(resourceTypes[3], newGO.transform);
            newGO.transform.SetParent(ResourceContainer.transform);
        }
    }

    [ContextMenu("Set Id ")]
    public void SetId()
    {
        int id = 0;
        int childCount = transform.childCount;

        Transform child = null;
        while (id < childCount)
        {
            child = transform.GetChild(id);
            child.GetComponent<NaturalResource>().Id = id + 1;
            id++;
            child.name = id.ToString();
        }
    }

#endif
}
