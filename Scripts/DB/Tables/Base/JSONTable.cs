using System.Collections.Generic;
using Generic.Singleton;
using UnityEngine;
using Json;
using System;
using Extensions.BinarySearch;
using System.Collections.ObjectModel;

namespace DataTable
{
    public class JSONTable<T> : ScriptableObject, ITable
        where T : ITableData
    {
        [SerializeField] private List<T> rows;

        private object locker;
        private Type rowType;
        private AJPHelper ajpHelper;
        private AJPHelper.Operation operation;

        protected object Locker
        {
            get
            {
                return locker ?? (locker = new object());
            }
        }

        public int Count
        {
            get { return Rows.Count; }
        }

        protected List<T> Rows
        {
            get { return rows ?? (rows = new List<T>()); }
        }

        public ReadOnlyCollection<T> ReadOnlyRows
        {
            get { return Rows.AsReadOnly(); }
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

        public void Sort()
        {
            Rows.BinarySort_R();
        }

        public virtual void LoadTable(JSONObject jsonObj)
        {
            Clear();

            int count = jsonObj.Count;
            for (int i = 0; i < count; i++)
            {
                T row = JsonUtility.FromJson<T>(jsonObj[i].ToString());
                Insert(row);
            }
        }

        public virtual void UpdateTable(JSONObject jsonObj)
        {
            int count = jsonObj.Count;
            Debugger.Log("count: " + count);
            if (count == 0)
            {
                T updateData = JsonUtility.FromJson<T>(jsonObj.ToString());
                UpdateOrAdd(updateData);
            }
            else if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    T updateData = JsonUtility.FromJson<T>(jsonObj[i].ToString());
                    UpdateOrAdd(updateData);
                }
            }
        }

        public virtual void AsyncLoadTable(JSONObject jsonObj)
        {
            Clear();

            Operation = AJPHelper.GetParser<T>().Start(new AsyncTableLoader<T>.ParseInfo()
            {
                Obj = jsonObj,
                ResultHandler = (T result) => Insert(result),
                Operation = new AJPHelper.Operation(),
            });
        }

        public virtual int Insert(T obj)
        {
            lock (Locker)
            {
                if (obj != null)
                {
                    return Rows.Insert_R(obj);
                }
            }
            return -1;
        }

        /// <summary>
        /// Find T and update or add new
        /// true: => update
        /// false: => add
        /// </summary>
        /// <param name="updateData">data for update</param>
        /// <returns></returns>
        protected virtual bool UpdateOrAdd(T updateData)
        {
            return Rows.UpdateOrInsert_R(updateData);
        }
    }
}

