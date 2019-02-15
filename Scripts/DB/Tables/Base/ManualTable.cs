using Json;
using Json.Interface;
using ManualTable.Interface;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Utils;

namespace ManualTable
{
    public class ManualTableBase<T> : ScriptableObject, ITable where T : IJSON, IManualRow, new()
    {
        [SerializeField] public string TableName;
        [SerializeField] public List<string> Columns;
        [SerializeField] public List<T> Rows;

        public System.Type RowType
        { get { return typeof(T); } }

        public int Count
        {
            get { return (int)Rows?.Count; }
        }

        public IJSON this[int rowID]
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

        public void LoadColumn(string column)
        {
            if (!Columns.Contains(column))
                Columns.Add(column);
        }

        public int FieldCount
        {
            get { return Columns != null ? Columns.Count : 0; }
        }

        public void Clear()
        {
            if (Rows != null)
                Rows.Clear();
            if (Columns != null)
                Columns.Clear();
        }

        public void SQLInsert(IDbConnection dbConnection, T row)
        {
            string[] cols = Columns.ToArray();
            string colsString = SQLUtils.GetSequenceString(",", cols);
            string valuesString = row.ValuesSequence;

            string cmd = SQLUtils.GetInsertCommand(TableName, colsString, valuesString);
            if (dbConnection.InsertValue(cmd))
                Rows.Add(row);
        }

        public void SQLUpdate(IDbConnection dbConnection, int rowID)
        {
            T row = Rows[rowID];
            string keyValuePairs = row.KeyValuePairs;
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