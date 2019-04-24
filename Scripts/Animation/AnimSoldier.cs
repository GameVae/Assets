
using EnumCollect;
using UnityEngine;

namespace Animation
{
    public class AnimSoldier : AnimatorController
    {
        protected override void RegisterAnimationState()
        {
            AddState(state: AnimState.Walking,
                play: delegate { Walking(true); },
                stop: delegate { Walking(false); });

            AddState(state: AnimState.Attack1,
                play: Attack1,
                stop: null);
        }

        private void Walking(bool value)
        {
            Animator.SetBool("walking", value);
        }

        private void Attack1()
        {
            Animator.SetTrigger("attack");
        }
    }
}