using Entities.Navigation;
using Generic.Singleton;
using UI.Animation;
using UI.Widget;
using UnityEngine;
using Generic.Pooling;
using Generic.Observer;
using System.Collections.Generic;

public class SelectAgentPanel : MonoBehaviour, IObserver
{
    private int selectedId;

    public SelectableAgentElement Prefab;
    public RectTransform ScrollViewContent;
    public GUIInteractableIcon OpenButton;
    public GUIInteractableIcon UnSelectAgentButton;

    public CameraButtonGroup CameraGroup;
    public ResizeAnimation ResizeAnimation;

    private MyAgentRemoteManager myAgentManager;
    private Pooling<SelectableAgentElement> selectablePooling;
    private List<SelectableAgentElement> catcher;

    public MyAgentRemoteManager MyAgentManager
    {
        get
        {
            return myAgentManager ?? (myAgentManager = Singleton.Instance<MyAgentRemoteManager>());
        }
    }
    public List<SelectableAgentElement> Catcher
    {
        get
        {
            return catcher ?? (catcher = new List<SelectableAgentElement>());
        }
    }

    public void Awake()
    {
        selectablePooling = new Pooling<SelectableAgentElement>(CreateButton);

        ResizeAnimation.CloseDoneEvt += delegate { ActiveContent(false); };
        OpenButton.OnClickEvents += OnOpenButton;
        UnSelectAgentButton.OnClickEvents += TurnOffUnselectButton;
    }

    private void Start()
    {
        MyAgentManager.Register(this);
    }

    private void ActiveContent(bool value)
    {
        ScrollViewContent.gameObject.SetActive(value);
    }

    private void OnOpenButton()
    {
        ResizeAnimation.Action();
        ActiveContent(true);
    }
 
    private void CreateSelecables()
    {
        Dictionary<int, AgentRemote> agentRemotes = MyAgentManager.MyAgentRemotes;
        foreach (var agent in agentRemotes)
        {
            Add(agent.Value);
        }
        FitSize(selectablePooling.ActiveCount);
    }

    private void OnSelected(AgentRemote remote)
    {
        ResizeAnimation.Close();
        MyAgentManager.ActiveNav(remote.AgentID);

        Vector3Int position = remote.CurrentPosition;
        CameraGroup.CameraMoveToAgent(position);
        TurnOnUnselectButton();

        selectedId = remote.AgentID;
    }

    private void FitSize(int count)
    {
        if (count == 1)
        {
            ResizeAnimation.MaxSize.y = 220;
        }
        else if (count == 2)
        {
            ResizeAnimation.MaxSize.y = 280;
        }
        else
        {
            ResizeAnimation.MaxSize.y = 450;
        }

        if (ResizeAnimation.IsOpen)
        {
            ResizeAnimation.ForceMaxSize();
        }
    }

    private void TurnOnUnselectButton()
    {
        UnSelectAgentButton.gameObject.SetActive(true);
    }

    private void TurnOffUnselectButton()
    {
        selectedId = -1;
        MyAgentManager.UnActiveNav();
        UnSelectAgentButton.gameObject.SetActive(false);
    }

    public void Refresh()
    {
        ReleasePoolObject();
        CreateSelecables();
    }

    public void Add(AgentRemote remote)
    {
        SelectableAgentElement el = selectablePooling.GetItem();
        el.PlaceholderComp.Text = remote.AgentID.ToString();

        el.SelectableComp.OnClickEvents += delegate
        {
            OnSelected(remote);
        };

        el.gameObject.SetActive(true);
        el.RectTransform.SetAsLastSibling();
        Catcher.Add(el);
    }

    #region Observer
    public void SubjectUpdated(object dataPacked)
    {
        if(!MyAgentManager.IsOwnerAgent(selectedId))
        {
            selectedId = -1;
            TurnOffUnselectButton();
        }
        Refresh();
    }

    public void Dispose()
    {
        ReleasePoolObject();
        MyAgentManager.Remove(this);
    }
    #endregion

    #region Pooling
    private void ReleasePoolObject()
    {
        int count = Catcher.Count;
        for (int i = 0; i < count; i++)
        {
            selectablePooling.Release(Catcher[i]);
        }
        Catcher.Clear();
    }

    private SelectableAgentElement CreateButton(int id)
    {
        SelectableAgentElement el = Instantiate(Prefab, ScrollViewContent);
        el.FirstSetup(id);
        return el;
    }
    #endregion
}
