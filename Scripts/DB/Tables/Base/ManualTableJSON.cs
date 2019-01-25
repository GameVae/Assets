using Json;
using Json.Interface;
using ManualTable.Interface;
using ManualTable.Row;
using System.Collections.Generic;
using UnityEngine;

namespace ManualTable
{
    public class ManualTableJSON<T> : ScriptableObject, ITable where T : JsonRow, new()
    {
        [SerializeField] public List<T> Rows;

        public IJSON this[int index]
        {
            get
            {
                if (index >= Rows.Count) return default(T);
                return Rows[index];
            }
            set
            {
                if (index < Rows.Count)
                    Rows[index] = (T)value;
            }
        }

        public int Count
        {
            get { return Rows.Count; }
        }

        public System.Type RowType
        { get { return typeof(T); } }

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
            //Debug.Log(Rows.Count);

            int count = data.Count;
            for (int i = 0; i < count; i++)
            {
                LoadRow(data[i].ToString());
            }
        }
    }
}

