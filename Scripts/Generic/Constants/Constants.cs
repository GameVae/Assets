using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

namespace Generic.Contants
{

    public sealed class Constants : MonoSingle<Constants>
    {
        /// <summary>
        /// Length base on cell
        /// </summary>
        public const int TOTAL_COL = 522;
        /// <summary>
        /// Width base on cell
        /// </summary>
        public const int TOTAL_ROW = 522;
        /// <summary>
        /// 0.0001f
        /// </summary>
        public const float TINY_VALUE = 0.0001f;
        /// <summary>
        /// Vector3Int.One * - 1
        /// </summary>
        public static readonly Vector3Int InvalidPosition = new Vector3Int(-1, -1, -1);
        /// <summary>
        /// Vector3Int (5,5,0)
        /// </summary>
        public static readonly Vector3Int ToClientPosition = new Vector3Int(5, 5, 0);
        /// <summary>
        /// Vector3Int (-5,-5,0)
        /// </summary>
        public static readonly Vector3Int ToSerPosition = new Vector3Int(-5, -5, 0);


        // 1 / DPI
        public float PixelDependencyDevice { get; private set; }

        public float ScreenRatio { get; private set; }
        // neighbour cell patterns
        public static class NeighbourHexCell
        {
            #region// y round
            public static readonly Vector3Int[] HexaPatternEven1 = new Vector3Int[]
            {
                new Vector3Int(-1, 0, 0),
                new Vector3Int(-1,-1, 0),
                new Vector3Int(-1, 1, 0),
                new Vector3Int( 0, 1, 0),
                new Vector3Int( 0,-1, 0),
                new Vector3Int( 1, 0, 0),
            };
            public static readonly Vector3Int[] HexaPatternEven2 = new Vector3Int[]
            {
                new Vector3Int(-2, 0, 0),
                new Vector3Int(-2,-1, 0),
                new Vector3Int(-2, 1, 0),
                new Vector3Int(-1,-2, 0),
                new Vector3Int(-1, 2, 0),
                new Vector3Int( 0,-2, 0),
                new Vector3Int( 0, 2, 0),
                new Vector3Int( 1, 2, 0),
                new Vector3Int( 1,-2, 0),
                new Vector3Int( 1,-1, 0),
                new Vector3Int( 1, 1, 0),
                new Vector3Int( 2, 0, 0),
            };
            public static readonly Vector3Int[] HexaPatternEven3 = new Vector3Int[]
            {
                new Vector3Int( -3, 0, 0),
                new Vector3Int( -3, 1, 0),
                new Vector3Int( -3,-1, 0),
                new Vector3Int( -2, 2, 0),
                new Vector3Int( -2,-2, 0),
                new Vector3Int( -2, 3, 0),
                new Vector3Int( -2,-3, 0),
                new Vector3Int( -1, 3, 0),
                new Vector3Int( -1,-3, 0),
                new Vector3Int(  0, 3, 0),
                new Vector3Int(  0,-3, 0),
                new Vector3Int(  1, 3, 0),
                new Vector3Int(  1,-3, 0),
                new Vector3Int(  2, 2, 0),
                new Vector3Int(  2,-2, 0),
                new Vector3Int(  2, 1, 0),
                new Vector3Int(  2,-1, 0),
                new Vector3Int(  3, 0, 0),
            };
            #endregion

