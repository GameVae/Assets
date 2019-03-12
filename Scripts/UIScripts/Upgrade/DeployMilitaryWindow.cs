using DB;
using EnumCollect;
using Generic.Singleton;
using ManualTable;
using ManualTable.Row;
using Network.Data;
using SocketIO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UI.Widget;

public class DeployMilitaryWindow : BaseWindow
{

    public DeployMilitaryTag TagPrefab;
    public GUIScrollView ScrollView;
    public GUIInteractableIcon DeployButton;
    public GUIOnOffSwitch OpenWDO;

    private FieldReflection fieldReflection;
    private DBReference dbRef;
    private DeployMilitaryTag refTag;
    private List<DeployMilitaryTag> tags;
    private UnitDataReference unitDataReference;

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
        fieldReflection = Singleton.Instance<FieldReflection>();
        unitDataReference = Singleton.Instance<UnitDataReference>();
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
        refTag?.SetValue(0);
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
        UserInfoRow user = SyncData.UserInfo.Rows[0];
        UnitJSONTable units = SyncData.UnitTable;
        BaseInfoRow baseInfo = SyncData.CurrentMainBase;
        BaseUpgradeJSONTable baseUpgrade = SyncData.CurrentBaseUpgrade;
        BaseDefendJSONTable baseDefend = SyncData.CurrentBaseDefend;

        int lv = baseUpgrade[refTag.Type].Level;
        ListUpgrade type = refTag.Type;
        int useId = user.ID_User;
        int baseN = baseInfo.BaseNumber;
        string position = baseInfo.Position;
        int quality = (int)refTag.Slider.Value;

        Json.Interface.IJSON militaryType = dbRef[type][lv - 1];
        float health = fieldReflection.GetFieldValue<float>(militaryType, "Health", BindingFlags.Public | BindingFlags.Instance);
        float cur_hea = health;

        UnitRow newUnit = new UnitRow()
        {
            ID = 0,
            ID_Unit = type,
            ID_User = useId,
            BaseNumber = baseN,
            Level = lv,
            Quality = quality,
            Hea_cur = cur_hea,
            Health = health,
            Position_Cell = position
        };
        units.Rows.Add(newUnit);

        unitDataReference.Create(newUnit, user);

        BaseDefendRow baseDefendRow = baseDefend.Rows.FirstOrDefault(r => r.ID_Unit == type);
        baseDefendRow.Quality -= quality;
    }
}
