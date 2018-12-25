using ManualTable;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeResearchManager : MonoBehaviour, IWindow
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

    private Dictionary<int, GameObject> windows;
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

    }

    private void Init()
    {

        preWindow = new Stack<Window>();
        BackBtn.OnClickEvents += delegate { Back(); };

        windows = new Dictionary<int, GameObject>()
        {
            { Window.Army.GetHashCode(), ArmyWindow.gameObject},
            { Window.UpgradeResearch.GetHashCode(), UpgradeResearchWindow.gameObject},
            { Window.Startup.GetHashCode(),StartupWindow.gameObject },
            { Window.Trade.GetHashCode(),TradeWindow.gameObject },
            { Window.Resource.GetHashCode(),ResourceWindow.gameObject },
            { Window.Defense.GetHashCode(),DefenseWindow.gameObject },
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
            windows[type.GetHashCode()].SetActive(true);
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
            windows[type.GetHashCode()].SetActive(false);
    }

    public void Back()
    {
        Open(preWindow.Pop(), true);
    }

    public void LoadData(params object[] input)
    {
        throw new System.NotImplementedException();
    }
}
