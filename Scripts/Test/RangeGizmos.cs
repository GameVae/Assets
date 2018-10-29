using UnityEngine;

public class RangeGizmos : MonoBehaviour
{
    public Grid Map;

    // y round
    private readonly Vector3Int[] HexaPattern10 = new Vector3Int[]
    {
        new Vector3Int( 0,-1, 0),   
        new Vector3Int( 0, 1, 0),   
        new Vector3Int(-1,-1, 0),   
        new Vector3Int(-1, 1, 0),   
        new Vector3Int( 1, 0, 0),   
        new Vector3Int(-1, 0, 0),         
        new Vector3Int( 0, 0, 0),            
    };
    private readonly Vector3Int[] HexaPattern11 = new Vector3Int[]
    {
        new Vector3Int( 0, 2, 0),
        new Vector3Int( 0,-2, 0),
        new Vector3Int( 2, 0, 0),
        new Vector3Int(-2, 0, 0),
        new Vector3Int( 1, 1, 0),
        new Vector3Int( 1,-2, 0),
        new Vector3Int( 1,-1, 0),
        new Vector3Int( 1, 2, 0),
        new Vector3Int(-1, 2, 0),
        new Vector3Int(-2, 1, 0),
        new Vector3Int(-2,-1, 0),
        new Vector3Int(-1,-2, 0),
    };
    private readonly Vector3Int[] HexaPattern12 = new Vector3Int[]
    {
        new Vector3Int( -3, 0, 0),
        new Vector3Int( -3,-1, 0),
        new Vector3Int( -2,-2, 0),
        new Vector3Int( -2,-3, 0),
        new Vector3Int( -1,-3, 0),
        new Vector3Int(  0,-3, 0),
        new Vector3Int(  1,-3, 0),
        new Vector3Int(  2,-2, 0),
        new Vector3Int(  2,-1, 0),
        new Vector3Int(  3, 0, 0),
        new Vector3Int(  2, 1, 0),
        new Vector3Int(  2, 2, 0),
        new Vector3Int(  1, 3, 0),
        new Vector3Int(  0, 3, 0),
        new Vector3Int( -1, 3, 0),
        new Vector3Int( -2, 3, 0),
        new Vector3Int( -2, 2, 0),
        new Vector3Int( -3, 1, 0),
    };
    // y odd
    private readonly Vector3Int[] HexaPattern20 = new Vector3Int[]
    {
        new Vector3Int( 1,-1, 0),   
        new Vector3Int( 1, 1, 0),   
        new Vector3Int( 0,-1, 0),   
        new Vector3Int( 0, 1, 0),   
        new Vector3Int( 1, 0, 0),   
        new Vector3Int(-1, 0, 0),        
        new Vector3Int( 0, 0, 0),           
    };
    private readonly Vector3Int[] HexaPattern21 = new Vector3Int[]
    {
        new Vector3Int( 0, 2, 0),
        new Vector3Int( 0,-2, 0),
        new Vector3Int(-2, 0, 0),
        new Vector3Int( 2, 0, 0),
        new Vector3Int(-1, 1, 0),
        new Vector3Int(-1, 2, 0),
        new Vector3Int( 1, 2, 0),
        new Vector3Int( 2, 1, 0),
        new Vector3Int(-1,-1, 0),
        new Vector3Int(-1,-2, 0),
        new Vector3Int( 2,-1, 0),
        new Vector3Int( 1,-2, 0),
    };
    private readonly Vector3Int[] HexaPattern22 = new Vector3Int[]
    {
        new Vector3Int( -3, 0, 0),
        new Vector3Int( -2,-1, 0),
        new Vector3Int( -2,-2, 0),
        new Vector3Int( -1,-3, 0),
        new Vector3Int(  0,-3, 0),
        new Vector3Int(  1,-3, 0),
        new Vector3Int(  2,-3, 0),
        new Vector3Int(  2,-2, 0),
        new Vector3Int(  3,-1, 0),
        new Vector3Int(  3, 0, 0),
        new Vector3Int(  3, 1, 0),
        new Vector3Int(  2, 2, 0),
        new Vector3Int(  2, 3, 0),
        new Vector3Int(  1, 3, 0),
        new Vector3Int(  0, 3, 0),
        new Vector3Int( -2, 2, 0),
        new Vector3Int( -2, 1, 0),
        new Vector3Int( -1, 3, 0),
    };

    private void OnDrawGizmos()
    {
        if (Map == null) Map = FindObjectOfType<Grid>();

        Vector3Int centerCell = Map.WorldToCell(transform.position);
        Vector3Int tempCell = centerCell;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(Map.CellToWorld(tempCell), Vector3.one * 1.5f);

        // round 1
        Gizmos.color = Color.gray;
        Vector3Int[] pattern = (centerCell.y % 2) == 0 ? HexaPattern10 : HexaPattern20;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (centerCell + pattern[i] == centerCell) continue;
            Gizmos.DrawCube(Map.CellToWorld(centerCell + pattern[i]), Vector3.one * 1.5f);
        }
        // round 2
        Gizmos.color = Color.red;
        pattern = (centerCell.y % 2) == 0 ? HexaPattern11 : HexaPattern21;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (centerCell + pattern[i] == centerCell) continue;
            Gizmos.DrawCube(Map.CellToWorld(centerCell + pattern[i]), Vector3.one * 1.5f);
        }
        // round 3
        Gizmos.color = Color.blue;
        pattern = (centerCell.y % 2) == 0 ? HexaPattern12 : HexaPattern22;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (centerCell + pattern[i] == centerCell) continue;
            Gizmos.DrawCube(Map.CellToWorld(centerCell + pattern[i]), Vector3.one * 1.5f);
        }

    }
}

