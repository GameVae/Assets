using EnumCollect;
using DataTable.Row;
using UnityEngine;
using Generic.Observer;
using Map;
using Generic.Pooling;
using UnityEngine.Events;
using Animation;
using Generic.Singleton;

namespace Entities.Navigation
{
    [RequireComponent(typeof(AgentWayPoint))]
    public sealed class AgentRemote : MonoBehaviour, IPoolable
    {
        [SerializeField] private ListUpgrade type;
        [SerializeField] private NavOffset offset;
        [SerializeField] private Transform headPoint;

        [SerializeField] // TODO:[TEST] for show in inspector
        private AgentInfo agentInfo;

        private UnityAction<AgentRemote> deathEvents;
        public event UnityAction<AgentRemote> OnDead
        {
            add { deathEvents += value; }
            remove { deathEvents -= value; }
        }

        private ISubject<Observer_Unit> unitSubject;
        private Observer_Unit observer;

        private NavAgent navAgent;
        private FixedMovement fixedMove;
        private AgentWayPoint waypoint;
        private AnimatorController animator;

        public AnimatorController Animator
        {
            get { return animator ?? (animator = GetComponent<AnimatorController>()); }
        }
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
        public Transform HeadPoint
        {
            get { return headPoint; }
        }

        public LightweightLabel Label;

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

        public int ManagedId { get; private set; }

        private NonControlAgentManager otherAgents;
        public NonControlAgentManager OtherAgent
        {
            get
            {
                return otherAgents ?? (otherAgents = Singleton.Instance<NonControlAgentManager>());
            }
        }

        private OwnerNavAgentManager ownerAgents;
        public OwnerNavAgentManager OwnerAgents
        {
            get
            {
                return ownerAgents ?? (ownerAgents = Singleton.Instance<OwnerNavAgentManager>());
            }
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

        public void Initalize(
            ISubject<Observer_Unit> subject,
            LightweightLabel label,
            UnitRow unitData,
            UserInfoRow user,
            bool isOwner)

        {
            Label = label;
            unitSubject = subject;

            AgentInfo.UserInfo = user;
            AgentInfo.UnitInfo = unitData;

            IsOwner = isOwner;

            string format = IsOwner ? "{0}" : "<color=red>{0}</color>";
            Label.NameInGame = string.Format(format, "Id " + unitData.ID + ": " + user.NameInGame);
            Label.SetHP(unitData.Hea_cur, unitData.Health);
            Label.Quality = unitData.Quality;

            observer = new Observer_Unit(unitData);
            observer.OnSubjectUpdated += SubjectChanged;
            subject.Register(observer);
        }

        private void SubjectChanged(Observer_Unit.Package package)
        {
            UnitRow data = package.Unit;
            if (data.Quality <= 0)
            {
                Dead();
            }
            else
            {
                Label.SetHP(data.Hea_cur, data.Health);
                Label.Quality = data.Quality;

                if (!string.IsNullOrEmpty(data.Attack_Unit_ID) &&
                    data.Attack_Unit_ID != "NULL")
                {
                    string[] strs = data.Attack_Unit_ID.Split('_');
                    int otherID = int.Parse(strs[3]);

                    if (otherID != AgentID)
                    {
                        Animator.Play(AnimState.Attack1);
                        FixedMovement other = OtherAgent.GetAgent(otherID);
                        transform.forward = (other.transform.position - transform.position).normalized;
                    }
                }
            }

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

        public void Dead()
        {
            deathEvents?.Invoke(this);
            deathEvents = null;
        }

        // IPoolable.Interface
        public void FirstSetup(int insId)
        {
            ManagedId = insId;
        }

        public void Dispose()
        {
            Unbinding();

            unitSubject.Remove(observer);
            observer.Dispose();
            gameObject.SetActive(false);
        }
    }
}