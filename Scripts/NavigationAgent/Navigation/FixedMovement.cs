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
        private HexMap mapIns;
        private AnimatorController Anim;
        private MovementSerMessageHandler moveHandler;

        private void Awake()
        {
            IsMoving = false;

            Anim = GetComponent<AnimatorController>();
            mapIns = Singleton.Instance<HexMap>();
            moveHandler = new MovementSerMessageHandler(mapIns);
        }

        public void StartMove(JSONObject r_move)
        {
            moveHandler.HandlerEvent(r_move);
            speed = moveHandler.FirstStep(transform.position, out target);

            IsMoving = true;
            Rotator.Target = target;
            Rotator.IsBlock = false;
            Anim.Play(AnimState.Walking);
            Unbinding();

        }

        private void NextStep()
        {
            speed = moveHandler.NextStep(transform.position, out target);
            Rotator.Target = target;
        }

        public void Stop()
        {
            IsMoving = false;
            Rotator.IsBlock = true;
            Anim.Stop(AnimState.Walking);
            Binding();
            moveHandler.Clear();
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
                    Stop();
                }
            }
        }
    }
}