using Generic.Singleton;
using ManualTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResourceManager : MonoSingle<ResourceManager>
{
    private HexMap mapIns;
    private GameObject ResourceContainer;
    private GameObject[] resourceTypes;
    private GameObject[] flags;


    private Dictionary<int, NaturalResource> Resources;

    public RSS_PositionJSONTable Datas;
    public Transform Prefab;

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
    public HexMap MapIns
    {
        get { return mapIns ?? (mapIns = Singleton.Instance<HexMap>()); }
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

        Resources = new Dictionary<int, NaturalResource>();
        ResourceContainer = new GameObject("RESOURCE_CONTAINER");
    }

    private void Start()
    {
        StartCoroutine(AsyncCreateRss());
    }

    private IEnumerator AsyncCreateRss()
    {
        int count = Datas.Rows.Count;
        int i = 0;

        while (i < count)
        {
            int id = i + 1;
            NaturalResource rs = GenResource((RssType)Datas.Rows[i].RssType, Flag.Owner, id);
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

        BoxCollider colli = newGO.gameObject.AddComponent<BoxCollider>();
        colli.center = new Vector3(0, 1, 0);
        colli.size = new Vector3(2, 2, 2);
        colli.isTrigger = true;

        return newGO;
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
