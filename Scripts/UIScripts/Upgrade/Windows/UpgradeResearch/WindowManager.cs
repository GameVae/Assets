using ManualTable;
using UI.Widget;
using System.Collections.Generic;
using UnityEngine;
using ManualTable.Interface;
using EnumCollect;
using Network.Sync;
using DB;
using System;
using Generic.Singleton;

[SerializeField]
public sealed class WindowManager : MonoBehaviour
{
    public enum WindowType
    {
        None,
        Startup,
        Army,
        Resource,
        UpgradeResearch,
        Defense,
        Trade,
        Trainning,
    }

    public enum WindowGroupType
    {
        None,
        UpgradeResearchGroup,
    }

    public static WindowManager Instance { get; private set; }

    private Dictionary<int, IWindow> windows;
    private Dictionary<int, WindowGroup> groups;

    private Stack<WindowType> preWindow;
    private WindowType curWindow;

    public GameObject WDOPanel;
    public Connection Conn;

    public void AddGroup(WindowGroupType groupType,WindowGroup windowGroup)
    {
        if (groups == null)
            groups = new Dictionary<int, WindowGroup>();
        groups[groupType.GetHashCode()] = windowGroup;
    }

    public Sync Sync { get { return Conn?.Sync; } }

    public Dictionary<int, IWindow> Windows
    {
        get { return windows; }
        private set { windows = value; }
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else Destroy(Instance);
    
        Init();
    }

    private void Init()
    {
        Conn = FindObjectOfType<Connection>();
        preWindow = new Stack<WindowType>();
    }

    public IWindow this[WindowType type]
    {
        get
        {
            windows.TryGetValue(type.GetHashCode(), out IWindow window);
            return window;
        }
    }

    public WindowGroup this[WindowGroupType type]
    {
        get
        {
            groups.TryGetValue(type.GetHashCode(), out WindowGroup value);
            return value;
        }
    }

    public ITable this[ListUpgrade type]
    {
        get { return Singleton.Instance<DBReference>()[type]; }
    }

    public void AddWindow(WindowType type, IWindow w)
    {
        if(windows == null)
            windows = new Dictionary<int, IWindow>();
        windows[type.GetHashCode()] = w;
    }
}
