using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    public sealed class GUIVerticalGird : CustomGUI
    {
        [SerializeField, HideInInspector]
        private Dictionary<string, RectTransform> contentDict;
        public float MaxWidth;


        public void Remove(string name)
        {
            try
            {
                contentDict.TryGetValue(name, out RectTransform value);
                if (value != null)
                {
                    contentDict?.Remove(name);
                    Destroy(value.gameObject);
                }
            }
            catch { }
        }

        public void Clear()
        {
            foreach (KeyValuePair<string, RectTransform> item in contentDict)
            {
                Destroy(contentDict[item.Key]?.gameObject);
            }
            contentDict?.Clear();
        }

        public void Add(string name, RectTransform rect)
        {
            if (contentDict == null)
                contentDict = new Dictionary<string, RectTransform>();

            rect.SetParent(transform);
            rect.localPosition = Vector3.zero;
            rect.localScale = Vector3.one;
            rect.gameObject.SetActive(true);

            Vector2 size = rect.Size();
            if (size.x > MaxWidth)
            {
                rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MaxWidth);
            }

            contentDict.TryGetValue(name, out RectTransform value);
            if (value != null)
            {
                Destroy(value.gameObject);
            }
            contentDict[name] = rect;
        }

        public bool Containts(string key)
        {
            if (contentDict != null)
                return contentDict.ContainsKey(key);
            return false;
        }
    }
}