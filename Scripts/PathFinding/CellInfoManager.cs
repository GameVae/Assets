using Generic.Contants;
using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class CellInfoManager : ISingleton
{
    private static readonly CellInfoManager instance = new CellInfoManager();
    // y round
    private readonly Vector3Int[] HexaPatternEven1 = GConstants.NeightbourHexCell.HexaPatternEven1;
    private readonly Vector3Int[] HexaPatternEven2 = GConstants.NeightbourHexCell.HexaPatternEven2;
    // y odd
    private readonly Vector3Int[] HexaPatternOdd1 = GConstants.NeightbourHexCell.HexaPatternOdd1;
    private readonly Vector3Int[] HexaPatternOdd2 = GConstants.NeightbourHexCell.HexaPatternOdd2;

    private static int Id = 0;

    public static int ID()
    {
        return Id++;
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
        return GetCellInfo(Singleton.Instance<HexMap>().WorldToCell(transGameObject.position).ZToZero());
    }
}
