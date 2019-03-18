using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class GUIHorizontalGrid : CustomGUI
    {
        [SerializeField, HideInInspector] private float sizePercent;
        [SerializeField, HideInInspector] private HorizontalLayoutGroup layoutGroup;

        public HorizontalLayoutGroup LayoutGroup
        {
            get { return layoutGroup ?? (layoutGroup = GetComponent<HorizontalLayoutGroup>()); }
            protected set { layoutGroup = value; }
        }
        /// <summary>
        /// Range from 0 % to 100 %
        /// </summary>
        public float ElementSize
        {
            get { return sizePercent; }
            set { sizePercent = Mathf.Clamp(value, 0, 1); }
        }

        [ContextMenu("calculate (size percent)")]
        public void CalculateElementSize()
        {
            RectTransform rect = transform as RectTransform;
            RectTransform parent = transform.parent as RectTransform;
            RectTransform[] childrent = GetComponentsInChildren<RectTransform>().IgnoreInstanceComponent(transform as RectTransform);

            float parentWidth = parent.Size().x;
            float size = parentWidth * ElementSize;
            int count = childrent.Length;
            for (int i = 0; i < count; i++)
            {
                childrent[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size);
                childrent[i].SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
            }

            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parent.Size().x);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size);
        }

        public void Add(List<RectTransform> rects)
        {
            rects.RemoveNull();
            for (int i = 0; i < rects.Count; i++)
            {
                rects[i].SetParent(transform);
                rects[i].localScale = Vector3.one;
                Vector3 pos = rects[i].localPosition;
                pos.z = 0;
                rects[i].localPosition = pos;
            }
            CalculateElementSize();
        }

        public void Add(RectTransform rect)
        {
            rect.SetParent(transform);
            rect.localScale = Vector3.one;
            CalculateElementSize();
        }

        public void SizeChange(float size)
        {
            ElementSize = size;
            CalculateElementSize();
        }
    }
}