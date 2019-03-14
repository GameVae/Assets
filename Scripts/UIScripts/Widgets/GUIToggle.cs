using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Widget
{
    public delegate void CheckMarkCallback(GUICheckMark mark);

    public class GUIToggle : CustomGUI
    {
        public enum AxisType
        {
            Vertical,
            Horizontal,
        }

        public event CheckMarkCallback CheckMarkEvents;
        [SerializeField, HideInInspector] private List<GUICheckMark> checkMarks;
        [SerializeField, HideInInspector] private AxisType type;

        public AxisType Type
        {
            get { return type; }
            protected set { type = value; }
        }

        public int ActiveIndex
        {
            get { return checkMarks.IndexOf(ActiveMark); }
        }

        public GUICheckMark ActiveMark
        {
            get; set;
        }

        public override TextMeshProUGUI Placeholder
        {
            get { return (placeholder = null); }
            set { return; }
        }
    
        protected void Awake()
        {
            SetupGroup();
        }

        private void ReCalculateAnchor()
        {
            RectTransform trans = null;
            float count = (float)checkMarks.Count;
            float dist = 1.0f / count;
            float lastAnchor = 0;

            for (int i = 0; i < count; i++)
            {
                trans = checkMarks[i].transform as RectTransform;
                trans.anchorMin = (type == AxisType.Vertical) ? new Vector2(0, lastAnchor) : new Vector2(lastAnchor, 0);
                trans.anchorMax = (type == AxisType.Vertical) ? new Vector2(1, lastAnchor + dist) : new Vector2(lastAnchor + dist, 1);
                trans.offsetMax = trans.offsetMin = Vector2.zero;
                lastAnchor += dist;
            }
        }

        private void SetupGroup()
        {
            for (int i = 0; i < checkMarks.Count; i++)
            {
                checkMarks[i].SetGroup(this);
                checkMarks[i].OnOffSwitch.On +=
                    delegate { CheckActionCallback(); };
            }
        }

        public void Add(GUICheckMark mark)
        {
            if (checkMarks == null)
                checkMarks = new List<GUICheckMark>();
            checkMarks.Add(mark);
            Refresh();
        }

        public void Refresh()
        {
            for (int i = 0; i < checkMarks?.Count; i++)
            {
                if (checkMarks[i] == null)
                {
                    checkMarks.RemoveAt(i);
                    i--;
                }
            }
            ReCalculateAnchor();
        }

        public void CheckActionCallback()
        {
            CheckMarkEvents?.Invoke(ActiveMark);
        }

        public void DestroyLastIndex()
        {
            if (checkMarks != null && checkMarks.Count > 0)
            {
                DestroyImmediate(checkMarks[checkMarks.Count - 1].gameObject);
                checkMarks.RemoveAt(checkMarks.Count - 1);
            }
        }

        public void TypeChange(AxisType argType)
        {
            Type = argType;
            Refresh();
        }

        public void ActiveToggle(int index)
        {
            checkMarks[index].OnOffSwitch.SwitchOn();
        }

        public override void InteractableChange(bool value)
        {
            Interactable = value;
            for (int i = 0; i < checkMarks?.Count; i++)
            {
                checkMarks[i].InteractableChange(value);
            }
        }

        public override void SetChildrenDependence() { }
    }
}