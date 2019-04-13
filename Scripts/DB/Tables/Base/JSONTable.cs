using Generic.Singleton;
using DataTable;
using System.Collections.Generic;
using UnityEngine;
using Json;

namespace DataTable
{
    public class JSONTable<T> : ScriptableObject, ITable where T : ITableData
    {
        [SerializeField] public List<T> Rows;

        public ITableData this[int index]
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

        public virtual void LoadRow(string json)
        {
            if (Rows == null)
                Rows = new List<T>();
            T row = JsonUtility.FromJson<T>(json);
            //T row = ParseJson<T>(json);
            Rows.Add(row);
        }

        public virtual void LoadTable(JSONObject data, bool clearPre = true)
        {
            if (Rows == null)
                Rows = new List<T>();
            else
            {
                if (clearPre)
                    Rows.Clear();
            }

            int count = data.Count;
            for (int i = 0; i < count; i++)
            {
                LoadRow(data[i].ToString());
            }
        }

        public virtual void AsyncLoadTable(JSONObject data, bool clearPre = true)
        {
            if (Rows == null)
                Rows = new List<T>();
            else
            {
                if (clearPre)
                    Rows.Clear();
            }
            Singleton.Instance<AJPHelper>().GetParser<T>().Start(new AsyncJsonParser<T>.ParseInfo()
            {
                Obj = data,
                ResultHandler = LoadRow,
                Operation = new AsyncJsonParser<T>.ParseOperation(),
            });
        }

        private void LoadRow(T r)
        {
            Rows.Add(r);
        }

        public static TResult ParseJson<TResult>(string json)
        {
            try
            {
                return JsonUtility.FromJson<TResult>(json);
            }
            catch(System.Exception e)
            {
                return default(TResult);
            }

        }
    }
}

