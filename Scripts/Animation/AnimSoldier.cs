
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
        }

        private void Walking(bool value)
        {
            Animator.SetBool("walking", value);
        }
    }
}