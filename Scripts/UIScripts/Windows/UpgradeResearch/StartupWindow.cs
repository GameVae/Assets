using EnumCollect;
using ManualTable;
using ManualTable.Row;
using Network.Sync;
using System;
using System.Linq;
using UI.Widget;
using UnityEngine;
using static UpgradeResearchManager;

public class StartupWindow : MonoBehaviour, IWindow
{
    private bool inited;
    private UpgradeResearchManager manager;

    public GUISliderWithBtn UpgradeProgressBar;
    public GUISliderWithBtn ResearchProgressBar;
    public GUIProgressSlider MainbaseLevelBar;

    public GUIInteractableIcon Mainbase;
    public GUIInteractableIcon Resource;
    public GUIInteractableIcon Defense;
    public GUIInteractableIcon Army;
    public GUIInteractableIcon Trade;

    private void Awake()
    {
    }

    private void Start()
    {
    }

    private void Update()
    {
        ProgressBarCount();
    }

    private void ProgressBarCount()
    {
        if (ResearchProgressBar.gameObject.activeInHierarchy)
        {
            ResearchProgressBar.Placeholder.text = manager.Sync.BaseInfo.ResearchWait_ID.ToString().InsertSpace() + " " +
                TimeSpan.FromSeconds(Mathf.RoundToInt(manager.Sync.BaseInfo.ResearchRemainingInt)).ToString();
            if (manager.Sync.BaseInfo.ResearchRemainingInt <= 0)
                ResearchProgressBar.gameObject.SetActive(false);
        }

        if (UpgradeProgressBar.gameObject.activeInHierarchy)
        {
            UpgradeProgressBar.Placeholder.text = manager.Sync.BaseInfo.UpgradeWait_ID.ToString().InsertSpace() + " " +
                TimeSpan.FromSeconds(Mathf.RoundToInt(manager.Sync.BaseInfo.UpgradeRemainingInt)).ToString();
            if (manager.Sync.BaseInfo.UpgradeRemainingInt <= 0)
                UpgradeProgressBar.gameObject.SetActive(false);
        }
    }

    public void Load(params object[] input)
    {
        MainbaseLevelBar.Value = manager.Sync.Levels.MainbaseLevel;

        UpgradeProgressBar.gameObject.SetActive(manager.Sync.BaseInfo.UpgradeTime != null &&
            manager.Sync.BaseInfo.UpgradeTime != "");
        ResearchProgressBar.gameObject.SetActive(manager.Sync.BaseInfo.ResearchTime != null &&
            manager.Sync.BaseInfo.ResearchTime != "");
    }

    private void Init()
    {
        manager = GetComponentInParent<UpgradeResearchManager>();
        Mainbase.OnClickEvents += OnMainbaseBtn;
        Resource.OnClickEvents += delegate { manager.Open(Window.Resource); };
        Defense.OnClickEvents += delegate { manager.Open(Window.Defense); };
        Army.OnClickEvents += delegate { manager.Open(Window.Army); };
        Trade.OnClickEvents += delegate { manager.Open(Window.Trade); };
    }

    private void OnMainbaseBtn()
    {
        int mainLevel = manager.Sync.Levels.MainbaseLevel;
        MainBaseTable table = manager[ListUpgrade.MainBase] as MainBaseTable;
        MainBaseRow row = table.Rows.FirstOrDefault(x => x.Level == mainLevel);

        int[] need = (row == null) ? new int[4] :
            new int[] { row.FoodCost, row.WoodCost, row.StoneCost, row.MetalCost };

        manager.Open(Window.UpgradeResearch);
        manager[Window.UpgradeResearch].Load
            (ListUpgrade.MainBase,
            need,
            row?.MightBonus,
            row?.TimeMin,
            row?.TimeInt
            );
    }

    public void Open()
    {
        if(!inited)
        {
            Init();
            MainbaseLevelBar.MaxValue = 20;
            inited = true;
        }
        Load();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
