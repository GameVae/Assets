using System;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Animation
{
    public class ResizeAnimation : MonoBehaviour
    {
        public enum Direction
        {
            None,
            Left,
            Right,
            Up,
            Down
        }

        private float timmer;
        private bool isAnimating;
        private Vector2 deltaSize;
        private Vector2 defaultSize;

        private UnityAction openDoneAction;
        private UnityAction closeDoneAction;

        public float Duration;
        public Vector2 MaxSize;
        public RectTransform RefRect;
        public Direction HorizontalDirection;
        public Direction VerticalDirection;

        public bool IsOpen { get; private set; }

        public event UnityAction OpenDoneEvt
        {
            add { openDoneAction += value; }
            remove { openDoneAction -= value; }
        }
        public event UnityAction CloseDoneEvt
        {
            add { closeDoneAction += value; }
            remove { closeDoneAction -= value; }
        }

        private void Awake()
        {
            defaultSize = RefRect.Size();
            InitPivot();

        }

        private void Update()
        {
            if (isAnimating)
            {
                timmer += Time.deltaTime;
                RefRect.SetSizeWithCurrentAnchors
                    (RectTransform.Axis.Horizontal, RefRect.Size().x + deltaSize.x * Time.deltaTime);
                RefRect.SetSizeWithCurrentAnchors
                    (RectTransform.Axis.Vertical, RefRect.Size().y + deltaSize.y * Time.deltaTime);
                if (timmer >= Duration)
                {
                    timmer = 0;
                    if (!IsOpen)
                    {
                        RefRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MaxSize.x);
                        RefRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MaxSize.y);
                        IsOpen = true;
                    }
                    else
                    {
                        RefRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, defaultSize.x);
                        RefRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, defaultSize.y);
                        IsOpen = false;
                    }
                    AnimationDone(IsOpen);
                    isAnimating = false;
                }
            }
        }

        public void Open()
        {
            if (!isAnimating)
            {
                deltaSize = (MaxSize - RefRect.Size()) / Duration;
                isAnimating = true;
            }
        }

        public void Close()
        {
            if (!isAnimating)
            {
                deltaSize = (defaultSize - MaxSize) / Duration;
                isAnimating = true;
            }
        }

        public void ForceMaxSize()
        {
            RefRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MaxSize.x);
            RefRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MaxSize.y);
        }

        public void Action()
        {
            if (IsOpen)
                Close();
            else Open();
        }

        private void InitPivot()
        {
            Vector2 pivot = RefRect.pivot;
            if (HorizontalDirection != Direction.None)
                pivot.x = HorizontalDirection == Direction.Right ? 0 : 1;
            if (VerticalDirection != Direction.None)
                pivot.y = VerticalDirection == Direction.Up ? 0 : 1;
            RefRect.SetPivotWithoutChangePosition(pivot);
        }

        private void AnimationDone(bool opened)
        {
            if (opened)
            {
                openDoneAction?.Invoke();
            }
            else
            {
                closeDoneAction?.Invoke();
            }
        }
    }
}