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
    }
}