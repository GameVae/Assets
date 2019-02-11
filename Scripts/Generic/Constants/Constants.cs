using UnityEngine;

namespace Generic.Contants
{
    public static class GConstants
    {
        public static class NeightbourHexCell
        {
            // y round
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
            // y odd
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
        }

    }
}