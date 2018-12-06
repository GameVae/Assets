using System.Collections.Generic;
using UnityEngine;

public class CellInfoManager
{
    private static readonly CellInfoManager instance = new CellInfoManager();
    // y round
    private readonly Vector3Int[] HexaPatternEven1 = new Vector3Int[]
    {
        new Vector3Int(-1, 0, 0),
        new Vector3Int(-1,-1, 0),
        new Vector3Int(-1, 1, 0),
        new Vector3Int( 0, 1, 0),
        new Vector3Int( 0,-1, 0),
        new Vector3Int( 1, 0, 0),
    };
    private readonly Vector3Int[] HexaPatternEven2 = new Vector3Int[]
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
    // y odd
    private readonly Vector3Int[] HexaPatternOdd1 = new Vector3Int[]
    {
        new Vector3Int(-1, 0, 0),
        new Vector3Int( 0,-1, 0),
        new Vector3Int( 0, 1, 0),
        new Vector3Int( 1,-1, 0),
        new Vector3Int( 1, 1, 0),
        new Vector3Int( 1, 0, 0),

    };
    private readonly Vector3Int[] HexaPatternOdd2 = new Vector3Int[]
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

    private static int Id = 0;

    public static int ID()
    {
        return Id++;
    }

    public static CellInfoManager Instance
    {
        get { return instance; }
    }

    private Dictionary<Vector3Int, CellInfomation> dict;
    public List<Vector3Int> BuildingCell { get; private set; }

    private CellInfoManager()
    {
        dict = new Dictionary<Vector3Int, CellInfomation>();
        BuildingCell = new List<Vector3Int>();
    }

    public bool AddToDict(Vector3Int key, CellInfomation value)
    {
        if (dict.ContainsKey(key))
        {
            return false;
        }
        dict[key] = value;
        return true;
    }

    public void AddBuilding(Vector3Int cellPos,CellInfomation info, bool isExpand)
    {
        Vector3Int[] pattern = cellPos.y % 2 == 0 ? HexaPatternEven1 : HexaPatternOdd1;
        for (int i = 0; i < pattern.Length; i++)
        {
            Vector3Int temp = cellPos + pattern[i];
            AddToDict(temp, info);
        }
        if (isExpand)
        {
            pattern = cellPos.y % 2 == 0 ? HexaPatternEven2 : HexaPatternOdd2;
            for (int i = 0; i < pattern.Length; i++)
            {
                Vector3Int temp = cellPos + pattern[i];
                AddToDict(temp, info);
            }
        }

        if (!BuildingCell.Contains(cellPos)) BuildingCell.Add(cellPos);
    }

    public bool RemoveDict(Vector3Int key)
    {
        if (dict.ContainsKey(key))
        {
            dict.Remove(key);
            return true;
        }
        return false;
    }

    public bool ContainsKey(Vector3Int key)
    {
        return dict.ContainsKey(key);
    }

    public bool GetCellInfo(Vector3Int key, out CellInfomation result)
    {
        return dict.TryGetValue(key, out result);
    }

    public CellInfomation GetCellInfo(Vector3Int key)
    {
        dict.TryGetValue(key, out CellInfomation result);
        return result;
    }

    public CellInfomation GetCellInfo(Transform transGameObject)
    {
        return GetCellInfo(HexMap.Instance.WorldToCell(transGameObject.position).ZToZero());
    }
}
