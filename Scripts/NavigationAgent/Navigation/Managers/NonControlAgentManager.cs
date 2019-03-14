using Generic.Singleton;
using System.Collections.Generic;

namespace Entities.Navigation
{
    public sealed class NonControlAgentManager : MonoSingle<NonControlAgentManager>
    {
        private Dictionary<int, NavAgent> nCtrlAgents;

        protected override void Awake()
        {
            base.Awake();
            nCtrlAgents = new Dictionary<int, NavAgent>();
        }

        public void Add(int id, NavAgent agent)
        {
            if (!nCtrlAgents.ContainsKey(id))
            {
                nCtrlAgents[id] = agent;
                //Debugger.Log("NCC " + nCtrlAgents.Count);
            }
        }

        public bool Remove(int id)
        {
            return nCtrlAgents.Remove(id);
        }

        public void MoveAgent(JSONObject jSONObject)
        {
            int id = -1;
            jSONObject.GetField(ref id, "ID");
            if (nCtrlAgents.ContainsKey(id))
            {
                nCtrlAgents[id].StartMove(jSONObject);
            }
        }
    }
}