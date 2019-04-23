using DataTable.Row;
using System.Linq;
using UnityEngine;

namespace DataTable.Loader
{
    public sealed class ManualTableLoader : MonoBehaviour
    {
        private VersionRow versionTask;

        public string ClientVersion;
        public string ServerVersion;

        public SQLiteTable_Version Version;
        public ScriptableObject[] SQLiteTables;

        private void Awake()
        {
            GetCurrentVersion();
        }

        private void LoadTables()
        {
            for (int i = 0; i < SQLiteTables.Length; i++)
            {
                ((ISQLiteTable)SQLiteTables[i]).LoadTable();
            }
        }

        private bool IsUpdateVersion(ref VersionRow versionTask)
        {           
            bool result = ClientVersion == null ? true : ClientVersion.CompareTo(ServerVersion) != 0;
            return result;
        }

        public bool CheckVersion()
        {
            return IsUpdateVersion(versionTask: ref versionTask);
        }

        public void ReloadAll()
        {
            if (versionTask != null)
            {
                versionTask.Content = ServerVersion;
                Version.SQLUpdate(Version.Rows.IndexOf(versionTask));
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
                Version.SQLInsert(versionTask);
            }
            LoadTables();
        }

        private void GetCurrentVersion()
        {
            if (versionTask == null)
                Version.LoadTable();

            versionTask = Version.Rows.FirstOrDefault(x => x.Task.CompareTo("Version") == 0);
            ClientVersion = versionTask?.Content;
        }
    }
}