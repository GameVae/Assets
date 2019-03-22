using UnityEngine;
using UnityEngine.EventSystems;
using UI.Widget;
using Generic.Singleton;
using Generic.CustomInput;

namespace Entities.Navigation
{
    public sealed class NavAgentController : MonoSingle<NavAgentController>
    {
        private EventSystem eventSystem;
        private SIO_MovementListener moveEvent;

        private HexMap mapIns;
        private CrossInput crossInput;

        private Vector3Int startCell;
        private Vector3Int endCell;
        private bool isDisable;

        private NestedCondition moveConditions;

        public GUIOnOffSwitch SwitchButton;
        public Camera CameraRaycaster;

        public NavAgent CurrentAgent { get; private set; }
        public event System.Func<bool> MoveConditions
        {
            add
            {
                if (moveConditions == null)
                    moveConditions = new NestedCondition();

                moveConditions.Conditions += value;
            }
            remove { moveConditions.Conditions -= value; }
        }

        protected override void Awake()
        {
            base.Awake();

            SwitchButton.On += On;
            SwitchButton.Off += Off;
            InitMoveCondition();

            eventSystem = FindObjectOfType<EventSystem>();
            moveEvent = FindObjectOfType<SIO_MovementListener>();
        }

        private void Start()
        {            
            mapIns = Singleton.Instance<HexMap>();
            crossInput = Singleton.Instance<CrossInput>();
            moveEvent.Emit("S_UNIT");
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
                    Vector3Int selectCell = mapIns.WorldToCell(hitInfo.point);
                    if (!mapIns.IsValidCell(selectCell.x, selectCell.y) || 
                        selectCell == CurrentAgent.CurrentPosition || 
                        (CurrentAgent.IsMoving && selectCell == CurrentAgent.EndPosition))
                    {
                        return;
                    }
                    endCell = selectCell.ZToZero();
                    startCell = mapIns.WorldToCell(CurrentAgent.transform.position).ZToZero();
                    AgentStartMove(startCell, endCell);
                }
            }
        }

        private void AgentStartMove(Vector3Int start, Vector3Int end)
        {
            bool foundPath = CurrentAgent.StartMove(start, end);
        }

        public void SwitchToAgent(NavAgent agent)
        {
            CurrentAgent = agent;
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
            MoveConditions += delegate
            {
                return CurrentAgent != null && !eventSystem.IsPointerOverGameObject();
            };
            MoveConditions += delegate
            {
                return crossInput.IsTouch && !isDisable;
            };
        }
    }
}