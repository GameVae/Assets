using EnumCollect;
using Generic.Singleton;
using ManualTable.Row;
using UnityEngine;

namespace Entities.Navigation
{
    [RequireComponent(typeof(NavAgent))]
    public sealed class NavRemote : MonoBehaviour
    {
        private NavAgentController ctrl;
        private NavAgent thisAgent;

        private UnitRow agentData;
        private UserInfoRow owner;

        public ListUpgrade Type;
        public AgentLabel Label;

        public int AgentID
        {
            get { return agentData.ID; }
        }

        public NavAgent Agent
        {
            get
            {
                return thisAgent ?? (thisAgent = GetComponent<NavAgent>());
            }
        }

        private void Awake()
        {
            ctrl = Singleton.Instance<NavAgentController>();
            Label.LookAt(ctrl.CameraRaycaster.transform);
        }

        public void ActiveNav()
        {
            ctrl.SwitchToAgent(Agent);
        }

        public void Init(UnitRow data, UserInfoRow user)
        {
            agentData = data;
            owner = user;

            InitLabel();
        }

        public void SetAgentData(UnitRow data)
        {
            agentData = data;
            InitLabel();
        }

        private void InitLabel()
        {
            Label.SetMaxHP(agentData.Health);
            Label.SetHp(agentData.Hea_cur);
            Label.SetQuality(agentData.Quality);
            Label.SetNameInGame(owner?.NameInGame + " Id " + agentData.ID);
        }
    }
}