using EnumCollect;
using Generic.Singleton;
using UnityEngine;

namespace Map
{
    [RequireComponent(typeof(TowerWayPoint))]
    public class BaseTower : MonoBehaviour
    {
        public WayPoint WayPoint
        {
            get;
            private set;
        }

        [Header("Offset")]
        public TowerType Type;
        public Vector3Int ExactlyPosition;

        private GlobalNodeManager nodeManager;
        private TowerNodeManager towerNodeManager;

        private void Awake()
        {
            WayPoint = GetComponent<TowerWayPoint>();
        }

        private void Start()
        {
            nodeManager = Singleton.Instance<GlobalNodeManager>();
            towerNodeManager = nodeManager.TowerNode;

            WayPoint.Binding();
        }

        [ContextMenu("Set Position")]
        public void SetPosition()
        {
            transform.position = WayPoint.MapIns.CellToWorld(ExactlyPosition);
        }

        [ContextMenu("Get Cell Position")]
        public void GetPosition()
        {
            ExactlyPosition = WayPoint.MapIns.WorldToCell(transform.position);
        }

        public void SetPosition(Vector3Int exactlyPos)
        {
            WayPoint.Unbinding();

            ExactlyPosition = exactlyPos;
            SetPosition();

            WayPoint.Binding();
        }

        public Vector3Int GetExactlyPosition()
        {
            GetPosition();
            return ExactlyPosition;
        }
    }
}