using ManualTable.Row;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RSS_info
{
    public int RSS_Type;
    public int Level;
    public string Position;
    public int Quality;
    public int Region_Position;
    public int ID_Player;
    public int ID_Base;
    public int ID_Unit;
    public string TimePrepare;
    public string TimeHarvestFinish;
    public string TimeRemove;
}

public class TestRSS : MonoBehaviour {

    public RSS_PositionRow Data;
    public RSS_PositionJSONTable table;
    public int ID;
    [SerializeField]
    public RSS_info RSS_info;
    private void Awake()
    {
        LoadData();
        
    }

    private void Start()
    {
        // parse position
        Data = ResourceManager.Instance.Datas[ID];
        LoadObject();
    }
    public void LoadData()
    {
        RSS_info.RSS_Type = table[ID].RssType;
    }
    public void LoadObject()
    {
        transform.GetChild(RSS_info.RSS_Type - 1).gameObject.SetActive(true);
    }
}
