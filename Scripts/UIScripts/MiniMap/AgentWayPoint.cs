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
            return agentNodeManager.Remove(Position);
        }

        protected override void Awake()
        {
            base.Awake();
            agentNodeManager = nodeManager.AgentNode;
        }
    }
}
