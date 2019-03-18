using System.Collections.Generic;
using UI.Widget;
using UnityEngine;
using static WindowManager;

public class WindowGroup : MonoBehaviour
{
    private Dictionary<int, IWindow> windows;

    private Stack<WindowType> preWindow;
    private WindowType curWindow;
    private WindowManager WDOCtrl;

    public GUIInteractableIcon OpenButton;
    public WindowGroupType GroupType;
    public GameObject Panel;
    public GUIInteractableIcon BackButton;
    public WindowType StatupType;
    public List<WindowType> Group;


    public IWindow this[WindowType type]
    {
        get
        {
            windows.TryGetValue(type.GetHashCode(), out IWindow window);
            return window;
        }
    }

    private void Awake()
    {
        OpenButton.OnClickEvents += Open;  
    }

    private void Start()
    {
        WDOCtrl = WindowManager.Instance;

        WDOCtrl.AddGroup(GroupType, this);
        windows = WDOCtrl.Windows;
        if (Group != null && !Group.Contains(WindowType.None))
            Group.Add(WindowType.None);
        BackButton.OnClickEvents += delegate{ Back(); };
        preWindow = new Stack<WindowType>();
    }

    public void Open()
    {
        curWindow = WindowType.None;
        Open(StatupType);
        Panel.SetActive(true);
    }

    public void Open(WindowType type, bool isBack = false)
    {
        if (type == WindowType.None)
        {
            Close();
            return;
        }
        else
        {
            if (!IsValid(type)) return;

            windows[type.GetHashCode()].Open();
            Close(curWindow);
            if (!isBack)
                preWindow.Push(curWindow);
            curWindow = type;
        }
    }

    public void Close()
    {
        preWindow.Clear();
        Close(curWindow);
        Panel.SetActive(false);
    }

    public void Close(WindowType type)
    {
        if (IsValid(type))
            windows[type.GetHashCode()].Close();
    }

    public void Back()
    {
        Open(preWindow.Pop(), true);
    }

    private bool IsValid(WindowType type)
    {
        return (type != WindowType.None && Group.Contains(type));
    }
}
