using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCircle : MonoBehaviour
{
    public LineRenderer lineRender;

    private void Start()
    {
        int radiusPerSegment = 10;
        int count = 360 / radiusPerSegment;
        lineRender.positionCount = count + 1;

        for(int i = 0; i < count + 1; i++)
        {
            float x = Mathf.Cos((radiusPerSegment * i) * Mathf.Deg2Rad) * 3;
            float y = Mathf.Sin((radiusPerSegment * i) * Mathf.Deg2Rad) * 3;
            lineRender.SetPosition(i, new Vector3(x, 0.5f, y));
            // test add
        }
    }
}
