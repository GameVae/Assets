using Generic.Contants;
using Generic.Singleton;
using UnityEngine;

namespace Generic.CustomInput
{
    public sealed class CrossInput : MonoSingle<CrossInput>
    {
        public enum PointerState
        {
            Free, Down, Up, Press, Swipe
        }

        private PointerState pointerState = PointerState.Free;
        private PointerState lastPointerState = PointerState.Free;

        private Vector3 lastPosition;
        private Vector2 axises;

        private Constants constants;
        private Constants Constants
        {
            get
            {
                return constants ?? (constants = Singleton.Singleton.Instance<Constants>());
            }
        }

        public PointerState CurrentState
        {
            get { return pointerState; }
        }

        public PointerState LastState
        {
            get { return lastPointerState; }
        }

        public bool IsTouch
        {
            get
            {
                return pointerState == PointerState.Up && lastPointerState != PointerState.Swipe;
            }
        }

        public bool IsPointerUp
        {
            get
            {
#if ( UNITY_REMOTE  && UNITY_EDITOR) || (UNITY_ANDROID && !UNITY_EDITOR) 
                return TouchCount == 1 &&
                    (GetTouch(0).phase == TouchPhase.Ended || GetTouch(0).phase == TouchPhase.Canceled);

#else
                return pointerState == PointerState.Up;
#endif
            }
        }

        public Vector2 Axises
        {
            get
            {
#if ( UNITY_REMOTE  && UNITY_EDITOR) || (UNITY_ANDROID && !UNITY_EDITOR) 
                if (TouchCount == 1)
                {
                    axises = GetTouch(0).deltaPosition;
                }
                else axises = Vector2.zero;
                return axises * Constants.PixelDependencyDevice;
#endif

#if !UNITY_REMOTE  || UNITY_STANDALONE
                return axises * Constants.PixelDependencyDevice;
#endif
            }
        }

        public Vector2 SwipeDirection
        {
            get
            {
                return Axises == Vector2.zero ? Vector2.zero : Axises.normalized;
            }
        }

        public Vector3 Position
        {
            get
            {
#if ( UNITY_REMOTE  && UNITY_EDITOR) || (UNITY_ANDROID && !UNITY_EDITOR) 
                if (TouchCount > 0)
                    return GetTouch(0).position;
                return Vector3.zero;
#else
                return Input.mousePosition;
#endif
            }
        }
#region Touch Properties
        public int TouchCount
        {
#if ( UNITY_REMOTE  && UNITY_EDITOR) || (UNITY_ANDROID && !UNITY_EDITOR) 
            get { return Input.touchCount; }
#else
            get { return Input.GetMouseButton(0) ? 1 : 0; }
#endif
        }

        public Touch GetTouch(int index)
        {
#if ( UNITY_REMOTE  && UNITY_EDITOR) || (UNITY_ANDROID && !UNITY_EDITOR)
            if (TouchCount > index)
                return Input.GetTouch(index);

            Debugger.ErrorLog("Out of Range of Touching");
            return default(Touch);
#else
            Debugger.WarningLog("Editor not support touch");
            return default(Touch);
#endif
        }
#endregion

        protected override void Awake()
        {
            Input.simulateMouseWithTouches = false;
            Input.multiTouchEnabled = true;
        }

        private void LateUpdate()
        {
#if (UNITY_REMOTE && UNITY_EDITOR) || (UNITY_ANDROID && !UNITY_EDITOR)
            pointerState = MobilePointerState(pointerState);
#else
            RecordMouseState();
            pointerState = EditorPointerState(pointerState);
#endif
        }

        private PointerState EditorPointerState(PointerState state)
        {
            lastPointerState = state;
            switch (state)
            {
                case PointerState.Free:
                    {
                        if (Input.GetMouseButtonDown(0))
                            return PointerState.Down;
                        return PointerState.Free;
                    }
                case PointerState.Down:
                    {
                        if (Input.GetMouseButton(0))
                            return PointerState.Press;
                        return PointerState.Up;
                    }
                case PointerState.Press:
                    {
                        if (Input.GetMouseButtonUp(0))
                            return PointerState.Up;
                        if (Axises.magnitude > 0.025f)
                            return PointerState.Swipe;
                        return PointerState.Press;
                    }
                case PointerState.Up:
                    {
                        return PointerState.Free;
                    }
                case PointerState.Swipe:
                    {
                        if (Input.GetMouseButtonUp(0))
                            return PointerState.Up;
                        return PointerState.Swipe;
                    }
            }
            return PointerState.Free;
        }

        private PointerState MobilePointerState(PointerState state)
        {
            lastPointerState = state;
            switch (state)
            {
                case PointerState.Free:
                    {
                        if (TouchCount == 1)
                            return PointerState.Down;
                        return PointerState.Free;
                    }
                case PointerState.Down:
                    {
                        if (TouchCount == 1)
                            return PointerState.Press;
                        return PointerState.Up;
                    }
                case PointerState.Press:
                    {
                        if (TouchCount != 1)
                            return PointerState.Up;
                        if (Axises.magnitude > 0.01f)
                        {
                            return PointerState.Swipe;
                        }
                        return PointerState.Press;
                    }
                case PointerState.Up:
                    {
                        return PointerState.Free;
                    }
                case PointerState.Swipe:
                    {
                        if (TouchCount != 1)
                            return PointerState.Up;
                        return PointerState.Swipe;
                    }
                default:
                    return PointerState.Free;
            }
        }

        public float ZoomValue()
        {
#if (UNITY_REMOTE && UNITY_EDITOR) || (UNITY_ANDROID && !UNITY_EDITOR)
            return GetMobileZoomValue();
#else
            return Input.mouseScrollDelta.y;
#endif
        }


        #region Editor
        private void RecordMouseState()
        {
            axises = Input.mousePosition - lastPosition;
            lastPosition = Input.mousePosition;
        }

        #endregion

        #region  Mobile

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
            return (zoomValue * Constants.PixelDependencyDevice) / Time.deltaTime;
        }
        #endregion
    }
}
