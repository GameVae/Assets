
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using EnumCollect;

namespace Animation
{
    public abstract class AnimatorController : MonoBehaviour
    {
        public class StateInfo
        {
            private bool isPlaying;
            public readonly bool IsTrigger;
            public readonly UnityAction PlayAct;
            public readonly UnityAction StopAct;

            public bool IsPlaying
            {
                get
                {
                    return isPlaying;
                }                
            }

            public StateInfo(bool isTrigger, UnityAction play, UnityAction stop = null)
            {
                IsTrigger = isTrigger;
                PlayAct = play;
                StopAct = stop;
            }

            public bool Play()
            {
                if(PlayAct != null && !IsPlaying)
                {
                    PlayAct.Invoke();
                    isPlaying = !IsTrigger;
                }
                return false;
            }
            public bool Stop()
            {
                if(StopAct != null && IsPlaying)
                {
                    StopAct.Invoke();
                    isPlaying = false;
                    return true;
                }
                return false;
            }
        }

        private const string SUFFIX_PLAY = "_play";
        private const string SUFFIX_STOP = "_stop";

        //private Dictionary<string, UnityAction> playlist;
        //protected Dictionary<string, UnityAction> Playlist
        //{
        //    get
        //    {
        //        return playlist ?? (playlist = new Dictionary<string, UnityAction>());
        //    }
        //}

        private Dictionary<string, StateInfo> stateList;
        protected Dictionary<string,StateInfo> StateList
        {
            get
            {
                return stateList ??
                    (stateList = new Dictionary<string, StateInfo>());
            }
        }

        public Animator Animator;

        protected abstract void RegisterAnimationState();

        protected virtual void Awake()
        {
            RegisterAnimationState();
        }

        //public void AddState(string state, UnityAction play, UnityAction stop)
        //{
        //    Playlist[state + SUFFIX_PLAY] = play;
        //    Playlist[state + SUFFIX_STOP] = stop;
        //}

        //public void AddState(AnimState state, UnityAction play, UnityAction stop)
        //{
        //    string key = state.ToString() + state.GetType().GetHashCode();
        //    AddState(key, play, stop);
        //}

        public void AddState(string state,StateInfo info)
        {
            if(!StateList.ContainsKey(state))
            {
                StateList.Add(state, info);
            }
        }

        public void AddState(AnimState state, StateInfo info)
        {
            string key = state.ToString() + state.GetType().GetHashCode();
            AddState(key, info);
        }

        public bool Play(string state)
        {
            //playlist.TryGetValue(state + SUFFIX_PLAY, out UnityAction value);
            //value?.Invoke();

            StateList.TryGetValue(state, out StateInfo info);
            if (info != null)
            {
                return info.Play();
            }

            return false;
        }

        public bool Stop(string state)
        {
            //playlist.TryGetValue(state + SUFFIX_STOP, out UnityAction value);
            //value?.Invoke();

            StateList.TryGetValue(state, out StateInfo info);
            if (info != null)
            {
                return info.Stop();
            }
            return false;
        }

        public virtual bool Play(AnimState state)
        {
            string key = state.ToString() + state.GetType().GetHashCode();
            return Play(key);
        }
                
        public virtual bool Stop(AnimState state)
        {
            string key = state.ToString() + state.GetType().GetHashCode();
            return Stop(key);
        }

        public StateInfo GetStateInfo(AnimState state)
        {
            string key = state.ToString() + state.GetType().GetHashCode();
            StateList.TryGetValue(key, out StateInfo value);
            return value;
        }
    }
}