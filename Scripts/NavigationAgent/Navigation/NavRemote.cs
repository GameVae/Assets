using EnumCollect;
using Generic.Singleton;
using DataTable.Row;
using Network.Data;
using UnityEngine;
using Generic.Observer;
using Map;

namespace Entities.Navigation
{
    [RequireComponent(typeof(AgentWayPoint))]
    public sealed class NavRemote : MonoBehaviour
    {
        [SerializeField] private ListUpgrade type;
        [SerializeField] private NavOffset offset;

        [SerializeField] // TODO:[TEST] for show in inspector
        private AgentInfo agentInfo;
        
        private NavAgent navAgent;
        private FixedMovement fixedMove;
        private AgentWayPoint waypoint;

        private AgentAttack agentAttack;
        private AgentNodeManager agentNodes;

        public NavAgent NavAgent
        {
            get { return navAgent ?? (navAgent = GetComponent<NavAgent>()); }
        }
        public FixedMovement FixedMove
        {
            get { return fixedMove ?? (fixedMove = GetComponent<FixedMovement>()); }
        }
        public AgentWayPoint WayPoint
        {
            get
            {
                return waypoint ?? (waypoint = GetComponent<AgentWayPoint>());  
            }
        }

        private Observer_Unit observer;

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
        public AgentNodeManager AgentNodes
        {
            get { return agentNodes ?? (agentNodes = Singleton.Instance<GlobalNodeManager>().AgentNode); }
        }

        public UserInfoRow UserInfo
        {
            get { return AgentInfo.UserInfo; }
        }
        public UnitRow UnitInfo
        {
            get
            {
                return AgentInfo.UnitInfo;
            }
        }
        
        public ListUpgrade Type
        {
            get { return type; }
        }
        public NavOffset Offset
        {
            get { return offset; }
        }
        public int AgentID
        {
            get { return UnitInfo.ID; }
        }

        public AgentLabel Label;

        public bool IsOwner
        {
            get; private set;
        }
        public HexMap MapIns
        {
            get
            {
                return WayPoint.MapIns;
            }
        }
        public AgentInfo AgentInfo
        {
            get
            {
                return agentInfo ?? (agentInfo = GetComponent<AgentInfo>());
            }

        }
        public Vector3Int CurrentPosition
        {
            get
            {
                return WayPoint.Position;
            }
        }

        private void Awake()
        {
            // Label.LookAt(NavAgentCtrl.CameraRaycaster.transform);
        }

        private void Start()
        {
            SyncPosition();
        }

        private void SyncPosition()
        {
            if (UnitInfo.AgentStatus == AgentStatus.Move)
            {
                string json = JsonUtility.ToJson(UnitInfo);
                JSONObject moveData = new JSONObject(json);
                FixedMove.StartMove(moveData);
            }
            else
            {
                FixedMove.Stop();
            }
        }

        public void Initalize(ISubject subject, UnitRow unitData, UserInfoRow user, bool isOwner)
        {
            AgentInfo.UserInfo = user;
            AgentInfo.UnitInfo = unitData;
            IsOwner = isOwner;

            Label.SetInfo(unitData, user, isOwner);

            observer = new Observer_Unit(unitData);
            observer.OnSubjectUpdated += SubjectChanged;
            subject.Register(observer);
        }

        public JSONObject GetAttackData(Vector3Int target)
        {
            if (AgentNodes.GetInfo(target, out NodeInfo info))
            {
                return AgentAttack.S_ATTACK(info.GameObject.GetComponent<NavRemote>());
            }
            return null;
        }

        private void SubjectChanged(Observer_Unit.Package package)
        {
            UnitRow data = package.Unit;
            if (data.Quality <= 0)
            {
                Debugger.Log("DEAD");
                Label.SetInfo(data, UserInfo, IsOwner);
            }
            else
                Label.SetInfo(data, UserInfo, IsOwner);
        }

        public bool IsMoving()
        {
            bool navMoving = NavAgent == null ? false : NavAgent.IsMoving;
            bool fixedMoving = FixedMove == null ? false : FixedMove.IsMoving;
            return navMoving || fixedMoving;
        }

        public bool Binding()
        {
            return WayPoint.Binding();
        }

        public bool Unbinding()
        {
            return WayPoint.Unbinding();
        }
    }
}