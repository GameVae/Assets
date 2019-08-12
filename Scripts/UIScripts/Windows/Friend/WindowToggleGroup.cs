using UI.Composites;
using UnityEngine;

public class WindowToggleGroup : BaseWindow
{
    public SelectableComp OpenButton;
    public SelectableComp CloseButton;

    [SerializeField] protected ToggleWindow[] windows;
    [SerializeField] protected ToggleGroupComp toggles;
    protected BaseWindow activeWindow;

    private void Awake()
    {
        OpenButton.OnClickEvents += Open;
        CloseButton.OnClickEvents += Close;
    }

    public override void Load(params object[] input)
    {
        toggles.Active(0);
    }

    protected override void Init()
    {
        toggles.ToggleSelectedEvt += OnToggleSelected;
    }

    private void OnToggleSelected(ToggleComp toggle)
    {
        int index = toggles.Toggles.IndexOf(toggle);
        activeWindow?.Close();
        //Debugger.Log("toggle index " + index + " " + windows.Length);
        if (index >= 0 && index < windows.Length)
        {
            activeWindow = windows[index];
            activeWindow.Open();
        }
    }

    public void Active(int index)
    {
        toggles.Active(index);
    }

#if UNITY_EDITOR
    [ContextMenu("Set up group")]
    public void SetGroupForChild()
    {
        for (int i = 0; i < windows.Length; i++)
        {
            windows[i].Manager = this;
        }
    }
#endif
}
