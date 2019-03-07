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

    public GUIOnOffSwitch SwitchButton;
    public Camera CameraRaycaster;

    public AStartAlgorithm AStarCalculator { get; private set; }
    public Vector3Int StartCell { get; private set; }
    public Vector3Int EndCell { get; private set; }
    public bool IsDisable { get; private set; }

    private List<System.Func<bool>> canMoveConditions;
    protected override void Awake()
    {
        base.Awake();

        SwitchButton.On += On;
        SwitchButton.Off += Off;
    }

    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        HexMap = Singleton.Instance<HexMap>();
        moveEvent = GetComponent<SIO_MovementListener>();

        AddMoveCondition(delegate
        {
            return Input.GetMouseButtonUp(0) && !IsDisable && !eventSystem.IsPointerOverGameObject() && curAgent != null;
        });
    }

    private void Update()
    {
        if (CheckCantMoveAgent())
        {
            if (eventSystem.IsPointerOverGameObject()) return;

            Vector3 mousePos = Input.mousePosition;
            bool raycastHitted = Physics.Raycast(
                CameraRaycaster.ScreenPointToRay(mousePos),
                out RaycastHit hitInfo,
                int.MaxValue);

            if (raycastHitted)
            {
                Vector3Int selectCell = HexMap.WorldToCell(hitInfo.point);
                if (!HexMap.IsValidCell(selectCell.x, selectCell.y) || selectCell == curAgent.CurrentCell)
                {
                    return;
                }
                EndCell = selectCell.ZToZero();
                StartCell = HexMap.WorldToCell(curAgent.transform.position).ZToZero();
                FindPath(StartCell, EndCell);
            }
        }
    }

    private void FindPath(Vector3Int start, Vector3Int end)
    {
        bool foundPath = curAgent.StartMove(start, end);
        if (foundPath)
        {
            //curAgent.GetMovePath().Log();
            //curAgent.GetTime().Log();

            moveEvent.Move(curAgent.GetMovePath(), curAgent.GetTime(), curAgent.CurrentCell,curAgent.Type);
        }
    }

    public void SwitchToAgent(NavAgent agent)
    {
        curAgent = agent;
    }

    private void On(GUIOnOffSwitch onOff)
    {
        IsDisable = true;
    }

    private void Off(GUIOnOffSwitch onOff)
    {
        IsDisable = false;
    }

    public void MoveAgent(JSONObject data)
    {
        Debugger.Log(data);
        //JSONObject path = data["PATH"];

        //List<Vector3Int> offSetPath = ConvertFromServerData(path);
        //float averageTime = data["AVERAGE_TIME"].n;

        //for (int i = 0; i < offSetPath.Count; i++)
        //{
        //    Debug.Log("Data position: " + offSetPath[i]);
        //}
    }

    private List<Vector3Int> ConvertFromServerData(JSONObject path)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        for (int i = 0; i < path.Count; i++)
        {
            result.Add(path[i].str.Parse3Int());
        }
        return result;
    }

    private bool CheckCantMoveAgent()
    {
        for (int i = 0; i < canMoveConditions?.Count; i++)
        {
            if (!canMoveConditions[i].Invoke())
                return false;
        }
        return true;
    }

    public void AddMoveCondition(System.Func<bool> func)
    {
        if (canMoveConditions == null)
            canMoveConditions = new List<System.Func<bool>>();
        if (!canMoveConditions.Contains(func))
            canMoveConditions.Add(func);
    }
}
