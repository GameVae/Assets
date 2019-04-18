using EnumCollect;
using DataTable.Row;
using UnityEngine;
using Generic.Observer;
using Map;
using Generic.Pooling;
using UnityEngine.Events;

namespace Entities.Navigation
{
    [RequireComponent(typeof(AgentWayPoint))]
    public sealed class NavRemote : MonoBehaviour, IPoolable
    {
        [SerializeField] private ListUpgrade type;
        [SerializeField] private NavOffset offset;

        [SerializeField] // TODO:[TEST] for show in inspector
        private AgentInfo agentInfo;

        private UnityAction<NavRemote> deathEvents;
        public event UnityAction<NavRemote> OnDead
        {
            add { deathEvents += value; }
            remove { deathEvents -= value; }
        }

        private ISubject unitSubject;
        private Observer_Unit observer;

        private NavAgent navAgent;
        private FixedMovement fixedMove;
        private AgentWayPoint waypoint;

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

        public int ManagedId { get; private set; }

        private void Awake()
        {
            Label.LookAt(Camera.main.transform);
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

        public void Initalize(ISubject subject,
            UnitRow unitData,
            UserInfoRow user,
            bool isOwner)

        {
            unitSubject = subject;
            AgentInfo.UserInfo = user;
            AgentInfo.UnitInfo = unitData;
            IsOwner = isOwner;

            Label.SetInfo(unitData, user, isOwner);

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
            NavAgent?.Dead();
            Unbinding();

            unitSubject.Remove(observer);
            observer.Dispose();
            gameObject.SetActive(false);
        }
    }
}