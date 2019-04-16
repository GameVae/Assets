using UnityEngine;
using UI.Widget;
using Generic.Singleton;
using Generic.CustomInput;
using UI;

namespace Entities.Navigation
{
    public sealed class NavAgentController : MonoSingle<NavAgentController>
    {
        private SIO_MovementListener moveEvent;
        private AgentNodeManager agentNodes;

        private Vector3Int startCell;
        private Vector3Int endCell;
        private bool isDisable;

        private NestedCondition moveConditions;

        public Camera CameraRaycaster
        {
            get
            {
                return CursorController.CameraController.TargetCamera;
            }
        }
        public GUIOnOffSwitch SwitchButton;
        public CursorController CursorController;

        public NavAgent CurrentAgent { get; private set; }

        public HexMap MapIns
        {
            get { return CursorController.MapIns; }
        }
        public CrossInput CrossInput
        {
            get { return CursorController.CrossInput; }
        }
        public AgentNodeManager AgentNodes
        {
            get { return agentNodes ?? (agentNodes = Singleton.Instance<GlobalNodeManager>().AgentNode); }
        }
        public UnityEventSystem EventSystem
        {
            get { return CursorController.EventSystem; }
        }
        public SIO_MovementListener MoveEvent
        {
            get { return moveEvent ?? (moveEvent = FindObjectOfType<SIO_MovementListener>()); }
        }

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
            CursorController.SelectedCallback += OnCursorSelected;
        }

        private void Start()
        {
            MoveEvent?.Emit("S_UNIT");
        }

        private void AgentStartMove(Vector3Int start, Vector3Int end)
        {
            //bool foundPath = CurrentAgent.StartMove(start, end);
            CurrentAgent.AsyncStartMove(start, end);
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
                return CurrentAgent != null && !isDisable;
            };
        }

        private bool IsTargetEmpty()
        {
            Vector3Int position = CursorController.SelectedPosition;
            if (AgentNodes.IsHolding(position)) return false;
            return true;
        }

        private void OnCursorSelected(Vector3Int position)
        {
            if (moveConditions.Evaluate())
            {
                Vector3Int selected = position;

                if (!MapIns.IsValidCell(selected.x, selected.y) ||
                    selected == CurrentAgent.CurrentPosition ||
                    (CurrentAgent.IsMoving && selected == CurrentAgent.EndPosition))
                {
                    return;
                }

                endCell = selected;
                startCell = CurrentAgent.CurrentPosition;
                AgentStartMove(startCell, endCell);
            }
            else
            {
                if (CurrentAgent != null && !IsTargetEmpty())
                {
                    CurrentAgent.Remote.Attack(CursorController.SelectedPosition);
                }
            }
        }
    }
}