using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;
using ManualTable.Interface;

namespace ManualTable.SQL
{
    public class SQLiteManualConnection : MonoBehaviour
    {
        public SqliteConnection DbConnection { get; private set; }

        public string DBPath;

        private string ConnectionString;       

        private void Awake()
        {
            DBPath = Application.dataPath + DBPath;
            ConnectionString = "URI=file:" + DBPath;

            if (File.Exists(DBPath))
            {
                DbConnection = new SqliteConnection(ConnectionString);
            }
        }

        public void LoadTable<T>(ManualTableBase<T> table) where T : IManualRow, new()
        {
            try
            {
                DbConnection.Open();
                using (IDbCommand dbCmd = DbConnection.CreateCommand())
                {
                    string cmd = string.Format("SELECT * FROM {0} ", table.TableName);
                    dbCmd.CommandText = cmd;
                    using (IDataReader reader = dbCmd.ExecuteReader())
                    {
                        table.Clear();
                        if (reader.Read())
                        {
                            LoadColumns(reader, table);
                            do
                            {
                                int fieldCount = table.FieldCount;
                                string json = "";
                                for (int i = 0; i < fieldCount; i++)
                                {
                                    json += MakeJSONValue(reader.GetName(i), reader.GetValue(i)) + ((i < fieldCount - 1) ? "," : "");
                                }
                                json = FormatJSON(json);
                                table.LoadRow((T)JsonUtility.FromJson(json, typeof(T)));
                            } while (reader.Read());
                        }
                    }
                    DbConnection.Close();
                }
            }
            catch (Exception e)
            {
                DbConnection.Close();
                Debug.Log(e.ToString());
            }
        }

//        public bool CheckVersion<T>(ManualTableBase<T> table) where T : IManualRow
//        {
//            try
//            {
//                DbConnection.Open();
//                using (IDbCommand dbCmd = DbConnection.CreateCommand())
//                {
//                    dbCmd.CommandText = string.Format("SELECT Version FROM {0} ", table.TableName);
//                    using (IDataReader reader = dbCmd.ExecuteReader())
//                    {
//                        if (reader.Read())
//                        {
//                            string version = reader.GetValue(0).ToString();
//                            //if (version.CompareTo(table.Version) == 0)
//                            //{
//                            //    DbConnection.Close();
//                            //    return true;
//                            //}
//                            //else
//                            //{
//                            //   // table.Version = version;
//                            //}
//                        }
//                    }
//                }
//                DbConnection.Close();
//                return false;
//            }
//            catch (Exception e)
//            {
//#if UNITY_EDITOR
//                Debug.Log(e.ToString());
//#endif
//                DbConnection.Close();
//                return false;
//            }
//        }

        private void LoadColumns<T>(IDataReader reader, ManualTableBase<T> table) where T : IManualRow
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                table.LoadColumn(reader.GetName(i));
            }
        }

        #region JSON Utilities
        public static string MakeJSONValue(string field, object value)
        {
            if (value.GetType() == typeof(DBNull) || value == null || value.GetType() == typeof(string))
                return string.Format("\"{0}\":\"{1}\"", field, value);
            else
                return string.Format("\"{0}\":{1}", field, value);
        }

        public static string FormatJSON(string valueString)
        {
            return "{" + valueString + "}";
        }
        #endregion      
    }
}