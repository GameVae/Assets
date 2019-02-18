using UnityEngine;

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

        private bool isOpen;
        private float timmer;
        private bool isAnimating;
        private Vector2 deltaSize;
        private Vector2 defaultSize;

        public float Duration;
        public Vector2 MaxSize;
        public RectTransform RefRect;
        public Direction HorizontalDirection;
        public Direction VerticalDirection;

        private void Awake()
        {
            defaultSize = RefRect.Size();
            InitPivot();

        }
        private void Start()
        {


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
                    if (!isOpen)
                    {
                        RefRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MaxSize.x);
                        RefRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MaxSize.y);
                        isOpen = true;
                    }
                    else
                    {
                        RefRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, defaultSize.x);
                        RefRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, defaultSize.y);
                        isOpen = false;
                    }
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

        public void Action()
        {
            if (isOpen)
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
    }
}