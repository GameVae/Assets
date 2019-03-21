using Map;
using UnityEngine;

namespace Entities.Navigation
{
    [RequireComponent(typeof(NavRemote), typeof(AgentWayPoint))]
    public abstract class AgentMoveability : MonoBehaviour
    {
        public bool IsMoving { get ; protected set; }

        private NavRemote remote;
        private AgentWayPoint wayPoint;
        private VectorRotator rotator;

        protected NavOffset Offset
        {
            get { return Remote.Offset; }
        }

        public WayPoint WayPoint
        {
            get { return wayPoint ?? (wayPoint = GetComponent<AgentWayPoint>()); }
        }

        public NavRemote Remote
        {
            get { return remote ?? (remote = GetComponent<NavRemote>()); }
        }

        public VectorRotator Rotator
        {
            get { return rotator ?? (rotator = GetComponent<VectorRotator>()); }
        }

        protected bool Binding()
        {
            return WayPoint.Binding();
        }

        protected bool Unbinding()
        {
            return WayPoint.Unbinding();
        }

        protected abstract void UpdateMove();

        protected virtual void Update()
        {
            UpdateMove();
        }
    }
}