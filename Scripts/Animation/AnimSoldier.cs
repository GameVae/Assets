using EnumCollect;
using UnityEngine;

namespace Animation
{
    public class AnimSoldier : AnimatorController
    {
        protected override void RegisterAnimationState()
        {
            AddState(state: AnimState.Walking,
                    info: new StateInfo(
                        isTrigger: false,
                        play: () => Walking(true),
                        stop: () => Walking(false))
                    );

            AddState(state: AnimState.Attack1,
                    info: new StateInfo(
                        isTrigger: true,
                        play: () => Attack1(true),
                        stop: () => Attack1(false))
                    );

            AddState(state: AnimState.Dead, 
                    info: new StateInfo(
                        isTrigger: true,
                        play: Dead,
                        stop: null)
                    );
        }

        private void Walking(bool value)
        {
            Stop(AnimState.Attack1);
            Animator.SetBool("walking", value);
        }

        private void Attack1(bool value)
        {
            //Animator.SetBool("attack", value);
            Animator.SetTrigger("attack");
        }

        private void Dead()
        {
            Animator.SetTrigger("death");
        }
    }
}