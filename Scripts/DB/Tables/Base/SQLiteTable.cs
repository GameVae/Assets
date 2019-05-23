using DataTable.SQL;
using Generic.Singleton;
using Json;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Utils;
using static SQLiteConnectFactory;

namespace DataTable
{
    public class SQLiteTable<T> : ScriptableObject, ISQLiteTable
        where T : ISQLiteData
    {
        public string TableName;
#pragma warning disable IDE0044
        [SerializeField] private SQLiteLinkType linkType;
#pragma warning restore IDE0044

        [SerializeField] private List<T> rows;
        [SerializeField] private System.Type rowType;

        private SQLiteHelper helper;
        private SQLiteManualConnection sqlConn;
        private SQLiteConnectFactory connFactory;

        public SQLiteConnectFactory ConnectFactory
        {
            get
            {
                return connFactory ?? (connFactory = Singleton.Instance<SQLiteConnectFactory>());
            }
        }
        public SQLiteManualConnection SQLiteConn
        {
            get
            {
                return sqlConn ?? (sqlConn = ConnectFactory.GetConnection(linkType));
            }
        }

        public SQLiteLinkType LinkType
        {
            get { return linkType; }
        }

        public List<T> Rows
        {
            get { return rows ?? (rows = new List<T>()); }
        }

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
            get { return Rows.Count; }
        }

        public ITableData this[int rowID]
        {
            get { return Rows[rowID]; }
            set { Rows[rowID] = (T)value; }
        }

        public void Add(T obj)
        {
            if (obj != null)
                Rows.Add(obj);
        }

        public void Clear()
        {
            Rows.Clear();
        }

        public void LoadTable()
        {
            SQLiteConn.LoadTable(this);
        }

        public void SQLInsert(T row)
        {
            string colsString = Helper.CreateColumnSequenceFrom(RowType, row);
            string valuesString = Helper.CreateValuesSequenceFrom(RowType, row);

            string cmd = SQLUtils.GetInsertCommand(TableName, colsString, valuesString);

            if (SQLiteConn.DbConnection.InsertValue(cmd))
                Rows.Add(row);
        }

        public void SQLUpdate(int rowID)
        {
            T row = Rows[rowID];
            string keyValuePairs = Helper.CreateUpdateValuesFrom(RowType, row);
            string cmd = SQLUtils.GetUpdateCommand(TableName, rowID + 1, keyValuePairs);

            SQLiteConn.DbConnection.UpdateValue(cmd);
        }

        public void SQLDelete(int rowId)
        {
            string cmd = SQLUtils.GetDeleteCommand(TableName, string.Format(" _rowid_ = {0}", rowId + 1));

            if (SQLiteConn.DbConnection.Delete(cmd))
                Rows.RemoveAt(rowId);
        }
    }
}