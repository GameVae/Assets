using Entities.Navigation;
using Generic.Singleton;
using ManualTable;
using ManualTable.Row;
using Network.Data;
using SocketIO;
using System.Collections.Generic;
using UI.Animation;
using UI.Widget;
using UnityEngine;

public class SelectAgentPanel : MonoBehaviour
{
    public UnitJSONTable Units;
    public GUIInteractableIcon Prefab;
    public RectTransform ScrollViewContent;
    public GUIInteractableIcon OpenButton;

    public CameraButtonGroup CameraGroup;
    public ResizeAnimation ResizeAnimation;
    public OwnerNavAgentManager OwnerNavController;

    private bool isInited;
    private bool isCanInit;

    private Dictionary<int, GUIInteractableIcon> elements;
    private EventListenersController Events;

    public void Awake()
    {
        isInited = false;
        isCanInit = false;

        elements = new Dictionary<int, GUIInteractableIcon>();
        Events = Singleton.Instance<EventListenersController>();

        ResizeAnimation.CloseDoneEvt += delegate { ActiveContent(false); };
        OpenButton.OnClickEvents += OnOpenButton;

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

        ResizeAnimation.Action();
        ActiveContent(true);
    }

    private void Init()
    {
        for (int i = 0; i < Units.Count; i++)
        {
            Add(Units.Rows[i]);
        }
        FitSize(elements.Count);
    }

    public void OnSelectAgent()
    {
        ResizeAnimation.Close();
    }

    public void Add(UnitRow agentInfo)
    {
        int id = agentInfo.ID;
        if (!elements.ContainsKey(id) && OwnerNavController.IsOwnerAgent(id))
        {
            GUIInteractableIcon el = Instantiate(Prefab, ScrollViewContent);
            el.Placeholder.text = id.ToString();

            el.OnClickEvents += delegate 
            {
                OwnerNavController.ActiveNav(id);
                Vector3Int position = OwnerNavController.GetNavRemote(id).FixedMove.CurrentPosition;

                CameraGroup.CameraMoveToAgent(position);
            };
            el.gameObject.SetActive(true);

            elements[id] = el;
        }

        if (isInited) // first init
            FitSize(elements.Count);
    }

    private void FitSize(int count)
    {
        if (count == 1)
        {
            ResizeAnimation.MaxSize.y = 250;
        }
        else if (count == 2)
        {
            ResizeAnimation.MaxSize.y = 320;
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
}
