using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    public sealed class GUIVerticalList : CustomGUI
    {
        [SerializeField, HideInInspector]
        private Dictionary<int, RectTransform> contentDict;
        [SerializeField, HideInInspector]
        private int id;

        public override Image MaskImage
        {
            get { return maskImage ?? (maskImage = GetComponent<Image>()); }
            protected set { maskImage = value; }
        }

        public override void InteractableChange(bool value) { }

        public override void SetChildrenDependence() { }

        protected override void Awake()
        {
            if (contentDict == null)
                contentDict = new Dictionary<int, RectTransform>();
        }

        public T AddElement<T>(RectTransform prefab, out int objId) where T : Component
        {
            if (contentDict == null)
                contentDict = new Dictionary<int, RectTransform>();

            contentDict[id] = Instantiate(prefab, transform);
            objId = id++;
            return contentDict[objId].GetComponent<T>();
        }

        public RectTransform AddElement(RectTransform prefab, out int objId)
        {
            if (contentDict == null)
                contentDict = new Dictionary<int, RectTransform>();

            contentDict[id] = Instantiate(prefab, transform);
            objId = id++;
            return contentDict[objId];
        }

        public void RemoveElement(int id)
        {
            try
            {
                contentDict?.Remove(id);
            }
            catch { }
        }

        public void Clear()
        {
            contentDict?.Clear();
        }
    }
}