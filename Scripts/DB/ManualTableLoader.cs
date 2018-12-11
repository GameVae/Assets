using ManualTable.Interface;
using ManualTable.Row;
using ManualTable.SQL;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ManualTable.Loader
{
    public sealed class ManualTableLoader : MonoBehaviour
    {
        private VersionRow versionTask;

        public string CurrentVersion;
        public string ServerVersion;
        public SQLiteManualConnection SQLDataConnection;
        public SQLiteManualConnection SQLVersionConnection;
        public VersionTable Version;
        public TableContainer[] Containers;

        public void Load(RowType ManualRowType, ScriptableObject TableData)
        {
            switch (ManualRowType)
            {
                case RowType.MainBase:
                    SQLDataConnection.LoadTable(Cast<MainBaseTable>(TableData));
                    break;
                case RowType.Version:
                    SQLVersionConnection.LoadTable(Cast<VersionTable>(TableData));
                    break;
            }
        }

        public T Cast<T>(ScriptableObject data) where T : ScriptableObject, ITable
        {
            return (T)data;
        }

        private void LoadTables()
        {
            for (int i = 0; i < Containers.Length; i++)
            {
                Load(Containers[i].RowType, Containers[i].Table);
            }
        }

        private bool CheckVersion(out VersionRow versionTask)
        {
            Load(RowType.Version, Version);
            versionTask = Version.rows.FirstOrDefault(x => x.Task.CompareTo("Version") == 0);
            CurrentVersion = versionTask?.Content;
            bool result = CurrentVersion == null ? true : CurrentVersion.CompareTo(ServerVersion) != 0;
            return result;
        }

        private void Awake()
        {            
            VersionRow version = Version.rows?.FirstOrDefault(x => x.Task.CompareTo("Version") == 0);
            CurrentVersion = version?.Content;
        }

        private void Start()
        {
            SQLVersionConnection.Init();
        }

        public bool CheckVersion()
        {
            return CheckVersion(versionTask: out versionTask);
        }

        public void ReloadData()
        {
            if (versionTask != null)
            {
                versionTask.Content = ServerVersion;
                Version.SQLUpdate(SQLVersionConnection.DbConnection, Version.rows.IndexOf(versionTask));
            }
            else
            {
                Version.SQLInsert(SQLVersionConnection.DbConnection, new VersionRow()
                {
                    Id = 1,
                    Task = "Version",
                    Content = ServerVersion,
                    Comment = "None"
                });
            }
            LoadTables();
        }

        public void InitSQLConnection()
        {
            try
            {
                SQLDataConnection.Init();
            }
            catch (System.Exception ex)
            {
                Debug.Log(ex);
                throw;
            }
        }

        private void OnApplicationQuit()
        {
            SQLDataConnection.Dispose();
            SQLVersionConnection.Dispose();
            System.GC.Collect(2, System.GCCollectionMode.Forced);
            System.GC.WaitForPendingFinalizers();
            Debug.Log("Dispose SQL");
        }
    }
}