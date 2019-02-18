using Generic.Contants;
using Generic.Singleton;
using UnityEngine;

namespace Generic.CustomInput
{
    public delegate void CrossInputAction();
    public sealed class CrossInput : MonoSingle<CrossInput>
    {
        private Vector2 lastMousePosition;
        private Vector2 deltaSwipe;
        private float swipeSpeed;

        private int fingerID = -1;
        private GConstants Constants;

        public event CrossInputAction OnBegan;
        public event CrossInputAction OnEnded;

        public float SwipeSpeed
        {
            get { return swipeSpeed; }
        }

        protected override void Awake()
        {
            Input.simulateMouseWithTouches = true;
            Input.multiTouchEnabled = true;
        }

        private void Start()
        {
            Constants = Singleton.Singleton.Instance<GConstants>();
        }

        private void Update()
        {

#if UNITY_EDITOR || UNITY_STANDALONE
            if (Input.GetMouseButtonDown(0))
                OnBegan?.Invoke();
            if (Input.GetMouseButtonUp(0))
                OnEnded?.Invoke();
            SwipeCalulate();
#endif
#if !UNITY_EDITOR && UNITY_ANDROID
            FirstTouchHandle();
#endif
        }
      
        private void SwipeCalulate()
        {
            deltaSwipe = (Vector2)Input.mousePosition - lastMousePosition;
            swipeSpeed = deltaSwipe.magnitude / Time.deltaTime;
        }

        private void LateUpdate()
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            lastMousePosition = Input.mousePosition;
#endif
        }

        public Vector2 DeltaSwipe()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            if(Input.touchCount > 0)
               return Input.GetTouch(0).deltaPosition
                  * Constants.PixelDependencyDevice;
            return Vector2.zero;
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
            return deltaSwipe;
#endif
        }

        public float ZoomValue()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            return GetMobileZoomValue() * Constants.PixelDependencyDevice;
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
            return Input.mouseScrollDelta.y;
#endif
        }

        #region Mobile
        private float GetMobileZoomValue()
        {
            float zoomValue = 0;
            if (Input.touchCount == 2)
            {
                Touch touchOne = Input.GetTouch(0);
                Touch touchTwo = Input.GetTouch(1);

                Vector2 preOnePos = touchOne.position - touchOne.deltaPosition;
                Vector2 preTwoPos = touchTwo.position - touchTwo.deltaPosition;

                float deltaPreMag = (preOnePos - preTwoPos).magnitude;
                float deltaTouchMag = (touchOne.position - touchTwo.position).magnitude;

                zoomValue = deltaTouchMag - deltaPreMag;
            }
            return zoomValue;
        }
        private void FirstTouchHandle()
        {
            if (fingerID == -1 && Input.touchCount > 0)
            {
                fingerID = Input.GetTouch(0).fingerId;
                OnBegan?.Invoke();
            }
            else if (fingerID != -1)
            {
                if (Input.touchCount == 0)
                {
                    fingerID = -1;
                }
                else
                {
                    bool isLife = false;
                    for (int i = 0; i < Input.touchCount; i++)
                    {
                        if (Input.GetTouch(i).fingerId == fingerID)
                        {
                            isLife = true;
                            break;
                        }
                    }
                    if (!isLife)
                    {
                        fingerID = -1;
                    }
                }
                if (fingerID == -1) OnEnded?.Invoke();
            }
        }
        #endregion
    }
}
