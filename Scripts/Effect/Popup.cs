﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public sealed class Popup : MonoBehaviour
{
    public static Popup Instance { get; private set; }

    private EventSystem events;

    public CursorPos Cursor;
    public GameObject Panel;
    public Text QualityText;
    public Text CoordText;
    public Text GeneralText;

    private bool enable;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        Panel.gameObject.SetActive(false);
        events = FindObjectOfType<EventSystem>();
    }

    public void Open(string generalInfo, string quality, string coord)
    {
        if (!enable) return;
        GeneralText.text = generalInfo;
        QualityText.text = quality;
        CoordText.text = coord;
        Panel.gameObject.SetActive(true);
    }

    public void OnOffSwitch()
    {
        enable = !enable;
    }

    public void SetCursor(Vector3Int cell)
    {
        Cursor.updateCursor(HexMap.Instance.CellToWorld(cell));
        Cursor.PositionCursor.SetPosTxt(cell.x.ToString(), cell.y.ToString());
    }
}