using ManualTable.Interface;
using ManualTable.Row;
using ManualTable.SQL;
using System.Linq;
using UnityEngine;

namespace ManualTable.Loader
{
    public sealed class ManualTableLoader : MonoBehaviour
    {
        public string CurrentVersion;
        public string ServerVersion = "2";
        public SQLiteManualConnection SQLDataConnection;
        public SQLiteManualConnection SQLVersionConnection;
        public VersionTable Version;
        public TableContainer[] Containers;

        public void Load(RowType ManualRowType, ScriptableObject TableData)
        {
            switch (ManualRowType)
            {
                case RowType.MainBase:
                    SQLDataConnection.LoadTable(((MainBaseTable)TableData));
                    break;
                case RowType.Version:
                    SQLVersionConnection.LoadTable(((VersionTable)TableData));
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
            return versionTask == null ? true : (versionTask.Content.CompareTo(ServerVersion) != 0);
        }

        private void Awake()
        {
            VersionRow version = Version.rows?.FirstOrDefault(x => x.Task.CompareTo("Version") == 0);
            CurrentVersion = version?.Content;
        }
        
        private void Start()
        {

            bool isReloadDatabase = CheckVersion(out VersionRow versionTask);
            if (isReloadDatabase)
            {
                if (versionTask != null)
                {
                    versionTask.Content = ServerVersion;
                    Version.SQLUpdate(SQLVersionConnection.DbConnection,Version.rows.IndexOf(versionTask));
                }

                LoadTables();
            }
        }
    }
}