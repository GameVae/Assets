using System.Collections.Generic;
using UnityEngine;

public class CellInfoManager
{
    private static readonly CellInfoManager instance = new CellInfoManager();
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

    private CellInfoManager()
    {
        dict = new Dictionary<Vector3Int, CellInfomation>();
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

    public CellInfomation GetCellInfo(Vector3Int key)
    {
        dict.TryGetValue(key, out CellInfomation result);
        return result;
        //if (dict.ContainsKey(key)) return dict[key];
        //return null;
    }

    public CellInfomation GetCellInfo(Transform transGameObject)
    {
        return GetCellInfo(HexMap.Instance.WorldToCell(transGameObject.position).ZToZero());
    }
}
