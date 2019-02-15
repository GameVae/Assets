using Generic.Singleton;
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
        public Vector3Int CellPosision;

        private void Awake()
        {
            WayPoint = GetComponent<WayPoint>();    
        }

        private void Start()
        {
            Singleton.Instance<CellInfoManager>().AddBase(WayPoint.CellPosition, WayPoint.CellInfo, IsExpand);
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
    }
}