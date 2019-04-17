using System.Collections.Generic;
using Generic.Singleton;
using UnityEngine;
using Json;

namespace DataTable
{
    public class JSONTable<T> : ScriptableObject, ITable where T : ITableData
    {
        [SerializeField] private List<T> rows;
        public List<T> Rows
        {
            get { return rows ?? (rows = new List<T>()); }
        }

        public ITableData this[int index]
        {
            get
            {
                if (index >= rows.Count) return default(T);
                return rows[index];
            }
            set
            {
                if (index < rows.Count)
                    rows[index] = (T)value;
            }
        }

        public int Count
        {
            get { return rows == null ? 0 : rows.Count; }
        }

        public System.Type RowType
        { get { return typeof(T); } }

        public virtual void LoadRow(string json)
        {
            if (rows == null)
                rows = new List<T>();
            T row = ParseJson<T>(json);
            rows.Add(row);
        }

        public virtual void LoadTable(JSONObject data, bool clearPre = true)
        {
            if (rows == null)
                rows = new List<T>();
            else
            {
                if (clearPre)
                    rows.Clear();
            }

            int count = data.Count;
            for (int i = 0; i < count; i++)
            {
                LoadRow(data[i].ToString());
            }
        }

        public virtual void AsyncLoadTable(JSONObject data, bool clearPre = true)
        {
            if (rows == null)
                rows = new List<T>();
            else
            {
                if (clearPre)
                    rows.Clear();
            }
            Singleton.Instance<AJPHelper>().GetParser<T>().Start(new AsyncLoadTable<T>.ParseInfo()
            {
                Obj = data,
                ResultHandler = LoadRow,
                Operation = new AsyncLoadTable<T>.ParseOperation(),
            });
        }

        private void LoadRow(T r)
        {
            rows.Add(r);
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

