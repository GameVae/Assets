using Generic.Singleton;
using UnityEngine;

namespace Map
{
    [DisallowMultipleComponent]
    public sealed class WayPoint : MonoBehaviour
    {
        private NodeInfo info;
        private HexMap hexMap;
        private GlobalNodeManager nodeManager;
        private AgentNodeManager agentNodeManager;

        public NodeInfo NodeInfo
        {
            get { return info; }
            private set { info = value; }
        }

        public Vector3Int Position
        {
            get
            {
                if (hexMap == null) hexMap = Singleton.Instance<HexMap>();
                return hexMap.WorldToCell(transform.position).ZToZero();
            }
        }

        public bool IsTower;

        private void Awake()
        {
            info = new NodeInfo()
            {
                Id = GlobalNodeManager.ID(),
                GameObject = gameObject,
            };
            nodeManager = Singleton.Instance<GlobalNodeManager>();
            agentNodeManager = nodeManager.AgentNode;
        }

        private void Start()
        {
            if (hexMap == null) hexMap = Singleton.Instance<HexMap>();          
        }

        public bool Binding()
        {
            if (!IsTower)
            {
                agentNodeManager.Add(Position, NodeInfo);
                return true;
            }
            return false;
        }

        public bool Unbinding()
        {
            return agentNodeManager.Remove(Position);
        }
    }
}
