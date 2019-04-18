using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class AgentWayPoint : WayPoint
    {
        private AgentNodeManager agentNodeManager;

        public override bool Binding()
        {
            return agentNodeManager.Add(Position, NodeInfo);
        }

        public override bool Unbinding()
        {
            if(agentNodeManager.GetInfo(Position, out NodeInfo info))
            {
                if(info == NodeInfo) // agent only remove its info otherwise return false (ignore)
                {
                    return agentNodeManager.Remove(Position);

                }
            }
            return false;
        }

        protected override void Awake()
        {
            base.Awake();
            agentNodeManager = NodeManager.AgentNode;
        }
    }
}
