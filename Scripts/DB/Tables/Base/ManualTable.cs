using Generic.Singleton;

using ManualTable.Interface;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Utils;

namespace ManualTable
{
    public class ManualTableBase<T> : ScriptableObject, ITable where T : IManualRow, new()
    {
        public string TableName;
        public List<T> Rows;

        [SerializeField] private System.Type rowType;

        private SQLiteHelper helper;
        protected SQLiteHelper Helper
        {
            get { return helper ?? (helper = Singleton.Instance<SQLiteHelper>()); }
        }

        public System.Type RowType
        {
            get
            {
                return rowType ?? (rowType = typeof(T));
            }
        }

        public int Count
        {
            get { return (int)Rows?.Count; }
        }

        public IManualRow this[int rowID]
        {
            get { return Rows[rowID]; }
            set { Rows[rowID] = (T)value; }
        }

        public void LoadRow(string json)
        {
            T newRow = JsonUtility.FromJson<T>(json);
            if (Rows == null)
                Rows = new List<T>();
            Rows.Add(newRow);
        }

        public void Clear()
        {
            if (Rows != null)
                Rows.Clear();
        }

        public void SQLInsert(IDbConnection dbConnection, T row)
        {
            string colsString   = Helper.CreateColumnSequenceFrom(RowType,row);
            string valuesString = Helper.CreateValuesSequenceFrom(RowType, row);

            string cmd = SQLUtils.GetInsertCommand(TableName, colsString, valuesString);
            if (dbConnection.InsertValue(cmd))
                Rows.Add(row);
        }

        public void SQLUpdate(IDbConnection dbConnection, int rowID)
        {
            T row = Rows[rowID];
            string keyValuePairs = Helper.CreateUpdateValuesFrom(RowType, row);
            string cmd = SQLUtils.GetUpdateCommand(TableName, rowID + 1, keyValuePairs);
            dbConnection.UpdateValue(cmd);
        }

        public void SQLDelete(IDbConnection dbConnection, int rowId)
        {
            string cmd = SQLUtils.GetDeleteCommand(TableName, string.Format(" _rowid_ = {0}", rowId + 1));
            if (dbConnection.Delete(cmd))
                Rows.RemoveAt(rowId);
        }
    }
}