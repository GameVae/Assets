﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CreateRefabs : MonoBehaviour
{
    public Transform CubeRefabs;

    public int gridWidth = 11;
    public int gridHeight = 11;

    float hexWidth = 1.732f;
    float hexHeight = 2.0f;
    public float gap = 0.0f;

    Vector3 startPos;

    [Space]
    public Button CreateMapBtn;
    public Button ClearMapBtn;

    private void Awake()
    {
        CreateMapBtn.onClick.AddListener(() => createMap());
        ClearMapBtn.onClick.AddListener(() => clearMap());
    }

    private void createMap()
    {
        int childCount = transform.childCount;
        if (childCount < gridWidth * gridHeight)
        {
            AddGap();
            CalcStartPos();
            CreateGrid();
        }
    }

    private void clearMap()
    {
        int childCount = transform.childCount;

        
        for (int i = 0; i < childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

    }

    void Start()
    {




    }

    void AddGap()
    {
        hexWidth += hexWidth * gap;
        hexHeight += hexHeight * gap;
    }

    void CalcStartPos()
    {
        float offset = 0;
        if (gridHeight / 2 % 2 != 0)
            offset = hexWidth / 2;

        float x = -hexWidth * (gridWidth / 2) - offset;
        float z = hexHeight * 0.75f * (gridHeight / 2);

        startPos = new Vector3(x, 0, z);
    }

    Vector3 CalcWorldPos(Vector2 gridPos)
    {
        float offset = 0;
        if (gridPos.y % 2 != 0)
            offset = hexWidth / 2;

        float x = startPos.x + gridPos.x * hexWidth + offset;
        float z = startPos.z - gridPos.y * hexHeight * 0.75f;

        return new Vector3(x, 0, z);
    }

    void CreateGrid()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                //Transform hex = Instantiate(hexPrefab) as Transform;
                Transform hex = Instantiate(CubeRefabs) as Transform;
                Vector2 gridPos = new Vector2(x, y);
                hex.position = CalcWorldPos(gridPos);
                hex.parent = this.transform;
                hex.name = "Hexagon" + x + "|" + y;
            }
        }
    }
}


