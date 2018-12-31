using ManualTable;
using UI.Widget;
using System.Collections.Generic;
using UnityEngine;
using ManualTable.Interface;

public class UpgradeResearchManager : MonoBehaviour
{
    public enum Database
    {
        Mainbase,
        Infantry,
        Ranged,
        Mounted,
        SeigeEngine
    }

    public enum Window
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

    private Stack<Window> preWindow;
    private Window curWindow;
    private bool inited;

    [Header("Construct Database")]
    [SerializeField] private MainBaseTable MainbaseDB;
    [SerializeField] private MainBaseTable InfantryDB;

    public GUIInteractableIcon BackBtn;
    [Header("windows")]
    [SerializeField] private StartupWindow StartupWindow;
    [SerializeField] private ArmyWindow ArmyWindow;
    [SerializeField] private UpgradeResearchWindow UpgradeResearchWindow;
    [SerializeField] private TradeWindow TradeWindow;
    [SerializeField] private ResourceWindow ResourceWindow;
    [SerializeField] private DefenseWindow DefenseWindow;


    private void Awake()
    {
        if (!inited)
        { Init(); }
    }

    private void Init()
    {
        preWindow = new Stack<Window>();
        BackBtn.OnClickEvents += delegate { Back(); };

        windows = new Dictionary<int, IWindow>()
        {
            { Window.Army.GetHashCode()             ,ArmyWindow},
            { Window.UpgradeResearch.GetHashCode()  ,UpgradeResearchWindow},
            { Window.Startup.GetHashCode()          ,StartupWindow},
            { Window.Trade.GetHashCode()            ,TradeWindow},
            { Window.Resource.GetHashCode()         ,ResourceWindow},
            { Window.Defense.GetHashCode()          ,DefenseWindow },
        };

        constructDB = new Dictionary<int, ITable>()
        {
            {Database.Mainbase.GetHashCode()    ,MainbaseDB},
            {Database.Infantry.GetHashCode()    ,InfantryDB },
            {Database.Ranged.GetHashCode()      ,null },
            {Database.Mounted.GetHashCode()     ,null },
            {Database.SeigeEngine.GetHashCode() ,null }
        };
        inited = true;
    }

    public void Open()
    {
        if (!inited)
        { Init(); }

        curWindow = Window.None;
        gameObject.SetActive(true);
        Open(Window.Startup);
    }

    public void Open(Window type, bool back = false)
    {
        if (type == Window.None)
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
        if (curWindow != Window.None)
            Close(curWindow);
        gameObject.SetActive(false);
    }

    public void Close(Window type)
    {
        if (curWindow != Window.None)
            windows[type.GetHashCode()].Close();
    }

    public void Back()
    {
        Open(preWindow.Pop(), true);
    }

    public IWindow this[Window type]
    {
        get
        {
            windows.TryGetValue(type.GetHashCode(), out IWindow window);
            return window;
        }
    }

    public ITable this[Database type]
    {
        get
        {
            constructDB.TryGetValue(type.GetHashCode(), out ITable table);
            return table;
        }
    }
}