            #region // y odd
            public static readonly Vector3Int[] HexaPatternOdd1 = new Vector3Int[]
            {
                new Vector3Int(-1, 0, 0),
                new Vector3Int( 0,-1, 0),
                new Vector3Int( 0, 1, 0),
                new Vector3Int( 1,-1, 0),
                new Vector3Int( 1, 1, 0),
                new Vector3Int( 1, 0, 0),

            };
            public static readonly Vector3Int[] HexaPatternOdd2 = new Vector3Int[]
            {
                new Vector3Int(-2, 0, 0),
                new Vector3Int(-1,-1, 0),
                new Vector3Int(-1, 1, 0),
                new Vector3Int(-1,-2, 0),
                new Vector3Int(-1, 2, 0),
                new Vector3Int( 0,-2, 0),
                new Vector3Int( 0, 2, 0),
                new Vector3Int( 1,-2, 0),
                new Vector3Int( 1, 2, 0),
                new Vector3Int( 2,-1, 0),
                new Vector3Int( 2, 1, 0),
                new Vector3Int( 2, 0, 0),
            };
            public static readonly Vector3Int[] HexaPatternOdd3 = new Vector3Int[]
            {
                new Vector3Int( -3, 0, 0),
                new Vector3Int( -2, 1, 0),
                new Vector3Int( -2,-1, 0),
                new Vector3Int( -2, 2, 0),
                new Vector3Int( -2,-2, 0),
                new Vector3Int( -1, 3, 0),
                new Vector3Int( -1,-3, 0),
                new Vector3Int(  0, 3, 0),
                new Vector3Int(  0,-3, 0),
                new Vector3Int(  1, 3, 0),
                new Vector3Int(  1,-3, 0),
                new Vector3Int(  2, 3, 0),
                new Vector3Int(  2,-3, 0),
                new Vector3Int(  2, 2, 0),
                new Vector3Int(  2,-2, 0),
                new Vector3Int(  3, 1, 0),
                new Vector3Int(  3,-1, 0),
                new Vector3Int(  3, 0, 0),
            };
            #endregion
        }

        #region Util methods
        public static Vector3Int[] GetNeighboursRange(Vector3Int center, int range)
        {
            Vector3Int[] result = (center.y % 2 == 0) ? GetEvenRange(center, range) : GetOddRange(center, range);
            return result;
        }

        public static bool IsValidCell(int x, int y)
        {
            return x >= 5 && x <= TOTAL_COL - 5 && y >= 5 && y <= TOTAL_ROW - 5;
        }

        #region Private Methods
        private static Vector3Int[] GetEvenRange(Vector3Int cell, int range)
        {
            Vector3Int[] pattern = null;
            switch (range)
            {
                case 1:
                    pattern = NeighbourHexCell.HexaPatternEven1;
                    break;
                case 2:
                    pattern = NeighbourHexCell.HexaPatternEven2;
                    break;
                case 3:
                    pattern = NeighbourHexCell.HexaPatternEven3;
                    break;
            }

            List<Vector3Int> neighbours = new List<Vector3Int>();
            Vector3Int neighbour;
            for (int i = 0; i < pattern.Length; i++)
            {
                neighbour = pattern[i] + cell;
                if (IsValidCell(neighbour.x, neighbour.y))
                {
                    neighbours.Add(neighbour);
                }
            }
            return neighbours.ToArray();
        }

        private static Vector3Int[] GetOddRange(Vector3Int cell, int range)
        {
            Vector3Int[] pattern = null;
            switch (range)
            {
                case 1:
                    pattern = NeighbourHexCell.HexaPatternOdd1;
                    break;
                case 2:
                    pattern = NeighbourHexCell.HexaPatternOdd2;
                    break;
                case 3:
                    pattern = NeighbourHexCell.HexaPatternOdd3;
                    break;
            }

            List<Vector3Int> neighbours = new List<Vector3Int>();
            Vector3Int neighbour;
            for (int i = 0; i < pattern.Length; i++)
            {
                neighbour = pattern[i] + cell;
                if (IsValidCell(neighbour.x, neighbour.y))
                {
                    neighbours.Add(neighbour);
                }
            }
            return neighbours.ToArray();
        }
        #endregion

        #endregion

        #region Mono method
        protected override void Awake()
        {
            base.Awake();
            PixelDependencyDevice = 1.0f / Screen.dpi;
            ScreenRatio = Screen.width * 1.0f / Screen.height;
        }

        private void Start()
        {
            Debugger.Log("DPI: " + Screen.dpi + " - Dependency Device: " + PixelDependencyDevice + " ratio: " + ScreenRatio);
        }
        #endregion
    }
}