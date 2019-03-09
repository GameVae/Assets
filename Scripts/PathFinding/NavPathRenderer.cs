using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPathRenderer
{
    private bool isLineAnimation;
    private float animLineCounter;

    private Gradient colors;
    private GradientColorKey[] graColorKeys;
    private GradientAlphaKey[] graAlKeys;

    private LineRenderer movingLineRenderer;

    public NavPathRenderer(LineRenderer lineRenderer)
    {
        movingLineRenderer = lineRenderer;
        LineRendererInit();
    }

    public void Update()
    {
        AnimationLine();
    }

    private void LineRendererInit()
    {
        movingLineRenderer.allowOcclusionWhenDynamic = true;

        graAlKeys = new GradientAlphaKey[2];
        graColorKeys = new GradientColorKey[2];

        graColorKeys[0].color = Color.red;
        graColorKeys[0].time = 0.0f;
        graColorKeys[1].color = Color.blue;
        graColorKeys[1].time = 1.0f;

        graAlKeys[0].alpha = 100.0f;
        graAlKeys[0].time = 0.0f;
        graAlKeys[1].alpha = 100.0f;
        graAlKeys[1].time = 1.0f;

        colors = new Gradient()
        {
            mode = GradientMode.Fixed,
            alphaKeys = graAlKeys,
            colorKeys = graColorKeys
        };
    }

    private void AnimationLine()
    {
        if (isLineAnimation)
        {
            animLineCounter += Time.deltaTime;
            if (animLineCounter >= 0.25f)
            {
                isLineAnimation = false;
                animLineCounter = 0.0f;
                movingLineRenderer.positionCount = 0;
            }
        }
    }

    public void RemoveForwardPoint(int pathCount, float t)
    {
        CalculateGradiantColor(pathCount, t);
        int count = movingLineRenderer.positionCount;
        movingLineRenderer.positionCount = (count > 0) ? count - 1 : 0;
    }

    private void CalculateGradiantColor(int pathCount, float t)
    {
        int count = pathCount;
        if (count != 0)
        {
            float time = t;
            graColorKeys[0].time = time;
            graAlKeys[0].time = graColorKeys[0].time;
            if (time <= 0)
            {
                graColorKeys[0].color = Color.blue;
            }
            else
            {
                graColorKeys[0].color = Color.red;
            }
        }
        else
        {
            graColorKeys[0].time = 1.0f;
            graColorKeys[0].color = Color.red;
            graAlKeys[0].time = graColorKeys[0].time;
        }

        colors.SetKeys(graColorKeys, graAlKeys);
        movingLineRenderer.colorGradient = colors;
    }

    private void GenNotFoundPath(Vector3 position, Vector3 target)
    {
        movingLineRenderer.positionCount = 2;
        movingLineRenderer.SetPosition(0, position);
        movingLineRenderer.SetPosition(1, target);
        CalculateGradiantColor(0, 0);
        isLineAnimation = true;
    }

    public void LineRendererGenPath
        (bool foundPath,
        List<Vector3> worldPath = null, float t = 0,// use when foundPath = true
        Vector3 position = default(Vector3), Vector3 target = default(Vector3)) // use when foundPath = false
    {
        if (foundPath)
        {
            GenValidPath(worldPath);
            CalculateGradiantColor(worldPath.Count, t);
        }
        else
        {
            GenNotFoundPath(position, target);
        }
    }


    public void Clear()
    {
        movingLineRenderer.positionCount = 0;
    }

    private void GenValidPath(List<Vector3> worldPath)
    {
        if (worldPath != null)
        {
            int count = worldPath.Count;
            movingLineRenderer.positionCount = count;
            for (int i = 0; i < count; i++)
            {
                movingLineRenderer.SetPosition(count - i - 1, worldPath[count - i - 1]);
            }
        }
    }


}
