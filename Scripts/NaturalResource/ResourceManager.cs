using Generic.Singleton;
using ManualTable;
using System.Collections.Generic;
using UnityEngine;

public sealed class ResourceManager : MonoSingle<ResourceManager>
{
    private GameObject[] resourceTypes;
    private GameObject[] flags;

    private Dictionary<int, NaturalResource> Resources;

    public RSS_PositionJSONTable Datas;
    public Transform Prefab;

    public NaturalResource this[int id]
    {
        get
        {
            NaturalResource value;
            return Resources.TryGetValue(id, out value) ? value : null;
        }
        set
        {
            Resources[id] = value;
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

        Resources = new Dictionary<int, NaturalResource>();
    }

    private void Start()
    {
        float start = Time.realtimeSinceStartup;
        int count = Datas.Rows.Count;
        for (int i = 0; i < count; i++)
        {
            GenResource((RssType)Datas.Rows[i].RssType, Flag.Owner, i + 1);
        }

        Debug.Log("instaniate done: " + (Time.realtimeSinceStartup - start));
    }


    public NaturalResource GenResource(RssType rssType, Flag group, int id)
    {
        NaturalResource newGO = new GameObject("Resource" + rssType.ToString() + id).AddComponent<NaturalResource>();

        newGO.Id = id;
        Instantiate(resourceTypes[((int)rssType - 1)], newGO.transform);    // resource
        Instantiate(flags[(int)group], newGO.transform);                         // flag

        newGO.transform.SetParent(transform);

        BoxCollider colli = newGO.gameObject.AddComponent<BoxCollider>();
        colli.center = new Vector3(0, 1, 0);
        colli.size = new Vector3(2, 2, 2);
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
            newGO.transform.SetParent(transform);
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
