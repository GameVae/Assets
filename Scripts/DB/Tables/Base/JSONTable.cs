using System.Collections.Generic;
using Generic.Singleton;
using UnityEngine;
using Json;
using System;

namespace DataTable
{
    public class JSONTable<T> : ScriptableObject, ITable
        where T : ITableData
    {
        [SerializeField] private List<T> rows;

        private Type rowType;
        private AJPHelper ajpHelper;
        private AJPHelper.Operation operation;

        public int Count
        {
            get { return Rows.Count; }
        }

        public List<T> Rows
        {
            get { return rows ?? (rows = new List<T>()); }
        }

        public Type RowType
        {
            get
            {
                return rowType ?? (rowType = typeof(T));
            }
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

        public AJPHelper.Operation Operation
        {
            get
            {
                if (operation == null)
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

        public void Clear()
        {
            Rows.Clear();
        }

        private void LoadRow(T r)
        {
            Rows.Add(r);
        }

        protected virtual void Add(T obj)
        {
            if (obj != null)
                Rows.Add(obj);
        }

        public virtual void LoadTable(JSONObject jsonObj)
        {
            Clear();

            int count = jsonObj.Count;
            for (int i = 0; i < count; i++)
            {
                T row = AJPHelper.ParseJson<T>(jsonObj[i].ToString());
                Add(row);
            }
        }

        public virtual void UpdateTable(JSONObject jsonObj)
        {
            int count = jsonObj.Count;
            if (count == 0)
            {
                T updateData = AJPHelper.ParseJson<T>(jsonObj.ToString());
                UpdateTableData(updateData);
            }
            else if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    T updateData = AJPHelper.ParseJson<T>(jsonObj[i].ToString());
                    UpdateTableData(updateData);
                }
            }
        }

        public virtual void AsyncLoadTable(JSONObject jsonObj)
        {
            Clear();

            Operation = AJPHelper.GetParser<T>().Start(new AsyncTableLoader<T>.ParseInfo()
            {
                Obj = jsonObj,
                ResultHandler = LoadRow,
                Operation = new AJPHelper.Operation(),
            });
        }

        protected virtual void UpdateTableData(T updateData)
        {
            // TODO: upgrade search
            int count = Count;
            for (int i = 0; i < count; i++)
            {
                if(updateData.CompareTo(Rows[i]) == 0)
                {
                    Rows[i] = updateData;
                    return;
                }
            }

            // not exist update data - add it to rows
            Add(updateData);
        }
    }
}

