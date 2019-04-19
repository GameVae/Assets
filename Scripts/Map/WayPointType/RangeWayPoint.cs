using Generic.Contants;
using UnityEngine;

namespace Map
{
    public abstract class RangeWayPoint : WayPoint
    {
        [SerializeField] protected int maxRange;        

        public override bool Binding()
        {
            return Manager.Add(this);
        }

        public override bool Unbinding()
        {
            return Manager.Remove(this);
        }

        public Vector3Int[] Bound()
        {
            return Constants.GetNeighboursRange(Position, maxRange);
        }
    }
}