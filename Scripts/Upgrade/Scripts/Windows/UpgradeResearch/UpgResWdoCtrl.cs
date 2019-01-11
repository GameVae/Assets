using ManualTable;
using UI.Widget;
using System.Collections.Generic;
using UnityEngine;
using ManualTable.Interface;
using EnumCollect;
using Network.Sync;

[SerializeField]
public sealed class UpgResWdoCtrl : MonoBehaviour
{
    public enum UgrResWindow
    {
        None,
        Startup,
        Army,
        Resource,
        UpgradeResearch,
        Defense,
        Trade
    }

    private Dictionary<int, IWindow> windows;
    private Dictionary<int, ITable> constructDB;

    private Stack<UgrResWindow> preWindow;
    private UgrResWindow curWindow;

    public GameObject WDOPanel;
    public Connection Conn;

    [Header("Construct Database")]
    [SerializeField] private MainBaseTable MainbaseDB;
    [SerializeField] private MainBaseTable InfantryDB;

    public GUIInteractableIcon BackBtn;
    [Header("Windows")]
    [SerializeField] private StartupWindow StartupWindow;
    [SerializeField] private ArmyWindow ArmyWindow;
    [SerializeField] private UpgResWindow UpgradeResearchWindow;
    [SerializeField] private TradeWindow TradeWindow;
    [SerializeField] private ResourceWindow ResourceWindow;
    [SerializeField] private DefenseWindow DefenseWindow;

    public Sync Sync { get { return Conn.Sync; } }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        Conn = FindObjectOfType<Connection>();
        preWindow = new Stack<UgrResWindow>();
        BackBtn.OnClickEvents += delegate { Back(); };

        windows = new Dictionary<int, IWindow>()
        {
            { UgrResWindow.Army.GetHashCode()             ,ArmyWindow},
            { UgrResWindow.UpgradeResearch.GetHashCode()  ,UpgradeResearchWindow},
            { UgrResWindow.Startup.GetHashCode()          ,StartupWindow},
            { UgrResWindow.Trade.GetHashCode()            ,TradeWindow},
            { UgrResWindow.Resource.GetHashCode()         ,ResourceWindow},
            { UgrResWindow.Defense.GetHashCode()          ,DefenseWindow },
        };

        constructDB = new Dictionary<int, ITable>()
        {
            {ListUpgrade.MainBase.GetHashCode()    ,MainbaseDB},
            {ListUpgrade.Infantry.GetHashCode()    ,InfantryDB },
            {ListUpgrade.Ranged.GetHashCode()      ,null },
            {ListUpgrade.Mounted.GetHashCode()     ,null },
            {ListUpgrade.SiegeEngine.GetHashCode() ,null }
        };
    }

    public void Open()
    {
        curWindow = UgrResWindow.None;
        WDOPanel.SetActive(true);
        Open(UgrResWindow.Startup);
    }

    public void Open(UgrResWindow type, bool back = false)
    {
        if (type == UgrResWindow.None)
        {
            Close();
            return;
        }
        else
        {
            windows[type.GetHashCode()].Open();
            Close(curWindow);
            if (!back)
                preWindow.Push(curWindow);
            curWindow = type;
        }
    }

    public void Close()
    {
        preWindow.Clear();
        if (curWindow != UgrResWindow.None)
            Close(curWindow);
        WDOPanel.SetActive(false);
    }

    public void Close(UgrResWindow type)
    {
        if (curWindow != UgrResWindow.None)
            windows[type.GetHashCode()].Close();
    }

    public void Back()
    {
        Open(preWindow.Pop(), true);
    }

    public IWindow this[UgrResWindow type]
    {
        get
        {
            windows.TryGetValue(type.GetHashCode(), out IWindow window);
            return window;
        }
    }

    public ITable this[ListUpgrade type]
    {
        get
        {
            constructDB.TryGetValue(type.GetHashCode(), out ITable table);
            return table;
        }
    }
}
