using ManualTable;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    private Dictionary<int, NaturalResource> Resources;
    public RSS_PositionJSONTable Datas;
    public GameObject Prefab;

    public NaturalResource this[int id]
    {
        get { return Resources.TryGetValue(id, out NaturalResource value) ? value : null; }
        set
        {
            Resources[id] = value;
        }
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        Resources = new Dictionary<int, NaturalResource>();
    }

#if UNITY_EDITOR
    [ContextMenu("Gen Resource")]
    public void GenEmptyResource()
    {
        for (int i = 0; i < 595; i++)
        {
            Instantiate(Prefab, transform).name = i.ToString();
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
        }
    }

#endif
}
