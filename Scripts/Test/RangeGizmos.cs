using UnityEngine;

public class RangeGizmos : MonoBehaviour
{
    public Grid Map;
    public bool LogCellInfoAround;

    private bool isOdd;
    private void Start()
    {
        isOdd = (Map.WorldToCell(transform.position).y % 2) != 0;
    }
    // y round
    private readonly Vector3Int[] HexaPatternEven1 = new Vector3Int[]
    {
        new Vector3Int( 0,-1, 0),
        new Vector3Int( 0, 1, 0),
        new Vector3Int(-1,-1, 0),
        new Vector3Int(-1, 1, 0),
        new Vector3Int( 1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int( 0, 0, 0),
    };
    private readonly Vector3Int[] HexaPatternEven2 = new Vector3Int[]
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
    private readonly Vector3Int[] HexaPatternEven3 = new Vector3Int[]
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
    private readonly Vector3Int[] HexaPatternOdd1 = new Vector3Int[]
    {
        new Vector3Int( 1,-1, 0),
        new Vector3Int( 1, 1, 0),
        new Vector3Int( 0,-1, 0),
        new Vector3Int( 0, 1, 0),
        new Vector3Int( 1, 0, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int( 0, 0, 0),
    };
    private readonly Vector3Int[] HexaPatternOdd2 = new Vector3Int[]
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
    private readonly Vector3Int[] HexaPatternOdd3 = new Vector3Int[]
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
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Map == null) Map = FindObjectOfType<Grid>();

        Vector3Int centerCell = Map.WorldToCell(transform.position);
        Vector3Int tempCell = centerCell;
        isOdd = (centerCell.y % 2) != 0;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(Map.CellToWorld(tempCell), Vector3.one * 1.5f);

        // round 1
        Gizmos.color = Color.gray;
        Vector3Int[]
        pattern = !isOdd ? HexaPatternEven1 : HexaPatternOdd1;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (centerCell + pattern[i] == centerCell) continue;
            Gizmos.DrawCube(Map.CellToWorld(centerCell + pattern[i]), Vector3.one * 1.5f);
            if (LogCellInfoAround)
                Debug.Log("1 : " + (centerCell + pattern[i]));

        }
        // round 2
        Gizmos.color = Color.red;
        pattern = !isOdd ? HexaPatternEven2 : HexaPatternOdd2;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (centerCell + pattern[i] == centerCell) continue;
            Gizmos.DrawCube(Map.CellToWorld(centerCell + pattern[i]), Vector3.one * 1.5f);
            if (LogCellInfoAround)
                Debug.Log("2 : " + (centerCell + pattern[i]));

        }
        // round 3
        Gizmos.color = Color.blue;
        pattern = !isOdd ? HexaPatternEven3 : HexaPatternOdd3;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (centerCell + pattern[i] == centerCell) continue;
            Gizmos.DrawCube(Map.CellToWorld(centerCell + pattern[i]), Vector3.one * 1.5f);
            if (LogCellInfoAround)
                Debug.Log("3 : " + (centerCell + pattern[i]));

        }

        LogCellInfoAround = false;
    }
#endif

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer != 9) return;
        Vector3Int otherCellPosition = Map.WorldToCell(other.gameObject.transform.position);
        Debug.Log(GetOverlapRange(otherCellPosition));
    }

    ///-1 Not found
    /// 0 Center   
    /// 1 Range 1  
    /// 2 Range 2  
    /// 3 Range 3  
    private int GetOverlapRange(Vector3Int cellPos)
    {
        Vector3Int centerCell = Map.WorldToCell(transform.position);
        cellPos.z = 0;
        centerCell.z = 0;

        isOdd = (centerCell.y % 2) != 0;
        if (centerCell == cellPos) return 0;
        // round 1
        Vector3Int[]
        pattern = !isOdd ? HexaPatternEven1 : HexaPatternOdd1;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] == Vector3Int.zero) continue;
            if (centerCell + pattern[i] == cellPos) return 1;
        }
        // round 2
        pattern = !isOdd ? HexaPatternEven2 : HexaPatternOdd2;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] == Vector3Int.zero) continue;
            if (centerCell + pattern[i] == cellPos) return 2;
        }
        // round 3
        pattern = !isOdd ? HexaPatternEven3 : HexaPatternOdd3;
        for (int i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] == Vector3Int.zero) continue;
            if (centerCell + pattern[i] == cellPos) return 3;
        }
        return -1;
    }
}

