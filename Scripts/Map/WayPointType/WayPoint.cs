using Generic.Singleton;
using UnityEngine;

namespace Map
{
    [DisallowMultipleComponent]
    public abstract class WayPoint : MonoBehaviour
    {
        private NodeInfo info;
        private HexMap mapIns;
        private GlobalNodeManager nodeManager;

        public HexMap MapIns
        {
            get
            {
                return mapIns ?? (mapIns = Singleton.Instance<HexMap>());
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
                return MapIns.WorldToCell(transform.position).ZToZero();
            }
        }

        public GlobalNodeManager NodeManager
        {
            get
            {
                return nodeManager ?? (nodeManager = Singleton.Instance<GlobalNodeManager>());
            }
        }

        private NodeManagerProvider nodeManagerProvider;
        public NodeManagerProvider NodeManagerProvider
        {
            get
            {
                return nodeManagerProvider ?? 
                    (nodeManagerProvider = Singleton.Instance<NodeManagerProvider>());
            }
        }
        protected abstract INodeManager Manager
        {
            get;
        }

        protected virtual void Awake()
        {
            info = new NodeInfo()
            {
                Id = GlobalNodeManager.ID(),
                GameObject = gameObject,
            };
        }

        public abstract bool Binding();

        public abstract bool Unbinding();
    }
}
