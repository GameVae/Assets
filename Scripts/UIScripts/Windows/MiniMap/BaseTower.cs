using EnumCollect;
using UnityEngine;

namespace Map
{
    [RequireComponent(typeof(ConstructWayPoint))]
    public class BaseTower : MonoBehaviour
    {
        private ConstructWayPoint wayPoint;
        public WayPoint WayPoint
        {
            get
            {
                return wayPoint ?? (wayPoint = GetComponent<ConstructWayPoint>());
            }
            
        }

        [Header("Offset")]
        public TowerType Type;
        public Vector3Int ExactlyPosition;

        private void Start()
        {
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