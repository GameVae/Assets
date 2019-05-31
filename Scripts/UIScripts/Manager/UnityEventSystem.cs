using Generic.CustomInput;
using Generic.Singleton;
using UnityEngine.EventSystems;

namespace UI
{
    public sealed class UnityEventSystem : MonoSingle<UnityEventSystem>
    {
        private bool isPointerOverUI;

        public EventSystem EventSystem;

        public CrossInput CrossInput;

        public bool IsPointerDownOverUI
        {
            get { return isPointerOverUI; }
            private set { isPointerOverUI = value; }
        }

        protected override void Awake()
        {
            base.Awake();
            CrossInput = Singleton.Instance<CrossInput>();
        }

        //private void Update()
        //{
        //    if (CrossInput.CurrentState == CrossInput.PointerState.Down)
        //    {
        //        PointerDown();
        //    }
        //}

        public void PointerDown()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            IsPointerDownOverUI = EventSystem.IsPointerOverGameObject(CrossInput.GetTouch(0).fingerId);
            //AndroidAdbLog.LogInfo("Is over UI: " + IsPointerDownOverUI);
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
            IsPointerDownOverUI = EventSystem.IsPointerOverGameObject();
#endif
        }
    }
}