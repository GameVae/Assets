using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System;
using ManualTable.Interface;
using Json.Interface;

namespace ManualTable.SQL
{
    public class SQLiteManualConnection : MonoBehaviour, IDisposable
    {
        public SqliteConnection DbConnection { get; private set; }

        public string DBPath;

        private string ConnectionString;

        private void Awake()
        {
            Init();
        }

        public void Init()
        {
            if(ConnectionString == null)
            {
                DBPath = Application.dataPath + DBPath;
                ConnectionString = "URI=file:" + DBPath;
            }
            if (DbConnection == null)
            {
                DbConnection = new SqliteConnection(ConnectionString);
            }
        }

        public void LoadTable<T>(ManualTableBase<T> table) where T : IManualRow, IJSON, new()
        {
            try
            {
                DbConnection.Open();
                using (IDbCommand dbCmd = DbConnection.CreateCommand())
                {
                    string cmd = string.Format("SELECT * FROM {0}", table.TableName);
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
                                //table.LoadRow((T)JsonUtility.FromJson(json, typeof(T)));
                                table.LoadRow(json);
                            } while (reader.Read());

                        }
                        else
                        {
#if UNITY_EDITOR
                            Debug.Log("Read failed ! " + table.TableName + " field count: " + reader.FieldCount);
#endif
                        }
                        reader.Close();
                        dbCmd.Dispose();
                    }
                    DbConnection.Close();
                }
            }
            catch (Exception e)
            {
                DbConnection.Close();
#if UNITY_EDITOR
                Debug.Log(e.ToString());
#endif
            }
        }

        private void LoadColumns<T>(IDataReader reader, ManualTableBase<T> table) where T : IManualRow, IJSON, new()
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

        public void Dispose()
        {
            if (DbConnection != null)
            {
                DbConnection.ConnectionString = "";
                DbConnection.Close();
                DbConnection.Dispose();
                DbConnection = null;
            }
        }
    }
}