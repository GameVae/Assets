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
        TimeSpan time = TimeSpan.ParseExact("1d 12:12:12", "%d hh:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
        Debug.Log(time);
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
            ResearchProgressBar.Placeholder.text = manager.Sync.UpgradeInfo.ResearchType.ToString().InsertSpace() + " " +
                TimeSpan.FromSeconds(Mathf.RoundToInt(manager.Sync.UpgradeInfo.ResearchRemainingInt)).ToString();
            if (manager.Sync.UpgradeInfo.ResearchRemainingInt <= 0)
                ResearchProgressBar.gameObject.SetActive(false);
        }

        if (UpgradeProgressBar.gameObject.activeInHierarchy)
        {
            UpgradeProgressBar.Placeholder.text = manager.Sync.UpgradeInfo.UpgradeType.ToString().InsertSpace() + " " +
                TimeSpan.FromSeconds(Mathf.RoundToInt(manager.Sync.UpgradeInfo.UpgradeRemainingInt)).ToString();
            if (manager.Sync.UpgradeInfo.UpgradeRemainingInt <= 0)
                UpgradeProgressBar.gameObject.SetActive(false);
        }
    }

    public void Load(params object[] input)
    {
        MainbaseLevelBar.Value = manager.Sync.Levels.MainbaseLevel;

        UpgradeProgressBar.gameObject.SetActive(manager.Sync.UpgradeInfo.UpgradeRemainingStr != null &&
            manager.Sync.UpgradeInfo.UpgradeRemainingStr != "" && manager.Sync.UpgradeInfo.UpgradeRemainingInt != 0);
        ResearchProgressBar.gameObject.SetActive(manager.Sync.UpgradeInfo.ResearchRemainingStr != null &&
            manager.Sync.UpgradeInfo.ResearchRemainingStr != "" && manager.Sync.UpgradeInfo.ResearchRemainingInt != 0);
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
