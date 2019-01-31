using EnumCollect;
using ManualTable.Interface;
using ManualTable.Row;
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

    public WindowGroup Group
    {
        get { return WDOCtrl[GroupType]; }
    }

    public WindowGroupType GroupType
    {
        get { return WindowGroupType.UpgradeResearchGroup; }
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
            ResProgBar.Slider.Value = ResProgBar.Slider.MaxValue - SyncData.CurrentMainBase.ResearchTime;
            ResProgBar.Placeholder.text = SyncData.CurrentMainBase.GetResTimeString();
            if (SyncData.CurrentMainBase.ResIsDone())
            {
                ResProgBar.gameObject.SetActive(false);
            }
        }

        if (UpgProgBar.gameObject.activeInHierarchy)
        {
            UpgProgBar.Slider.Value = UpgProgBar.Slider.MaxValue - SyncData.CurrentMainBase.UpgradeTime;
            UpgProgBar.Placeholder.text = SyncData.CurrentMainBase.GetUpgTimeString();
            if (SyncData.CurrentMainBase.UpgIsDone())
            {
                UpgProgBar.gameObject.SetActive(false);
            }
        }
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
            string jsonData = table[upgRef.Level - 1].ToJSON();
            GenericUpgradeInfo upgInfo = JsonUtility.FromJson<GenericUpgradeInfo>(jsonData);
            UpgProgBar.Slider.MaxValue = upgInfo != null ? upgInfo.TimeInt : 0;
        }

        if (isResearch)
        {
            table = WDOCtrl[resRef.ID];
            string jsonData = table[resRef.Level - 1].ToJSON();
            GenericUpgradeInfo resInfo = JsonUtility.FromJson<GenericUpgradeInfo>(jsonData);
            ResProgBar.Slider.MaxValue = resInfo != null ? resInfo.TimeInt : 0;
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
