using ManualTable.Interface;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Utils;

namespace ManualTable
{
    public class ManualTableBase<T> : ScriptableObject, ITable where T : IManualRow
    {
        [SerializeField] public string TableName;
        [SerializeField] public List<string> Columns;
        [SerializeField] public List<T> Rows;


        public void LoadRow(T newRow)
        {
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

        public T this[int rowID]
        {
            get { return Rows[rowID]; }
            set { Rows[rowID] = value; }
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

    //[CreateAssetMenu(fileName = "New MainBase Table", menuName = "SQLiteTable/MainBase", order = 2)]
    //public class MainBaseTable : ManualTableBase<MainBaseRow> { }

    //[CreateAssetMenu(fileName = "New Version Table", menuName = "SQLiteTable/Version", order = 3)]
    //public class VersionTable : ManualTableBase<VersionRow> { }

}