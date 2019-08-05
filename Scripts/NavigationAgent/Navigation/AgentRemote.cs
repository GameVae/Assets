using EnumCollect;
using DataTable.Row;
using UnityEngine;
using Generic.Observer;
using Generic.Pooling;
using UnityEngine.Events;
using Animation;
using Generic.Singleton;
using System;
using DataTable;

namespace Entities.Navigation
{
    [RequireComponent(typeof(AgentWayPoint))]
    public sealed class AgentRemote : MonoBehaviour, IPoolable
    {
#pragma warning disable IDE0044
        [SerializeField] private ListUpgrade type;
        [SerializeField] private NavOffset offset;
        [SerializeField] private Transform headPoint;
#pragma warning restore IDE0044

        [SerializeField] // TODO:[TEST] for show in inspector
        private AgentInfo agentInfo;

        private UnityAction<AgentRemote> deathEvents;
        public event UnityAction<AgentRemote> OnDead
        {
            add { deathEvents += value; }
            remove { deathEvents -= value; }
        }

        private JSONTable_Unit unitSubject;
        private Observer_Unit observer;

        private NavAgent navAgent;
        private FixedMovement fixedMove;
        private AgentWayPoint waypoint;
        private AnimatorController animator;
        private AgentRemoteManager agentRemoteManager;

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
        public AgentRemoteManager AgentRemoteManager
        {
            get
            {
                return agentRemoteManager ??
                    (agentRemoteManager = Singleton.Instance<AgentRemoteManager>());
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
        public int ManagedId
        {
            get; private set;
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
            JSONTable_Unit subject,
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

            //string format = IsOwner ? "{0}" : "<color=red>{0}</color>"; // not friend

            //if (!IsOwner)
            //{
            //    bool isFriend = Singleton.Instance<FriendSys>().IsMyFriend(user.ID_User);
            //    format = isFriend ? "<color=green>{0}</color>" : format; // friend
            //}

            string format = GetLabelFormat();


            Label.NameInGame = string.Format(format, "Id " + unitData.ID + ": " + user.NameInGame);
            Label.SetHP(unitData.Hea_cur, unitData.Health);
            Label.Quality = unitData.Quality;
            Label.gameObject.SetActive(true);

            observer = subject.ObserverPooling.GetItem();            
            observer.RefreshSubject(unitData);
            observer.OnSubjectUpdated += SubjectChanged;
            subject.Register(observer);
        }

        private void SubjectChanged(Observer_Unit.Package package)
        {
            agentInfo.UnitInfo = package.Unit;

            if (UnitInfo.Quality <= 0)
            {
                // Dead();
                NavAgent.Stop();
                Animator.Play(AnimState.Dead);
            }
            else
            {
                CheckAttack();
                SetLabel();
            }

        }

        private string GetLabelFormat()
        {
            string format = IsOwner ? "{0}" : "<color=red>{0}</color>"; // not friend

            if (!IsOwner)
            {
                bool isFriend = Singleton.Instance<FriendSys>().IsMyFriend(UserInfo.ID_User);
                format = isFriend ? "<color=green>{0}</color>" : format; // friend
            }

            return format;
        }

        public void RefreshNameLable()
        {
            string lableFormat = GetLabelFormat();
            Label.NameInGame = string.Format(lableFormat, "Id " + UnitInfo.ID + ": " + UserInfo.NameInGame);
        }

        private void SetLabel()
        {
            Label.SetHP(UnitInfo.Hea_cur, UnitInfo.Health);
            Label.Quality = UnitInfo.Quality;
        }

        private void CheckAttack()
        {
            if (!string.IsNullOrEmpty(UnitInfo.Attack_Unit_ID) &&
                     UnitInfo.Attack_Unit_ID.ToLower() != "null")
            {
                string[] strs = UnitInfo.Attack_Unit_ID.Split('_');
                int otherID = int.Parse(strs[3]);

                AgentRemote other = AgentRemoteManager.GetAgentRemote(otherID);
                if (other != null && !IsMoving())
                {
                    transform.forward = (other.transform.position - transform.position).normalized;
                    Animator.Play(AnimState.Attack1);
                }
            }
            else
            {
                AnimatorController.StateInfo attackState = Animator.GetStateInfo(AnimState.Attack1);
                if (attackState != null && attackState.IsPlaying)
                {
                    attackState.Stop();
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
            Debugger.Log(AgentID + ": dead called from animation");
            unitSubject.Remove(observer);
            deathEvents?.Invoke(this);
            deathEvents = null;


            FixedMove.Stop();
            NavAgent?.Stop();
            Unbinding();
            unitSubject.Remove(observer);
            unitSubject.ObserverPooling.Release(observer);
        }

        // IPoolable.Interface
        public void FirstSetup(int insId)
        {
            ManagedId = insId;
        }

        public void Dispose()
        {

            gameObject.SetActive(false);
        }
    }
}