using Generic.Singleton;
using Network.Sync;
using UnityEngine;
using static WindowManager;

public abstract class BaseWindow : MonoBehaviour, IWindow
{
    [SerializeField] protected WindowType type;
    [SerializeField] protected GameObject Window;

    protected bool inited = false;
    private WindowManager ctrl;

    protected WindowManager WDOCtrl
    {
        get { return ctrl ?? (ctrl = Singleton.Instance<WindowManager>()); }
    }

    protected Sync SyncData { get { return WDOCtrl?.Sync; } }

    protected virtual void Start()
    {
        if (type != 0)
            WDOCtrl.AddWindow(type, this);
    }

    protected virtual void Update() { }

    protected abstract void Init();

    public abstract void Load(params object[] input);

    public virtual void Close()
    {
        Window.SetActive(false);
    }

    public virtual void Open()
    {
        if (!inited)
        {
            Init();
            inited = true;
        }
        Window.SetActive(true);
    }
}
