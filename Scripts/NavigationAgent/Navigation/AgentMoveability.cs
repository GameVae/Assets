using UnityEngine;

namespace Entities.Navigation
{
    [RequireComponent(typeof(AgentRemote))]
    public abstract class AgentMoveability : MonoBehaviour
    {
        public bool IsMoving { get ; protected set; }

        private AgentRemote remote;
        private VectorRotator rotator;

        protected HexMap MapIns
        {
            get { return Remote.MapIns; }
        }

        public AgentRemote Remote
        {
            get { return remote ?? (remote = GetComponent<AgentRemote>()); }
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