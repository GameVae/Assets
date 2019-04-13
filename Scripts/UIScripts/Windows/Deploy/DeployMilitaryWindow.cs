using DB;
using EnumCollect;
using Generic.Singleton;
using DataTable;
using DataTable.Row;
using Network.Data;
using SocketIO;
using System.Collections.Generic;
using System.Linq;
using UI.Widget;
using UnityEngine;

public class DeployMilitaryWindow : BaseWindow
{
    public DeployMilitaryTag TagPrefab;
    public GUIScrollView ScrollView;
    public GUIInteractableIcon DeployButton;
    public GUIInteractableIcon OpenButton;
    public SelectAgentPanel SelectAgentPanel;

    private DBReference dbRef;
    private DeployMilitaryTag refTag;
    private List<DeployMilitaryTag> tags;
    private UnitDataReference unitDataReference;
    private EventListenersController events;

    protected override void Awake()
    {
        base.Awake();
        OpenButton.OnClickEvents += Open;
        events = Singleton.Instance<EventListenersController>();
    }

    protected override void Start()
    {
        base.Start();
        events.AddEmiter("S_DEPLOY", S_DEPLOY);
        events.On("R_DEPLOY", R_DEPLOY);

        DeployButton.OnClickEvents += OnDeployButton;


        dbRef = Singleton.Instance<DBReference>();
        unitDataReference = Singleton.Instance<UnitDataReference>();
    }

    private void R_DEPLOY(SocketIOEvent obj)
    {
        Debugger.Log(obj);
        string json = obj.data["R_DEPLOY"].ToString();
        UnitRow unit = JsonUtility.FromJson<UnitRow>(json);
        UserInfoRow user = SyncData.UserInfos.GetUser(unit.ID_User);

        unitDataReference.Create(unit, user);

        JSONTable_Unit units = SyncData.UnitTable;
        units.Rows.Add(unit);
        SelectAgentPanel.Add(unit);
        Debugger.Log("added " + unit.ID);
    }

    private void OnDeployButton()
    {
        if (refTag != null && refTag.Slider.Value > 0)
        {
            Singleton.Instance<EventListenersController>().Emit("S_DEPLOY");
            DecreaseQuality();

            Close();
        }
    }

    private JSONObject S_DEPLOY()
    {
        UserInfoRow user = SyncData.MainUser;
        BaseInfoRow baseInfo = SyncData.CurrentMainBase;

        Dictionary<string, string> data = new Dictionary<string, string>()
        {
            { "Server_ID"   ,user.Server_ID },
            { "ID_User"     ,user.ID_User.ToString()},
            { "ID_Unit"     ,((int)refTag.Type).ToString() },
            { "Quality"     ,((int)refTag.Slider.Value).ToString() },
            { "BaseNumber"  ,baseInfo.BaseNumber.ToString()}
        };
        JSONObject packet = new JSONObject(data);
        Debugger.Log(packet);
        return packet;
    }

    public void TagSelected(DeployMilitaryTag deployMilitaryTag)
    {
        if (refTag != deployMilitaryTag)
        {
            if (refTag != null && refTag.Slider.Value > 0)
            {
                deployMilitaryTag.Refresh();
            }
            else
            {
                refTag = deployMilitaryTag;
            }
        }
    }

    public override void Load(params object[] input)
    {
        RefreshTags();
        JSONTable_BaseDefend baseDefendData = SyncData.CurrentBaseDefend;
        for (int i = 0; i < baseDefendData.Count; i++)
        {
            BaseDefendRow row = baseDefendData.Rows[i];
            CreateType(row.ID_Unit, row.Quality);
        }
    }

    public override void Open()
    {
        base.Open();
        Load();
        refTag?.Refresh();
    }

    protected override void Init()
    {
        tags = new List<DeployMilitaryTag>();
    }

    private void CreateType(ListUpgrade type, int maxQuality)
    {
        DeployMilitaryTag tag = GetTag();
        tag.Icon.Placeholder.text = type.ToString();
        tag.MaxQuality = maxQuality;
        tag.Type = type;
        tag.gameObject.SetActive(true);
    }

    private DeployMilitaryTag GetTag()
    {
        for (int i = 0; i < tags.Count; i++)
        {
            if (!tags[i].gameObject.activeInHierarchy)
                return tags[i];
        }
        DeployMilitaryTag tag = Instantiate(TagPrefab, ScrollView.Content);
        tags.Add(tag);
        return tag;
    }

    private void RefreshTags()
    {
        for (int i = 0; i < tags.Count; i++)
        {
            tags[i].gameObject.SetActive(false);
        }
    }

    private void DecreaseQuality()
    {
        JSONTable_BaseDefend baseDefend = SyncData.CurrentBaseDefend;

        ListUpgrade unitType = refTag.Type;
        int quality = (int)refTag.Slider.Value;

        BaseDefendRow baseDefendRow = baseDefend.Rows.FirstOrDefault(r => r.ID_Unit == unitType);
        baseDefendRow.Quality -= quality;
    }
}
