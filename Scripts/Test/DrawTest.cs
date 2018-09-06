using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawTest : MonoBehaviour
{

    

    private void OnDrawGizmos()
    {
        //Gizmos.DrawCube(Vector3.zero, new Vector3(10, 10, 10));

        //Gizmos.color = Color.red;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < 512; i++)
        {
            Gizmos.DrawLine(new Vector3(0, 0, i), new Vector3(512, 0, i));
        }
        for (int i = 0; i < 512; i++)
        {
            Gizmos.DrawLine(new Vector3(i, 0, 0), new Vector3(i, 0, 512));
        }
    }
}

   
  
