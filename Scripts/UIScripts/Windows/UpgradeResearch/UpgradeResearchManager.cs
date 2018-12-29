using ManualTable;
using UI.Widget;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeResearchManager : MonoBehaviour
{

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
    private Stack<Window> preWindow;
    private Window curWindow;
    private bool inited;

    public MainBaseTable MainbaseData;

    public GUIInteractableIcon BackBtn;

    public StartupWindow StartupWindow;
    public ArmyWindow ArmyWindow;
    public UpgradeResearchWindow UpgradeResearchWindow;
    public TradeWindow TradeWindow;
    public ResourceWindow ResourceWindow;
    public DefenseWindow DefenseWindow;


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
            { Window.Army.GetHashCode(), ArmyWindow},
            { Window.UpgradeResearch.GetHashCode(), UpgradeResearchWindow},
            { Window.Startup.GetHashCode(),StartupWindow},
            { Window.Trade.GetHashCode(),TradeWindow},
            { Window.Resource.GetHashCode(),ResourceWindow},
            { Window.Defense.GetHashCode(),DefenseWindow },
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
}
