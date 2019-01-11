using EnumCollect;
using ManualTable;
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

        UpgProgBar.gameObject.SetActive(SyncData.CurrentMainBase.UpgradeTime > 0.0f);
        ResProgBar.gameObject.SetActive(SyncData.CurrentMainBase.ResearchTime > 0.0f);

        MainBaseTable mainbaseDB = Controller[ListUpgrade.MainBase] as MainBaseTable; // dependence by CurrentMainBase Upg - Res

        MainBaseRow resRef = mainbaseDB.Rows.FirstOrDefault(x => x.Level == SyncData.CurrentResearch?.Level);
        MainBaseRow upgRef = mainbaseDB.Rows.FirstOrDefault(x => x.Level == SyncData.CurrentUpgrade?.Level);

        UpgProgBar.Slider.MaxValue = upgRef != null ? upgRef.TimeInt : 0;
        ResProgBar.Slider.MaxValue = resRef != null ? resRef.TimeInt : 0;
    }

    protected override void Init()
    {
        Mainbase.OnClickEvents  += OnMainbaseBtn;
        Resource.OnClickEvents  += delegate { Controller.Open(UgrResWindow.Resource); };
        Defense.OnClickEvents   += delegate { Controller.Open(UgrResWindow.Defense); };
        Trade.OnClickEvents     += delegate { Controller.Open(UgrResWindow.Trade); };
        Army.OnClickEvents      += delegate { Controller.Open(UgrResWindow.Army); };

    }

    private void OnMainbaseBtn()
    {
        int mainLevel = SyncData.BaseUpgrade[ListUpgrade.MainBase].Level;

        MainBaseTable mainbaseDB = Controller[ListUpgrade.MainBase] as MainBaseTable;
        MainBaseRow row = mainbaseDB.Rows.FirstOrDefault(x => x.Level == mainLevel);

        int[] need = (row == null) ? new int[4] :
            new int[] { row.FoodCost, row.WoodCost, row.StoneCost, row.MetalCost };

        Controller.Open(UgrResWindow.UpgradeResearch);
        Controller[UgrResWindow.UpgradeResearch].Load
            (ListUpgrade.MainBase,
            need,
            row?.MightBonus,
            row?.TimeMin,
            row?.TimeInt
            );
    }

    public override void Open()
    {
        base.Open();
        Load();
    }
}
