﻿using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace PathFinding
{
    public class BreathFirstSearch
    {
        public static BreathFirstSearch Instance
        {
            get { return instance ?? (instance = new BreathFirstSearch()); }
            private set { instance = value; }
        }

        private static BreathFirstSearch instance;
        private List<Vector3Int> open;
        private List<Vector3Int> closed;

        private BreathFirstSearch()
        {
            open = new List<Vector3Int>();
            closed = new List<Vector3Int>();
        }

        public bool GetNearestCell(Vector3Int center, out Vector3Int result)
        {
            open.Clear();
            closed.Clear();

            result = Vector3Int.one * -1;
            open.Add(center);

            Calculate(ref result);

            if (result != (Vector3Int.one * -1)) return true;

            return false;
        }

        private void Calculate(ref Vector3Int result)
        {
            // failure             // goal             
            if (open.Count <= 0 || result != (Vector3Int.one * -1)) return;

            Vector3Int currentCell = open[0];
            open.RemoveAt(0);
            closed.Add(currentCell);

            Vector3Int[] neighbours = Singleton.Instance<HexMap>().GetNeighbours(currentCell);

            for (int i = 0; i < neighbours.Length; i++)
            {
                if (!Singleton.Instance<CellInfoManager>().ContainsKey(neighbours[i]))
                {
                    result = neighbours[i];
                    return;
                }
                open.Add(neighbours[i]);
            }
            Calculate(ref result);
        }
    }
}