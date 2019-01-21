using EnumCollect;
using ManualTable;
using ManualTable.Interface;
using ManualTable.Row;
using System;
using System.Linq;
using UI.Widget;
using UnityEngine;
using static UpgResWdoCtrl;

public class StartupWindow : BaseWindow
{
    public GUISliderWithBtn UpgProgBar;
    public GUISliderWithBtn ResProgBar;
    public GUIProgressSlider MainbaseLevelBar;

    public GUIInteractableIcon Mainbase;
    public GUIInteractableIcon Resource;
    public GUIInteractableIcon Defense;
    public GUIInteractableIcon Army;
    public GUIInteractableIcon Trade;


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
        MainbaseLevelBar.Value = SyncData.BaseUpgrade[ListUpgrade.MainBase].Level;
        BaseUpgradeRow resRef = SyncData.CurrentResearch;
        BaseUpgradeRow upgRef = SyncData.CurrentUpgrade;

        bool isUpgrade = upgRef != null ? upgRef.ID.IsDefined() : false;
        bool isResearch = resRef != null ? resRef.ID.IsDefined() : false;

        Debug.Log(isUpgrade + " - " + isResearch);
        ITable table = null;

        if (isUpgrade)
        {
            table = WDOCtrl[upgRef.ID];
            string jsonData = table[upgRef.Level - 1].ToJSON();
            GenericUpgradeInfo upgInfo = Json.JSONBase.FromJSON<GenericUpgradeInfo>(jsonData);
            UpgProgBar.Slider.MaxValue = upgInfo != null ? upgInfo.TimeInt : 0;
        }

        if (isResearch)
        {
            table = WDOCtrl[resRef.ID];
            string jsonData = table[resRef.Level - 1].ToJSON();
            GenericUpgradeInfo resInfo = Json.JSONBase.FromJSON<GenericUpgradeInfo>(jsonData);
            ResProgBar.Slider.MaxValue = resInfo != null ? resInfo.TimeInt : 0;
        }

        UpgProgBar.gameObject.SetActive(isUpgrade);
        ResProgBar.gameObject.SetActive(isResearch);
    }

    protected override void Init()
    {
        Mainbase.OnClickEvents += OnMainbaseBtn;
        Resource.OnClickEvents += delegate { WDOCtrl.Open(UgrResWindow.Resource); };
        Defense.OnClickEvents += delegate { WDOCtrl.Open(UgrResWindow.Defense); };
        Trade.OnClickEvents += delegate { WDOCtrl.Open(UgrResWindow.Trade); };
        Army.OnClickEvents += delegate { WDOCtrl.Open(UgrResWindow.Army); };

    }

    private void OnMainbaseBtn()
    {
        WDOCtrl.Open(UgrResWindow.UpgradeResearch);
        WDOCtrl[UgrResWindow.UpgradeResearch].Load(ListUpgrade.MainBase);
    }

    public override void Open()
    {
        base.Open();
        Load();
    }
}
