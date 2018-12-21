using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public delegate void CheckMarkCallback(GUICheckMark mark);
public class GUIToggle : CustomGUI
{
    public enum ToggleType
    {
        Vertical,
        Horizontal,
    }

    [SerializeField] [HideInInspector]
    private List<GUICheckMark> checkMarks;

    public GUICheckMark ActiveMark { get; set; }
    public ToggleType Type;
    public event CheckMarkCallback CheckMarkEvents;

    private void Awake()
    {
        for (int i = 0; i < checkMarks.Count; i++)
        {
            checkMarks[i].SetGroup(this);
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
        for (int i = 0; i < checkMarks.Count; i++)
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
        CheckMarkEvents(ActiveMark);
    }

    public void DestroyLastIndex()
    {
        DestroyImmediate(checkMarks[checkMarks.Count - 1].gameObject);
        checkMarks.RemoveAt(checkMarks.Count - 1);
    }

    public override void InteractableChange(bool value)
    {
        Interactable = value;
        for (int i = 0; i < checkMarks.Count; i++)
        {
            checkMarks[i].InteractableChange(value);
        }
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
            trans.anchorMin = (Type == ToggleType.Vertical) ? new Vector2(0, lastAnchor) : new Vector2(lastAnchor, 0);
            trans.anchorMax = (Type == ToggleType.Vertical) ? new Vector2(1, lastAnchor + dist) : new Vector2(lastAnchor + dist, 1);
            trans.offsetMax = trans.offsetMin = Vector2.zero;
            lastAnchor += dist;
        }
    }

}
