using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UI.Widget.GUIToggle;

namespace UI.Widget
{
    public sealed class GUITableElement : CustomGUI
    {
        [SerializeField, HideInInspector] private TextMeshProUGUI prefab;
        [SerializeField, HideInInspector] private List<TextMeshProUGUI> cells;
        [SerializeField, HideInInspector] private AxisType axisType;

        public override TextMeshProUGUI Placeholder
        {
            get { return null; }
            set { return; }
        }

        public override void InteractableChange(bool value) { }

        public override void SetChildrenDependence()
        {
            if (prefab == null)
            {
                prefab = new GameObject("Prefab").AddComponent<TextMeshProUGUI>();
                prefab.transform.SetParent(transform);
                prefab.transform.localScale = new Vector3(1, 1, 1);
                prefab.gameObject.SetActive(false);
            }

        }

        public AxisType AxisType
        {
            get { return axisType; }
            private set { axisType = value; }
        }

        public TextMeshProUGUI this[int index]
        {
            get
            {
                try { return cells[index]; }
                catch { return null; }
            }
        }

        public void AxisTypeChange(AxisType axisType)
        {
            AxisType = axisType;
            Alignment();
        }

        public void SetContents(params string[] contents)
        {
            for (int i = 0; i < cells.Count && i < contents.Length; i++)
            {
                cells[i].text = contents[i];
            }
        }

        public void Add(string contents = null)
        {
            if (cells == null)
                cells = new List<TextMeshProUGUI>();
            TextMeshProUGUI text = Instantiate(prefab, transform);
            text.text = contents;
            text.overflowMode = TextOverflowModes.Truncate;
            text.alignment = TextAlignmentOptions.Center;

            text.gameObject.SetActive(true);

            cells.Add(text);

            Alignment();
        }

#if UNITY_EDITOR
        public void EditorDeleteLastIndex()
        {
            int count = cells.Count;
            if (count > 0)
            {
                DestroyImmediate(cells[count - 1].gameObject);
                cells.RemoveAt(count - 1);
            }

            Alignment();
        }
#endif
        public void DeleteLastIndex()
        {
            int count = cells.Count;
            if (count > 0)
            {
                Destroy(cells[count - 1]);
                cells.RemoveAt(count - 1);
            }

            Alignment();
        }

        private void Alignment()
        {
            cells.RemoveNull();

            RectTransform cur = null;
            int count = cells.Count;
            float deltaPos = 1.0f / count;
            float lastMin = 0;
            for (int i = 0; i < count; i++)
            {
                cur = cells[i].transform as RectTransform;
                if (AxisType == AxisType.Horizontal)
                {
                    cur.anchorMin = new Vector2(lastMin, 0);
                    cur.anchorMax = new Vector2(lastMin + deltaPos, 1);
                }
                else
                {
                    cur.anchorMin = new Vector2(0, lastMin);
                    cur.anchorMax = new Vector2(1, lastMin + deltaPos);
                }
                cur.offsetMin = cur.offsetMax = Vector2.zero;
                lastMin += deltaPos;
            }
        }
    }
}