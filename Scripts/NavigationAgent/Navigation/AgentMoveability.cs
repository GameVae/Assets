using UnityEngine;

namespace Entities.Navigation
{
    [RequireComponent(typeof(NavRemote))]
    public abstract class AgentMoveability : MonoBehaviour
    {
        public bool IsMoving { get ; protected set; }

        private NavRemote remote;
        private VectorRotator rotator;

        protected HexMap MapIns
        {
            get { return Remote.MapIns; }
        }

        public NavRemote Remote
        {
            get { return remote ?? (remote = GetComponent<NavRemote>()); }
        }
        public VectorRotator Rotator
        {
            get { return rotator ?? (rotator = GetComponent<VectorRotator>()); }
        }
        public Vector3Int CurrentPosition
        {
            get
            {
                return Remote.CurrentPosition;
            }
        }

        protected abstract void UpdateMove();

        protected virtual void Update()
        {
            UpdateMove();
        }
    }
}