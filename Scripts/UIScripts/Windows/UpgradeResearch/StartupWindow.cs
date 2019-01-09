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
            ResProgBar.Placeholder.text = Controller.Sync.BaseInfo.GetResTimeString();

            if (Controller.Sync.BaseInfo.ResIsDone())
            {
                ResProgBar.gameObject.SetActive(false);
            }
        }

        if (UpgProgBar.gameObject.activeInHierarchy)
        {
            UpgProgBar.Placeholder.text = Controller.Sync.BaseInfo.GetUpgTimeString();
            if (Controller.Sync.BaseInfo.UpgIsDone())
            {
                UpgProgBar.gameObject.SetActive(false);
            }
        }
    }

    public override void Load(params object[] input)
    {
        MainbaseLevelBar.Value = Controller.Sync.Levels.MainbaseLevel;

        UpgProgBar.gameObject.SetActive(Controller.Sync.BaseInfo.UpgradeTime != null &&
            Controller.Sync.BaseInfo.UpgradeTime != "");
        ResProgBar.gameObject.SetActive(Controller.Sync.BaseInfo.ResearchTime != null &&
            Controller.Sync.BaseInfo.ResearchTime != "");
    }

    protected override void Init()
    {
        Mainbase.OnClickEvents += OnMainbaseBtn;
        Resource.OnClickEvents += delegate { Controller.Open(UgrResWindow.Resource); };
        Defense.OnClickEvents += delegate { Controller.Open(UgrResWindow.Defense); };
        Army.OnClickEvents += delegate { Controller.Open(UgrResWindow.Army); };
        Trade.OnClickEvents += delegate { Controller.Open(UgrResWindow.Trade); };
    }

    private void OnMainbaseBtn()
    {
        int mainLevel = Controller.Sync.Levels.MainbaseLevel;
        MainBaseTable table = Controller[ListUpgrade.MainBase] as MainBaseTable;
        MainBaseRow row = table.Rows.FirstOrDefault(x => x.Level == mainLevel);

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
