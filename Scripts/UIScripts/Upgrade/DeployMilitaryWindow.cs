using DB;
using EnumCollect;
using Generic.Singleton;
using ManualTable;
using ManualTable.Row;
using Network.Data;
using SocketIO;
using System.Collections.Generic;
using UI.Widget;

public class DeployMilitaryWindow : BaseWindow
{

    public DeployMilitaryTag TagPrefab;
    public GUIScrollView ScrollView;
    public GUIInteractableIcon DeployButton;
    public GUIOnOffSwitch OpenWDO;

    private DBReference dbRef;
    private DeployMilitaryTag refTag;
    private List<DeployMilitaryTag> tags;

    protected override void Start()
    {
        base.Start();
        Singleton.Instance<EventListenersController>().AddEmiter("S_DEPLOY", S_DEPLOY);
        WDOCtrl.Conn.On("R_DEPLOY", R_DEPLOY);

        DeployButton.OnClickEvents += delegate
        {
            EmitDeployData();
        };

        OpenWDO.OnClick.AddListener(Open);
        dbRef = Singleton.Instance<DBReference>();
    }

    private void R_DEPLOY(SocketIOEvent obj)
    {
        Debugger.Log(obj);
    }
    private void EmitDeployData()
    {
        if (refTag.Slider.Value > 0)
        {
            Singleton.Instance<EventListenersController>().Emit("S_DEPLOY");
            Close();
        }
    }

    private JSONObject S_DEPLOY()
    {
        UserInfoRow user = SyncData.UserInfo.Rows[0];
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
        AddUnit();
        Debugger.Log(packet);
        return packet;
    }

    public void TagSelected(DeployMilitaryTag deployMilitaryTag)
    {
        if (refTag != deployMilitaryTag)
        {
            if (refTag != null && refTag.Slider.Value > 0)
            {
                deployMilitaryTag.SetValue(0);
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
        BaseDefendJSONTable baseDefendData = SyncData.CurrentBaseDefend;
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

    private void AddUnit()
    {
        UnitJSONTable units = SyncData.UnitTable;
        BaseInfoRow baseInfo = SyncData.CurrentMainBase;


    }
}
