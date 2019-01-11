using Network.Sync;
using UnityEngine;

public abstract class BaseWindow : MonoBehaviour, IWindow
{
    private bool inited = false;
    private UpgResWdoCtrl ctrl;

    protected UpgResWdoCtrl Controller
    {
        get { return ctrl ?? (ctrl = GetComponentInParent<UpgResWdoCtrl>()); }
    }
    protected Sync SyncData { get { return Controller?.Sync; } }

    protected virtual void Awake()
    {
       
    }

    protected virtual void Start() { }

    protected virtual void Update() { }

    protected abstract void Init();

    public abstract void Load(params object[] input);

    public virtual void Close()
    {
        gameObject.SetActive(false);
    }

    public virtual void Open()
    {
        if(!inited)
        {
            Init();
            inited = true;
        }
        gameObject.SetActive(true);
    }  
}
