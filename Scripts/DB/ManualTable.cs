using ManualTable.Interface;
using ManualTable.Row;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Utils;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New Container", menuName = "SQLiteTable/Container", order = 1)]
    public class TableContainer : ScriptableObject
    {
        [SerializeField] public ScriptableObject Table;
        [SerializeField] public RowType RowType;
    }

    public class ManualTableBase<T> : ScriptableObject, ITable where T : IManualRow
    {
        [SerializeField] public string TableName;
        [SerializeField] public string Version;
        [SerializeField] public List<string> Columns;
        [SerializeField] public List<T> rows;

        public void LoadRow(T newRow)
        {
            if (rows == null)
                rows = new List<T>();
            rows.Add(newRow);
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

        public T this[int rowID]
        {
            get { return rows[rowID]; }
            set { rows[rowID] = value; }
        }

        public void Clear()
        {
            if (rows != null)
                rows.Clear();
            if (Columns != null)
                Columns.Clear();
        }

        public void SQLInsert(IDbConnection dbConnection, T row)
        {
            string[] cols = Columns.ToArray();
            string colsString = SQLUtils.GetSequenceString(",", cols);
            string valuesString = row.ValuesSequence + "," + Version;

            string cmd = SQLUtils.GetInsertCommand(TableName, colsString, valuesString);
            Debug.Log(cmd);
            if (dbConnection.InsertValue(cmd))
                rows.Add(row);
        }

        public void SQLUpdate(IDbConnection dbConnection, int index)
        {
            T row = rows[index];
            string keyValuePairs = row.KeyValuePairs;
            string cmd = SQLUtils.GetUpdateCommand(TableName, index + 1, keyValuePairs);
            Debug.Log(cmd);
            dbConnection.UpdateValue(cmd);
        }

        public void SQLDelete(IDbConnection dbConnection, int rowId)
        {
            string cmd = SQLUtils.GetDeleteCommand(TableName, string.Format(" _rowid_ = {0}", rowId + 1));
            if (dbConnection.Delete(cmd))
                rows.RemoveAt(rowId);
        }

        //public bool SQLAddColumn(IDbConnection dbConnection, string colName, string dataType, string defaultValue, bool notNull = false)
        //{
        //    string cmd = SQLUtils.GetGenerateColumnCommand(TableName, colName, dataType, defaultValue, notNull);
        //    return dbConnection.AddColumn(cmd);
        //}
    }

    [CreateAssetMenu(fileName = "New MainBase Table", menuName = "SQLiteTable/MainBase", order = 2)]
    public class MainBaseTable : ManualTableBase<MainBaseRow> { }
    
}