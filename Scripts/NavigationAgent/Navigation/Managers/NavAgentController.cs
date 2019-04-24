using UnityEngine;
using UI.Widget;
using Generic.Singleton;
using Generic.CustomInput;
using UI;
using Generic.Contants;
using MultiThread;

namespace Entities.Navigation
{
    public sealed class NavAgentController : MonoSingle<NavAgentController>
    {
        private SIO_MovementListener moveEvent;
        private MultiThreadHelper threadHelper;
        private SingleWayPointManager agentNodes;

        private Vector3Int startCell;
        private Vector3Int endCell;
        private bool isDisable;

        private NestedCondition canMoveConditions;

        public SelectAgentPanel SelectAgentPanel;
        public Camera CameraRaycaster
        {
            get
            {
                return CursorController.CameraController.TargetCamera;
            }
        }
        public GUIOnOffSwitch SwitchButton;
        public CursorController CursorController;
        public AgentDecisionBehavior agentBehaviorMaking;

        public NavAgent CurrentAgent { get; private set; }

        public HexMap MapIns
        {
            get { return CursorController.MapIns; }
        }
        public CrossInput CrossInput
        {
            get { return CursorController.CrossInput; }
        }
        public SingleWayPointManager AgentNodes
        {
            get { return agentNodes ?? (agentNodes = MapIns.AgentNodeManager); }
        }
        public UnityEventSystem EventSystem
        {
            get { return CursorController.EventSystem; }
        }
        public SIO_MovementListener MovementListener
        {
            get { return moveEvent ?? (moveEvent = FindObjectOfType<SIO_MovementListener>()); }
        }
        public MultiThreadHelper ThreadHelper
        {
            get
            {
                return threadHelper ?? (threadHelper = Singleton.Instance<MultiThreadHelper>());
            }
        }

        private event System.Func<bool> MoveConditions
        {
            add
            {
                if (canMoveConditions == null)
                    canMoveConditions = new NestedCondition();

                canMoveConditions.Conditions += value;
            }
            remove { canMoveConditions.Conditions -= value; }
        }

        protected override void Awake()
        {
            base.Awake();

            SwitchButton.On += On;
            SwitchButton.Off += Off;

            InitNestedConditios();
            CursorController.SelectedCallback += OnCursorSelected;
        }

        private void MoveActiveAgent(Vector3Int start, Vector3Int end, AgentRemote enemy)
        {
            //bool foundPath = CurrentAgent.StartMove(start, end);
            //CurrentAgent.AsyncStartMove(start, end);
            MoveAgent(CurrentAgent, start, end, enemy);
        }

        public void UnSelectCurrentAgent()
        {
            SelectAgentPanel.OnUnSelectAgent();
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

        private void InitNestedConditios()
        {
            // move conditions
            MoveConditions += delegate
            {
                return CurrentAgent != null && !isDisable;
            };
        }

        private bool ShouldHandleSelect(Vector3Int selected)
        {
            return !(!MapIns.IsValidCell(selected.x, selected.y) ||
                    selected == CurrentAgent.CurrentPosition ||
                    (CurrentAgent.IsMoving && selected == CurrentAgent.EndPosition));
        }
        private void OnCursorSelected(Vector3Int position)
        {
            if (canMoveConditions.Evaluate())
            {
                Vector3Int selected = position;

                if (!ShouldHandleSelect(position))
                {
                    return;
                }

                agentBehaviorMaking.MakeDecision();
            }
        }

        //only main thread
        private void EmitMoveEvent(NavAgent agent, bool isFound, AgentRemote enemy)
        {
            if (isFound)
            {
                MovementListener.Move(
                    agent.GetMovePath(),
                    agent.GetTimes(),
                    agent.CurrentPosition,
                    agent.Remote,
                    enemy);
            }
        }

        public void MoveAgent(NavAgent agent, Vector3Int start, Vector3Int end, AgentRemote enemy)
        {
            agent.AsyncStartMove(start, end, enemy);
        }
        public void FindPathDone_OnlyMainThread(NavAgent agent, bool found)
        {
            ThreadHelper.MainThreadInvoke(() => EmitMoveEvent(agent, found, agent.TargetEnemy));
        }

        // Decision making
        public AgentRemote GetEnemyAt(Vector3Int position)
        {
            AgentNodes.GetInfo(position, out NodeInfo info);

            if (info == null) return null;
            else
            {
                AgentRemote otherAgent = info.GameObject.GetComponent<AgentRemote>();
                // is enemy
                bool isEnemy = otherAgent.UserInfo.ID_User != CurrentAgent.Remote.UserInfo.ID_User;
                return isEnemy == true ? otherAgent : null;
            }
        }

        public void Move_Action()
        {
            endCell = CursorController.SelectedPosition;
            startCell = CurrentAgent.CurrentPosition;

            MoveActiveAgent(startCell, endCell, GetEnemyAt(endCell));
        }

        public bool IsEnemyAtTarget_Boolean()
        {
            Vector3Int position = CursorController.SelectedPosition;
            return GetEnemyAt(position) != null;
        }

        public bool IsTargetInRange_Boolean(int attackRange)
        {
            if (CurrentAgent != null)
            {
                Vector3Int curPosition = CurrentAgent.CurrentPosition;
                Vector3Int targetPosition = CursorController.SelectedPosition;

                Vector3Int[] pattern = Constants.GetNeighboursRange(curPosition, attackRange);
                return pattern.IsContaint(targetPosition);
            }
            return false;
        }
    }
}