using DataTable;
using DataTable.Row;
using DataTable.SQL;
using System.Linq;
using UnityEngine;

namespace DataTable.Loader
{
    public sealed class ManualTableLoader : MonoBehaviour
    {
        private VersionRow versionTask;

        public string ClientVersion;
        public string ServerVersion;
        public SQLiteManualConnection SQLDataConnection;
        public SQLiteManualConnection SQLVersionConnection;
        public SQLiteTable_Version Version;
        public TableContainer[] Containers;

        private void Awake()
        {
            GetCurrentVersion();
        }

        private void OnApplicationQuit()
        {
            SQLDataConnection.Dispose();
            SQLVersionConnection.Dispose();

            System.GC.Collect(2, System.GCCollectionMode.Forced);
            System.GC.WaitForPendingFinalizers();
        }

        private void LoadTables()
        {
            for (int i = 0; i < Containers.Length; i++)
            {
                Load(Containers[i].RowType, Containers[i].Table);
            }
        }

        private bool IsUpdateVersion(ref VersionRow versionTask)
        {           
            bool result = ClientVersion == null ? true : ClientVersion.CompareTo(ServerVersion) != 0;
            return result;
        }

        public void Load(DBRowType ManualRowType, ScriptableObject TableData)
        {
            switch (ManualRowType)
            {
                case DBRowType.MainBase:
                    SQLDataConnection.LoadTable(Cast<SQLiteTable_MainBase>(TableData));
                    break;
                case DBRowType.Military:
                    SQLDataConnection.LoadTable(Cast<SQLiteTable_Military>(TableData));
                    break;
                case DBRowType.Version:
                    SQLVersionConnection.LoadTable(Cast<SQLiteTable_Version>(TableData));
                    break;
                case DBRowType.TrainningCost:
                    SQLDataConnection.LoadTable(Cast<SQLiteTable_TrainningCost>(TableData));
                    break;
            }
        }

        public T Cast<T>(ScriptableObject data) where T : ScriptableObject, ITable
        {
            return (T)data;
        }

        public bool CheckVersion()
        {
            return IsUpdateVersion(versionTask: ref versionTask);
        }

        public void ReloadData()
        {
            if (versionTask != null)
            {
                versionTask.Content = ServerVersion;
                Version.SQLUpdate(SQLVersionConnection.DbConnection, Version.Rows.IndexOf(versionTask));
            }
            else
            {
                versionTask = new VersionRow()
                {
                    Id = 1,
                    Task = "Version",
                    Content = ServerVersion,
                    Comment = "None"
                };
                Version.SQLInsert(SQLVersionConnection.DbConnection, versionTask);
            }
            LoadTables();
        }


        private void GetCurrentVersion()
        {
            if (versionTask == null)
                Load(DBRowType.Version, Version);

            versionTask = Version.Rows.FirstOrDefault(x => x.Task.CompareTo("Version") == 0);
            ClientVersion = versionTask?.Content;
        }
    }
}