using EnumCollect;
using Generic.Singleton;
using Json.Interface;
using ManualTable.Interface;
using ManualTable.Row;
using System.Reflection;
using UI.Widget;
using UnityEngine;
using static WindowManager;

public class StartupWindow : BaseWindow, IWindowGroup
{
    public GUISliderWithBtn UpgProgBar;
    public GUISliderWithBtn ResProgBar;
    public GUIProgressSlider MainbaseLevelBar;

    public GUIInteractableIcon Mainbase;
    public GUIInteractableIcon Resource;
    public GUIInteractableIcon Defense;
    public GUIInteractableIcon Army;
    public GUIInteractableIcon Trade;

    private FieldReflection fieldReflection;

    public WindowGroup Group
    {
        get { return WDOCtrl[GroupType]; }
    }

    public WindowGroupType GroupType
    {
        get { return WindowGroupType.UpgradeResearchGroup; }
    }


    protected override void Start()
    {
        base.Start();
        fieldReflection = Singleton.Instance<FieldReflection>();
    }
    protected override void Update()
    {
        base.Update();
        SetTextProgCoundown();
    }

    private void SetTextProgCoundown()
    {
        if (ResProgBar.gameObject.activeInHierarchy)
        {
            ResProgBar.Slider.Value = ResProgBar.Slider.MaxValue - (float)SyncData.CurrentMainBase.ResearchTime;
            ResProgBar.Placeholder.text = ResearchText(SyncData.CurrentMainBase);
            if (SyncData.CurrentMainBase.IsResearchDone)
            {
                ResProgBar.gameObject.SetActive(false);
            }
        }

        if (UpgProgBar.gameObject.activeInHierarchy)
        {
            UpgProgBar.Slider.Value = UpgProgBar.Slider.MaxValue - (float)SyncData.CurrentMainBase.UpgradeTime;
            UpgProgBar.Placeholder.text = UpgradeText(SyncData.CurrentMainBase);
            if (SyncData.CurrentMainBase.IsUpgradeDone)
            {
                UpgProgBar.gameObject.SetActive(false);
            }
        }
    }

    private string ResearchText(BaseInfoRow baseInfo)
    {
        string type = baseInfo.ResearchWait_ID.ToString().InsertSpace();
        string remainTime = System.TimeSpan.FromSeconds(Mathf.RoundToInt((float)baseInfo.ResearchTime)).ToString().Replace(".", "d ");

        return type + " " + remainTime;
    }

    private string UpgradeText(BaseInfoRow baseInfo)
    {
        string type = baseInfo.UpgradeWait_ID.ToString().InsertSpace();
        string remainTime = System.TimeSpan.FromSeconds(Mathf.RoundToInt((float)baseInfo.UpgradeTime)).ToString().Replace(".", "d ");

        return type + " " + remainTime;
    }

    public override void Load(params object[] input)
    {
        MainbaseLevelBar.Value = SyncData.CurrentBaseUpgrade[ListUpgrade.MainBase].Level;
        BaseUpgradeRow resRef = SyncData.CurrentResearch;
        BaseUpgradeRow upgRef = SyncData.CurrentUpgrade;

        bool isUpgrade = upgRef != null ? upgRef.ID.IsDefined() : false;
        bool isResearch = resRef != null ? resRef.ID.IsDefined() : false;

        //Debug.Log(isUpgrade + " - " + isResearch);
        ITable table = null;

        if (isUpgrade)
        {
            table = WDOCtrl[upgRef.ID];

            IJSON upgInfo = table[upgRef.Level - 1];
            int timeInt = fieldReflection.GetFieldValue<int>(upgInfo, "TimeInt", BindingFlags.Public | BindingFlags.Instance);
            UpgProgBar.Slider.MaxValue = timeInt;
        }

        if (isResearch)
        {
            table = WDOCtrl[resRef.ID];

            IJSON resInfo = table[upgRef.Level - 1];
            int timeInt = fieldReflection.GetFieldValue<int>(resInfo, "TimeInt", BindingFlags.Public | BindingFlags.Instance);
            ResProgBar.Slider.MaxValue = timeInt;
        }

        UpgProgBar.gameObject.SetActive(isUpgrade);
        ResProgBar.gameObject.SetActive(isResearch);
    }

    protected override void Init()
    {
        Mainbase.OnClickEvents += OnMainbaseBtn;
        Resource.OnClickEvents += delegate { Group.Open(WindowType.Resource); };
        Defense.OnClickEvents += delegate { Group.Open(WindowType.Defense); };
        Trade.OnClickEvents += delegate { Group.Open(WindowType.Trade); };
        Army.OnClickEvents += delegate { Group.Open(WindowType.Army); };

    }

    private void OnMainbaseBtn()
    {
        Group.Open(WindowType.UpgradeResearch);
        Group[WindowType.UpgradeResearch].Load(ListUpgrade.MainBase);
    }

    public override void Open()
    {
        base.Open();
        Load();
    }
}
