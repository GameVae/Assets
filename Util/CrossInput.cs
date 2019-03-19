﻿using System;
using Generic.Contants;
using Generic.Singleton;
using UnityEngine;

namespace Generic.CustomInput
{
    public sealed class CrossInput : MonoSingle<CrossInput>
    {
        public enum PointerState
        {
            Free,Down,Up,Press
        }

        private PointerState pointerState = PointerState.Free;
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

         


        public bool IsPointerUp
        {
            get
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                return TouchCount == 1 &&
                    (GetTouch(0).phase == TouchPhase.Ended || GetTouch(0).phase == TouchPhase.Canceled);
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
                return pointerState == PointerState.Up;
#endif
            }
        }
        public Vector2 Axises
        {
            get
            {
#if !UNITY_EDITOR && UNITY_ANDROID
                if (TouchCount == 1)
                {
                    axises = GetTouch(0).deltaPosition;                    
                } else axises = Vector2.zero;
                return axises * Constants.PixelDependencyDevice;
#endif

#if UNITY_EDITOR || UNITY_STANDALONE
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

        #region Touch Properties
        public int TouchCount
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            get { return Input.touchCount; }
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
            get { return Input.GetMouseButton(0) ? 1 : 0; }
#endif
        }

        public Touch GetTouch(int index)
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            Debugger.WarningLog("Editor not support touch");
            return default(Touch);
#endif
#if !UNITY_EDITOR && UNITY_ANDROID
            if (TouchCount > index)
                return Input.GetTouch(index);

            Debugger.ErrorLog("Out of Range of Touching");
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
#if UNITY_EDITOR || UNITY_STANDALONE
            RecordMouseState();
            pointerState = RecordPointerState(pointerState);
#endif
        }

        private PointerState RecordPointerState(PointerState state)
        {
           switch(state)
            {
                case PointerState.Free:
                    {
                        if (Input.GetMouseButtonDown(0))
                            return PointerState.Down;
                        break;
                    }
                case PointerState.Down:
                    {
                        if (Input.GetMouseButton(0))
                            return PointerState.Press;
                        break;
                    }
                case PointerState.Press:
                    {
                        if (Input.GetMouseButton(0))
                            return PointerState.Press;
                        if (Input.GetMouseButtonUp(0))
                            return PointerState.Up;
                        break;
                    }

            }
            return PointerState.Free;
        }

        public float ZoomValue()
        {
#if !UNITY_EDITOR && UNITY_ANDROID
            return GetMobileZoomValue();
#endif
#if UNITY_EDITOR || UNITY_STANDALONE
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
