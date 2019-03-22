using Generic.Singleton;
using ManualTable;
using ManualTable.Row;
using System.Collections.Generic;


namespace Entities.Navigation
{
    public sealed class NonControlAgentManager : MonoSingle<NonControlAgentManager>
    {
        private Dictionary<int, FixedMovement> nCtrlAgents;

        protected override void Awake()
        {
            base.Awake();
            nCtrlAgents = new Dictionary<int, FixedMovement>();
        }

        public void Add(int id, FixedMovement agent)
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

        public FixedMovement GetAgent(int id)
        {
            nCtrlAgents.TryGetValue(id, out FixedMovement value);
            return value;
        }
    }
}