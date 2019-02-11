using UnityEngine;
using UnityEngine.EventSystems;
using UI.Widget;
using Generic.Singleton;

public class AgentController : MonoBehaviour
{
    public static AgentController Instance { get; private set; }

    private EventSystem eventSystem;
    private HexMap HexMap;
    private NavAgent curAgent;

    public GUIOnOffSwitch SwitchButton;
    public Camera CameraRaycaster;
    public NavAgent[] Agents;

    public AStartAlgorithm  AStarCalculator { get; private set; }
    public Vector3Int StartCell { get; private set; }
    public Vector3Int EndCell { get; private set; }
    public bool IsDisable { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(Instance.gameObject);
       
        curAgent = Agents[0];

        SwitchButton.On += On;
        SwitchButton.Off += Off;
    }

    private void Start()
    {
        eventSystem = FindObjectOfType<EventSystem>();
        HexMap = Singleton.Instance<HexMap>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsDisable)
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
                if (!HexMap.IsValidCell(selectCell.x, selectCell.y))
                {
                    return;
                }
                EndCell = selectCell.ZToZero();
                StartCell = HexMap.WorldToCell(curAgent.transform.position).ZToZero();
                FindPath(StartCell,EndCell);
            }
        }
    }

    private void FindPath(Vector3Int start,Vector3Int end)
    {
        curAgent.StartMove(start,end);
    }

    public void SwitchToAgent(int index)
    {
        if (index > Agents.Length)
        {
            Debug.LogError("OUT OF RANGE: switch index failure" + index);
            return;
        }
        curAgent = Agents[index];
    }

    private void On(GUIOnOffSwitch onOff)
    {
        IsDisable = true;
    }

    private void Off(GUIOnOffSwitch onOff)
    {
        IsDisable = false;
    }
}
