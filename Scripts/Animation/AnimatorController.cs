
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EnumCollect;

namespace Animation
{
    public abstract class AnimatorController : MonoBehaviour
    {
        private const string SUFFIX_PLAY = "_play";
        private const string SUFFIX_STOP = "_stop";

        private Dictionary<string, UnityAction> animPlaylist;

        public Animator Animator;

        protected abstract void RegisterAnimationState();

        protected virtual void Awake()
        {
            RegisterAnimationState();
        }

        public void AddState(string state, UnityAction play, UnityAction stop)
        {
            if (animPlaylist == null)
                animPlaylist = new Dictionary<string, UnityAction>();

            animPlaylist[state + SUFFIX_PLAY] = play;
            animPlaylist[state + SUFFIX_STOP] = stop;
        }

        public void AddState(AnimState state, UnityAction play, UnityAction stop)
        {
            if (animPlaylist == null)
                animPlaylist = new Dictionary<string, UnityAction>();

            string key = state.ToString() + state.GetType().GetHashCode();
            AddState(key, play, stop);
        }

        public void Play(string state)
        {
            animPlaylist.TryGetValue(state + SUFFIX_PLAY, out UnityAction value);
            value?.Invoke();
        }

        public void Stop(string state)
        {
            animPlaylist.TryGetValue(state + SUFFIX_STOP, out UnityAction value);
            value?.Invoke();
        }

        public void Play(AnimState state)
        {
            string key = state.ToString() + state.GetType().GetHashCode();
            Play(key);
        }

        public void Stop(AnimState state)
        {
            string key = state.ToString() + state.GetType().GetHashCode();
            Stop(key);
        }
    }
}