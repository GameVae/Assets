using ManualTable.Row;
using Network.Sync;
using System.Linq;
using UI.Widget;
using UnityEngine;

public class StartupWindow : MonoBehaviour, IWindow
{
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
        Init();
        MainbaseLevelBar.MaxValue = 20;
    }

    private void Start()
    {
        Load();
    }

    public void Load(params object[] input)
    {
        MainbaseLevelBar.Value = Sync.Instance.MainBaseLevel;
    }

    private void Init()
    {
        manager = GetComponentInParent<UpgradeResearchManager>();
        //Mainbase.OnClickEvents += delegate { manager.Open(UpgradeResearchManager.Window.UpgradeResearch); };
        Mainbase.OnClickEvents += OnMainbaseBtn;
        Resource.OnClickEvents += delegate { manager.Open(UpgradeResearchManager.Window.Resource); };
        Defense.OnClickEvents += delegate { manager.Open(UpgradeResearchManager.Window.Defense); };
        Army.OnClickEvents += delegate { manager.Open(UpgradeResearchManager.Window.Army); };
        Trade.OnClickEvents += delegate { manager.Open(UpgradeResearchManager.Window.Trade); };
    }

    private void OnMainbaseBtn()
    {
        int mainLevel = Sync.Instance.MainBaseLevel;
        MainBaseRow row = manager.MainbaseData.rows.FirstOrDefault(x => x.Level == mainLevel);
        int[] need = (row == null) ? new int[4] :
            new int[] { row.FoodCost, row.WoodCost, row.StoneCost, row.MetalCost };

        manager.Open(UpgradeResearchManager.Window.UpgradeResearch);
        manager.UpgradeResearchWindow.Load
            ("Main Base",
            mainLevel,
            new int[] { 0, 0, 0, 0 },
            need,
            row?.MightBonus,
            row?.TimeMin,
            row?.TimeInt
            );
    }

    public void Open()
    {
        Load();
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
