using Animation;
using EnumCollect;
using Generic.Contants;
using Generic.Singleton;
using UnityEngine;

namespace Entities.Navigation
{
    public sealed class FixedMovement : AgentMoveability
    {
        private float speed;
        private Vector3 target;
        private AnimatorController anim;
        private MovementSerMessageHandler moveHandler;

        public AnimatorController AnimatorController
        {
            get
            {
                return anim ?? (anim = GetComponent<AnimatorController>());
            }
        }

        public MovementSerMessageHandler MoveHandler
        {
            get
            {
                return moveHandler ?? (moveHandler = new MovementSerMessageHandler(MapIns));
            }
        }

        private void Awake()
        {
            IsMoving = false;
        }

        public void StartMove(JSONObject r_move)
        {
            MoveHandler.HandlerEvent(r_move);
            speed = MoveHandler.FirstStep(transform.position, out target);

            IsMoving = true;
            Rotator.Target = target;
            Rotator.IsBlock = false;
            AnimatorController.Play(AnimState.Walking);

            Remote.Unbinding();
        }

        private void NextStep()
        {
            speed = MoveHandler.NextStep(transform.position, out target);
            Rotator.Target = target;
        }

        public void Stop()
        {
            IsMoving = false;
            Rotator.IsBlock = true;
            AnimatorController.Stop(AnimState.Walking);
            MoveHandler.Clear();

            Remote.Binding();
        }

        protected override void UpdateMove()
        {
            if (IsMoving)
            {
                if (speed > 0)
                {
                    transform.position = Vector3.MoveTowards(
                        current: transform.position,
                        target: target,
                        maxDistanceDelta: Time.deltaTime * speed);

                    if ((transform.position - target).magnitude <= Constants.TINY_VALUE)
                    {
                        NextStep();
                    }
                }
                else
                {
                    if (Remote.IsOwner)
                    {
                        Remote.NavAgent.MoveFinish();
                    }
                    else
                        Stop();
                }
            }
        }
    }
}