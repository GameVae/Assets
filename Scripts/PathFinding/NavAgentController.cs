using UnityEngine;
using UnityEngine.EventSystems;
using UI.Widget;
using Generic.Singleton;
using System.Collections.Generic;

public class NavAgentController : MonoSingle<NavAgentController>
{
    private EventSystem eventSystem;
    private HexMap HexMap;
    private NavAgent curAgent;
    private SIO_MovementListener moveEvent;
    
    private Vector3Int startCell;
    private Vector3Int endCell;
    private bool isDisable;

    private NestedCondition moveConditions;

    public GUIOnOffSwitch SwitchButton;
    public Camera CameraRaycaster;

    public event System.Func<bool> MoveConditions
    {
        add     { moveConditions.Conditions += value; }
        remove  { moveConditions.Conditions -= value; }
    }

    protected override void Awake()
    {
        base.Awake();

        SwitchButton.On += On;
        SwitchButton.Off += Off;
        InitMoveCondition();
    }

    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        HexMap = Singleton.Instance<HexMap>();
        moveEvent = GetComponent<SIO_MovementListener>();
    }

    private void Update()
    {
        if (moveConditions.Evaluate())
        {
            Vector3 mousePos = Input.mousePosition;
            bool raycastHitted = Physics.Raycast(
                CameraRaycaster.ScreenPointToRay(mousePos),
                out RaycastHit hitInfo,
                int.MaxValue);

            if (raycastHitted)
            {
                Vector3Int selectCell = HexMap.WorldToCell(hitInfo.point);
                if (!HexMap.IsValidCell(selectCell.x, selectCell.y) || selectCell == curAgent.CurrentPosition)
                {
                    return;
                }
                endCell = selectCell.ZToZero();
                startCell = HexMap.WorldToCell(curAgent.transform.position).ZToZero();
                AgentStartMove(startCell, endCell);
            }
        }
    }

    private void AgentStartMove(Vector3Int start, Vector3Int end)
    {
        bool foundPath = curAgent.StartMove(start, end);
        if (foundPath)
        {
            //curAgent.GetMovePath().Log();
            //curAgent.GetTime().Log();

            moveEvent.Move(curAgent.GetMovePath(), curAgent.GetTime(), curAgent.CurrentPosition, curAgent.Type);
        }
    }

    public void SwitchToAgent(NavAgent agent)
    {
        curAgent = agent;
    }

    private void On(GUIOnOffSwitch onOff)
    {
        isDisable = true;
    }

    private void Off(GUIOnOffSwitch onOff)
    {
        isDisable = false;
    }

    private void InitMoveCondition()
    {
        moveConditions = new NestedCondition();
        MoveConditions += delegate
        {
            return !isDisable && !eventSystem.IsPointerOverGameObject() && curAgent != null;
        };
#if UNITY_EDITOR
        MoveConditions += delegate
        {
            return Input.GetMouseButtonUp(0);
        };
#endif
    }
}
