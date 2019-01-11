using Json;
using Json.Interface;
using ManualTable.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace ManualTable
{
    public class ManualTableJSON<T> : ScriptableObject, ITable where T : IJSON, IManualRow, new()
    {
        [SerializeField] public List<T> Rows;

        public T this[int index]
        {
            get
            {
                if (index >= Rows.Count) return default(T);
                return Rows[index];
            }
        }

        public System.Type RowType()
        { return typeof(T); }

        public void LoadRow(string json)
        {
            if (Rows == null)
                Rows = new List<T>();
            T row = JSONBase.FromJSON<T>(json);
            Rows.Add(row);
        }

        public void LoadTable(JSONObject data)
        {
            if (Rows == null)
                Rows = new List<T>();
            else Rows.Clear();

            int count = data.Count;
            for (int i = 0; i < count; i++)
            {
                LoadRow(data[i].ToString());
            }
        }
    }
}

