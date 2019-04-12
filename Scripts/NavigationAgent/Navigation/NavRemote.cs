using EnumCollect;
using Generic.Singleton;
using ManualTable.Row;
using Network.Data;
using UnityEngine;

namespace Entities.Navigation
{
    public sealed class NavRemote : MonoBehaviour
    {
        private NavAgentController navAgentController;

        private NavAgent navAgent;
        private FixedMovement fixedMove;
        private UserInfoRow owner;
        private AgentAttack agentAttack;
        private CursorController cursorController;
        private AgentNodeManager agentNodes;

        private NavAgentController NavAgentCtrl
        {
            get
            {
                return navAgentController ?? (navAgentController = Singleton.Instance<NavAgentController>());
            }
        }
        public FixedMovement FixedMove
        {
            get { return fixedMove ?? (fixedMove = GetComponent<FixedMovement>()); }
        }

        public AgentMoveability MainNavAgent
        {
            get
            {
                if (IsOwner)
                    return navAgent ?? (navAgent = GetComponent<NavAgent>());
                else
                    return FixedMove;
            }
        }
        public UnitRow UnitData;

        public AgentNodeManager AgentNodes
        {
            get { return agentNodes ?? (agentNodes = Singleton.Instance<GlobalNodeManager>().AgentNode); }
        }
        public UserInfoRow UserInfo
        {
            get { return owner; }
        }
        public CursorController CursorController
        {
            get { return cursorController ?? (cursorController = FindObjectOfType<CursorController>()); }
        }

        public ListUpgrade Type;
        public AgentLabel Label;
        public NavOffset Offset;
        public AgentAttack AgentAttack
        {
            get
            {
                if (agentAttack == null)
                {
                    agentAttack = GetComponent<AgentAttack>();

                }
                return agentAttack ?? (agentAttack = gameObject.AddComponent<AgentAttack>());
            }
        }

        public int AgentID
        {
            get { return UnitData.ID; }
        }

        public bool IsOwner
        {
            get; private set;
        }

        private void Awake()
        {
            Label.LookAt(NavAgentCtrl.CameraRaycaster.transform);
        }

        private void Start()
        {
            SyncPosition();           
        }

        private void SyncPosition()
        {
            if (UnitData.AgentStatus == AgentStatus.Move)
            {
                string json = JsonUtility.ToJson(UnitData);
                JSONObject moveData = new JSONObject(json);
                FixedMove.StartMove(moveData);
            }
            else
            {
                FixedMove.Stop();
            }
        }

        public void ActiveNav()
        {
            if (IsOwner)
                NavAgentCtrl.SwitchToAgent(MainNavAgent as NavAgent);
        }

        public void SetUnitData(UnitRow data, UserInfoRow user, bool isOwner)
        {
            UnitData = data;
            owner = user;
            IsOwner = isOwner;

            Label.SetInfo(data, user, isOwner);
        }

        public void Attack(Vector3Int position)
        {
            if (!MainNavAgent.IsMoving)
            {
                if (AgentNodes.GetInfo(position, out NodeInfo info))
                {
                    JSONObject attackData = AgentAttack.S_ATTACK(info.GameObject.GetComponent<NavRemote>());
                    Singleton.Instance<EventListenersController>().Emit("S_ATTACK", attackData);
                }
            }
        }
    }
}