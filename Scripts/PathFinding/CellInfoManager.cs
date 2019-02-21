using Generic.Contants;
using Generic.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class CellInfoManager : ISingleton
{
    private static int Id = 0;

    public static int ID()
    {
        return Id++;
    }

    private Dictionary<Vector3Int, CellInfo> dict;
    public List<Vector3Int> BaseCell { get; private set; }

    private CellInfoManager()
    {
        dict = new Dictionary<Vector3Int, CellInfo>();
        BaseCell = new List<Vector3Int>();
    }

    public bool AddToDict(Vector3Int key, CellInfo value)
    {
        if (dict.ContainsKey(key))
        {
            return false;
        }
        dict[key] = value;
        return true;
    }

    public void AddBase(Vector3Int cellPos,CellInfo info, bool isExpand)
    {
        //Vector3Int[] pattern = cellPos.y % 2 == 0 ? HexaPatternEven1 : HexaPatternOdd1;
        Vector3Int[] pattern = Constants.GetNeighboursRange(cellPos, 1);
        for (int i = 0; i < pattern.Length; i++)
        {
            Vector3Int temp = cellPos + pattern[i];
            AddToDict(temp, info);
        }
        if (isExpand)
        {
            //pattern = cellPos.y % 2 == 0 ? HexaPatternEven2 : HexaPatternOdd2;
            pattern = Constants.GetNeighboursRange(cellPos, 2);

            for (int i = 0; i < pattern.Length; i++)
            {
                Vector3Int temp = cellPos + pattern[i];
                AddToDict(temp, info);
            }
        }

        if (!BaseCell.Contains(cellPos)) BaseCell.Add(cellPos);
        //Debug.Log(info.GameObject.name + " - " + cellPos);
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

    public bool GetCellInfo(Vector3Int key, out CellInfo result)
    {
        return dict.TryGetValue(key, out result);
    }

    public CellInfo GetCellInfo(Vector3Int key)
    {
        dict.TryGetValue(key, out CellInfo result);
        return result;
    }

    #region not use
    public CellInfo GetCellInfo(Transform transGameObject)
    {
        return GetCellInfo(Singleton.Instance<HexMap>().WorldToCell(transGameObject.position).ZToZero());
    }
    #endregion
}
