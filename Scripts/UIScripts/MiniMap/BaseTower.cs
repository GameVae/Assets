using Generic.Singleton;
using ManualTable.Row;
using UnityEngine;

namespace Map
{
    [RequireComponent(typeof(WayPoint))]
    public class BaseTower : MonoBehaviour
    {
        public WayPoint WayPoint
        {
            get;
            private set;
        }

        public bool IsExpand;
        public bool IsDependentServer;
        public int BaseId;
        public Vector3Int CellPosision;

        private int maxRange;
        private Connection Conn;
        private GlobalNodeManager nodeManager;
        private TowerNodeManager towerNodeManager;
        private void Awake()
        {
            WayPoint = GetComponent<WayPoint>();
            WayPoint.IsTower = true;
        }

        private void Start()
        {
            SetupPositionBaseOnServerData();
            SetPosition();

            nodeManager = Singleton.Instance<GlobalNodeManager>();
            towerNodeManager = nodeManager.TowerNode;

            maxRange = IsExpand ? 2 : 1;
            towerNodeManager.AddRange(WayPoint.Position, WayPoint.NodeInfo, maxRange);
        }

        [ContextMenu("Set Position")]
        public void SetPosition()
        {
            transform.position = Singleton.Instance<HexMap>().CellToWorld(CellPosision);
        }

        [ContextMenu("Get Cell Position")]
        public void GetPosition()
        {
            CellPosision = Singleton.Instance<HexMap>().WorldToCell(transform.position);
        }

        private bool SetupPositionBaseOnServerData()
        {
            if (IsDependentServer)
            {
                Conn = Singleton.Instance<Connection>();
                int count = Conn.Sync.BaseInfo.Count;
                for (int i = 0; i < count; i++)
                {
                    BaseInfoRow baseInfo = Conn.Sync.BaseInfo[i] as BaseInfoRow;
                    if (baseInfo.BaseNumber == BaseId)
                    {
                        CellPosision = baseInfo.Position.Parse3Int() + new Vector3Int(5, 5, 0);
                        return true;
                    }
                }
            }
            return false;
        }
    }
}