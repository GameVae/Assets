using Generic.Singleton;
using DataTable;
using DataTable.Row;
using Network.Data;
using SocketIO;
using System.Collections.Generic;
using System.Linq;

namespace Entities.Navigation
{
    public sealed class OwnerNavAgentManager : MonoSingle<OwnerNavAgentManager>
    {
        private Dictionary<int, NavRemote> agentRemotes;
        private NavAgentController navController;

        private NavAgentController NavCtrl
        {
            get
            {
                return navController ?? (navController = Singleton.Instance<NavAgentController>());
            }
        }

        protected override void Awake()
        {
            base.Awake();
            agentRemotes = new Dictionary<int, NavRemote>();
        }

        public void Add(NavRemote agentRemote)
        {
            if (!agentRemotes.ContainsKey(agentRemote.AgentID))
            {
                agentRemotes[agentRemote.AgentID] = agentRemote;
            }
        }

        public bool Remove(int id)
        {
            return agentRemotes.Remove(id);
        }

        public void ActiveNav(int id)
        {
            if (agentRemotes.ContainsKey(id))
                NavCtrl.SwitchToAgent(agentRemotes[id].NavAgent);
        }

        public bool IsOwnerAgent(int id)
        {
            return agentRemotes.ContainsKey(id);
        }

        public NavRemote GetNavRemote(int id)
        {
            agentRemotes.TryGetValue(id, out NavRemote value);
            return value;
        }

        public void UnSelectCurrentAgent()
        {
            NavCtrl.SwitchToAgent(null);
        }
    }
}