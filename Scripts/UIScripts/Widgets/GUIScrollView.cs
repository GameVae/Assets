using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    [RequireComponent(typeof(ScrollRect))]
    public class GUIScrollView : CustomGUI
    {
        [SerializeField, HideInInspector] private ScrollRect scrollRect;

        public RectTransform Content
        {
            get { return ScrollRect?.content; }
        }

        public ScrollRect ScrollRect
        {
            get { return scrollRect ?? (scrollRect = GetComponent<ScrollRect>()); }
            private set { scrollRect = value; }
        }

        [ContextMenu("VALUE")]
        public void Get()
        {
            Debug.Log(ScrollRect.verticalNormalizedPosition);
        }

        [ContextMenu("VALUE 0")]
        public void Set0()
        {
            ScrollRect.verticalNormalizedPosition = 0;
        }

        [ContextMenu("VALUE 1")]
        public void Set1()
        {
            ScrollRect.verticalNormalizedPosition = 1;
        }

    }
}