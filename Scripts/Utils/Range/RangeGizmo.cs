using Generic.Contants;
using Generic.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeGizmo : MonoBehaviour
{
    private HexMap mapIns;
    protected HexMap MapIns
    {
        get
        {
            return mapIns ?? (mapIns = Singleton.Instance<HexMap>());
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            Vector3Int[] range = Constants.GetNeighboursRange(MapIns.WorldToCell(transform.position), 1);

            Gizmos.color = Color.red;
            for (int i = 0; i < range.Length; i++)
            {
                Gizmos.DrawSphere(MapIns.CellToWorld(range[i]), 1);
            }

            // 2
            range = Constants.GetNeighboursRange(MapIns.WorldToCell(transform.position), 2);

            Gizmos.color = Color.green;
            for (int i = 0; i < range.Length; i++)
            {
                Gizmos.DrawSphere(MapIns.CellToWorld(range[i]), 1);
            }

            // 3
            range = Constants.GetNeighboursRange(MapIns.WorldToCell(transform.position), 3);

            Gizmos.color = Color.blue;
            for (int i = 0; i < range.Length; i++)
            {
                Gizmos.DrawSphere(MapIns.CellToWorld(range[i]), 1);
            }
        }
    }
}
