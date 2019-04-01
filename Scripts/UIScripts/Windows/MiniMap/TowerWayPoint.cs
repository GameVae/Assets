using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Map
{
    public class TowerWayPoint : WayPoint
    {
        [SerializeField]
        private int maxRange;
        private TowerNodeManager towerNode;

        protected override void Awake()
        {
            base.Awake();
            towerNode = nodeManager.TowerNode;
        }

        public override bool Binding()
        {
            return towerNode.AddRange(Position, NodeInfo, maxRange);
        }

        public override bool Unbinding()
        {
            return towerNode.Remove(Position, maxRange);
        }
    }
}