using Generic.Singleton;
using UnityEngine;

namespace Map
{
    [DisallowMultipleComponent]
    public sealed class WayPoint : MonoBehaviour
    {
        private CellInfo cellInfo;

        public CellInfo CellInfo
        {
            get { return cellInfo; }
            private set { cellInfo = value; }
        }

        public Vector3Int CellPosition
        {
            get { return Singleton.Instance<HexMap>().WorldToCell(transform.position).ZToZero(); }
        }

        private void Awake()
        {
            cellInfo = new CellInfo()
            {
                Id = CellInfoManager.ID(),
                GameObject = gameObject,
            };
        }

        private void OnEnable()
        {
            Binding();
        }

        private void OnDisable()
        {
            Unbinding();
        }

        public bool Binding()
        {
            return Singleton.Instance<CellInfoManager>().AddToDict(CellPosition, cellInfo);
        }

        public bool Unbinding()
        {
            return Singleton.Instance<CellInfoManager>().RemoveDict(CellPosition);
        }
    }
}
