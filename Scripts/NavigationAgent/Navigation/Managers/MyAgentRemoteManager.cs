using Generic.Observer;
using Generic.Singleton;
using System.Collections.Generic;


namespace Entities.Navigation
{
    public sealed class MyAgentRemoteManager : MonoSingle<MyAgentRemoteManager>, ISubject
    {
        private Dictionary<int, AgentRemote> agentRemotes;
        private NavAgentController navController;

        private List<IObserver> observers;
        private List<IObserver> Observers
        {
            get
            {
                return observers ?? (observers = new List<IObserver>());
            }
        }

        public NavAgentController NavAgentController
        {
            get
            {
                return navController ?? (navController = Singleton.Instance<NavAgentController>());
            }
        }
        public Dictionary<int, AgentRemote> MyAgentRemotes
        {
            get
            {
                return agentRemotes;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            agentRemotes = new Dictionary<int, AgentRemote>();
        }

        public void Add(AgentRemote agentRemote)
        {
            if (!agentRemotes.ContainsKey(agentRemote.AgentID))
            {
                agentRemotes[agentRemote.AgentID] = agentRemote;

                NotifyAll();
            }
        }

        public bool Remove(int id)
        {
            if (agentRemotes.Remove(id))
            {
                NotifyAll();
                return true;
            }
            return false;
        }

        public void ActiveNav(int id)
        {
            if (agentRemotes.ContainsKey(id))
                NavAgentController.SwitchToAgent(agentRemotes[id].NavAgent);
        }

        public void UnActiveNav()
        {
            NavAgentController.SwitchToAgent(null);
        }

        public bool IsOwnerAgent(int id)
        {
            return agentRemotes.ContainsKey(id);
        }

        public AgentRemote GetNavRemote(int id)
        {
            agentRemotes.TryGetValue(id, out AgentRemote value);
            return value;
        }    

        public void Register(IObserver observer)
        {
            if (!Observers.Contains(observer))
            {
                Observers.Add(observer);
            }
        }
        public void Remove(IObserver observer)
        {
            Observers.Remove(observer);
        }
        public void NotifyAll()
        {
            for (int i = 0; i < Observers.Count; i++)
            {
                Observers[i].SubjectUpdated(null);
            }
        }
    }
}