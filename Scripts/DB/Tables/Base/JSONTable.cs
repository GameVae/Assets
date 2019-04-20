using System.Collections.Generic;
using Generic.Singleton;
using UnityEngine;
using Json;

namespace DataTable
{
    public class JSONTable<T> : ScriptableObject, ITable where T : ITableData
    {
        [SerializeField] private List<T> rows;
        private AJPHelper ajpHelper;
        private AJPHelper.Operation operation;

        public AJPHelper.Operation Operation
        {
            get
            {
                if(operation == null)
                {
                    operation = new AJPHelper.Operation()
                    {
                        Progress = 1,
                        IsDone = true,
                        SpentTime = 0,
                    };
                }
                return operation;
            }
            protected set { operation = value; }
        }

        public List<T> Rows
        {
            get { return rows ?? (rows = new List<T>()); }
        }
        public AJPHelper AJPHelper
        {
            get { return ajpHelper ?? (ajpHelper = Singleton.Instance<AJPHelper>()); }
        }

        public ITableData this[int index]
        {
            get
            {
                if (index >= Count) return default(T);
                return rows[index];
            }
            set
            {
                if (index < Count)
                    rows[index] = (T)value;
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
            T row = ParseJson<T>(json);
            Rows.Add(row);
        }

        public virtual void LoadTable(JSONObject data, bool clearPre = true)
        {
            if (clearPre)
                Rows.Clear();

            int count = data.Count;
            for (int i = 0; i < count; i++)
            {
                LoadRow(data[i].ToString());
            }
        }

        public virtual void AsyncLoadTable(JSONObject data, bool clearPre = true)
        {
            if (clearPre)
                Rows.Clear();

            Operation = AJPHelper.GetParser<T>().Start(new AsyncTableLoader<T>.ParseInfo()
            {
                Obj = data,
                ResultHandler = LoadRow,
                Operation = new AJPHelper.Operation(),
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
            catch (System.Exception e)
            {
                Debugger.Log(e.ToString());
                return default(TResult);
            }

        }
    }
}

