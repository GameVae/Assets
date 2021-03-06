﻿using UnityEngine;
using System.Data;
using Mono.Data.Sqlite;
using System;
using Json;
using System.Collections;

namespace DataTable.SQL
{
    public class SQLiteManualConnection : IDisposable
    {
        private string dBPath;
        public SqliteConnection DbConnection { get; private set; }

        private string connString;

        public SQLiteManualConnection(string localPath)
        {
            dBPath = localPath;
            Initalize();
        }

        private void Initalize()
        {
                connString = "URI=file:" + dBPath;
                DbConnection = new SqliteConnection(connString);
        }

        public void LoadTable<T>(SQLiteTable<T> table)
            where T : ISQLiteData
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
                            do
                            {
                                int fieldCount = reader.FieldCount;
                                string json = "";
                                for (int i = 0; i < fieldCount; i++)
                                {
                                    json += MakeJSONValue
                                        (reader.GetName(i), reader.GetValue(i)) + ((i < fieldCount - 1) ? "," : "");
                                }
                                json = FormatJSON(json);

                                table.Add(JsonUtility.FromJson<T>(json));
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