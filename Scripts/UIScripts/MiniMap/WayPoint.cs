using Generic.Singleton;
using UnityEngine;

namespace Map
{
    [DisallowMultipleComponent]
    public abstract class WayPoint : MonoBehaviour
    {
        private NodeInfo info;
        private HexMap hexMap;

        protected GlobalNodeManager nodeManager;

        public HexMap MapIns
        {
            get
            {
                return hexMap ?? (hexMap = Singleton.Instance<HexMap>());
            }
        }

        public NodeInfo NodeInfo
        {
            get { return info; }
            private set { info = value; }
        }

        public Vector3Int Position
        {
            get
            {
                return (Vector3Int)MapIns?.WorldToCell(transform.position).ZToZero();
            }
        }

        protected virtual void Awake()
        {
            info = new NodeInfo()
            {
                Id = GlobalNodeManager.ID(),
                GameObject = gameObject,
            };
            nodeManager = Singleton.Instance<GlobalNodeManager>();
        }

        public abstract bool Binding();

        public abstract bool Unbinding();
    }
}
