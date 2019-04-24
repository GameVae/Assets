using Entities.Navigation;
using Generic.Singleton;
using DataTable;
using DataTable.Row;
using Network.Data;
using SocketIO;
using UI.Animation;
using UI.Widget;
using UnityEngine;
using Generic.Pooling;

public class SelectAgentPanel : MonoBehaviour
{
    public JSONTable_Unit Units;
    public SelectableAgentElement Prefab;
    public RectTransform ScrollViewContent;
    public GUIInteractableIcon OpenButton;
    public GUIInteractableIcon UnSelectAgentButton;

    public CameraButtonGroup CameraGroup;
    public ResizeAnimation ResizeAnimation;
    public OwnerNavAgentManager OwnerNavController;

    private bool isInited;
    private bool isCanInit;

    private Pooling<SelectableAgentElement> elementPooling;
    private EventListenersController Events;

    public void Awake()
    {
        isInited = false;
        isCanInit = false;

        elementPooling = new Pooling<SelectableAgentElement>(CreateElement);

        Events = Singleton.Instance<EventListenersController>();

        ResizeAnimation.CloseDoneEvt += delegate { ActiveContent(false); };
        OpenButton.OnClickEvents += OnOpenButton;
        UnSelectAgentButton.OnClickEvents += OnUnSelectAgent;

        Events.On("R_UNIT", UnitAlreadyForInit);
    }

    private void UnitAlreadyForInit(SocketIOEvent obj)
    {
        isCanInit = true;
    }

    private void ActiveContent(bool value)
    {
        ScrollViewContent.gameObject.SetActive(value);
    }

    private void OnOpenButton()
    {
        if (isCanInit && !isInited)
        {
            Init();
            isInited = true;
        }
        if (isInited)
        {
            ResizeAnimation.Action();
            ActiveContent(true);
        }
    }

    public void OnUnSelectAgent()
    {
        OwnerNavController.UnSelectCurrentAgent();
        UnSelectAgentButton.gameObject.SetActive(false);
    }

    private void Init()
    {
        for (int i = 0; i < Units.Count; i++)
        {
            Add(Units.Rows[i]);
        }
        FitSize(elementPooling.ActiveCount);
    }

    public void OnSelectAgent()
    {
        ResizeAnimation.Close();
    }

    public void Add(UnitRow agentInfo)
    {
        int id = agentInfo.ID;
        if (OwnerNavController.IsOwnerAgent(id))
        {
            SelectableAgentElement el = elementPooling.GetItem();
            el.PlaceholderComp.Text = id.ToString();

            AgentRemote navRemote = OwnerNavController.GetNavRemote(id);
            navRemote.OnDead += delegate
            {
                elementPooling.Release(el);
                FitSize(elementPooling.ActiveCount);
            };

            el.SelectableComp.OnClickEvents += delegate 
            {
                OwnerNavController.ActiveNav(id);
                ResizeAnimation.Close();
                
                Vector3Int position = navRemote.CurrentPosition;
                CameraGroup.CameraMoveToAgent(position);
                ActiveUnSelectButton();
            };
            el.gameObject.SetActive(true);
        }

        if (isInited) // first init
            FitSize(elementPooling.ActiveCount);
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

        if(ResizeAnimation.IsOpen)
        {
            ResizeAnimation.ForceMaxSize();
        }
    }

    private void ActiveUnSelectButton()
    {
        UnSelectAgentButton.gameObject.SetActive(true);
    }

    private SelectableAgentElement CreateElement(int id)
    {
        SelectableAgentElement el = Instantiate(Prefab, ScrollViewContent);
        el.FirstSetup(id);
        return el;
    }
}
