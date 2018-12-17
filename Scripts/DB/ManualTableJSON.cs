using ManualTable.Interface;
using ManualTable.Row;
using System.Collections.Generic;
using UnityEngine;

public class ManualTableJSON<T> : ScriptableObject, ITable where T : IManualRow,new()
{
    [SerializeField] public List<T> rows;

    public T this[int index]
    {
        get
        {
            if (index >= rows.Count) return default(T);
            return rows[index];
        }
    }

    public System.Type RowType()
    { return typeof(T); }

    public void LoadRow(string json)
    {
        T row = JsonUtility.FromJson<T>(json);
        rows.Add(row);
    }

    public void LoadTable(JSONObject data)
    {
        if (rows == null)
            rows = new List<T>();
        else rows.Clear();

        int count = data.Count;
        for (int i = 0; i < count; i++)
        {
            LoadRow(data[i].ToString());
        }
    }
}


[CreateAssetMenu(fileName = "New RSS Table", menuName = "SQLiteTable/JSONTable", order = 4)]
public class RSS_PositionJSONTable : ManualTableJSON<RSS_PositionRow> { }