﻿using UnityEngine;
using UI.Widget;
using Generic.Singleton;
using Generic.CustomInput;
using UI;

namespace Entities.Navigation
{
    public sealed class NavAgentController : MonoSingle<NavAgentController>
    {
        private HexMap mapIns;
        private CrossInput crossInput;
        private UnityEventSystem eventSystem;
        private SIO_MovementListener moveEvent;
        private AgentNodeManager agentNodes;
             
        private Vector3Int startCell;
        private Vector3Int endCell;
        private bool isDisable;

        private NestedCondition moveConditions;

        public Camera CameraRaycaster;
        public GUIOnOffSwitch SwitchButton;
        public CursorController CursorController;

        public NavAgent CurrentAgent { get; private set; }
        public AgentNodeManager AgentNodes
        {
            get { return agentNodes ?? (agentNodes = Singleton.Instance<GlobalNodeManager>().AgentNode); }
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

            eventSystem = Singleton.Instance<UnityEventSystem>();
            moveEvent = FindObjectOfType<SIO_MovementListener>();

            CursorController.SelectedCallback += OnCursorSelected;
        }

        private void Start()
        {
            mapIns      = Singleton.Instance<HexMap>();
            crossInput  = Singleton.Instance<CrossInput>();
            moveEvent?.Emit("S_UNIT");
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
                return CurrentAgent != null && !eventSystem.IsPointerDownOverUI;
            };
            MoveConditions += delegate
            {
                return crossInput.IsTouch && !isDisable;
            };
            MoveConditions += IsTargetEmpty;
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

                if (!mapIns.IsValidCell(selected.x, selected.y) ||
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
                if(CurrentAgent != null && !IsTargetEmpty())
                {
                    CurrentAgent.Remote.Attack(CursorController.SelectedPosition);
                }
            }
        }
    }
}