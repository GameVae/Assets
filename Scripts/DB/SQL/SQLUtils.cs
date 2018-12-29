using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Utils
{
    public static class SQLUtils
    {
        #region Select 
        public static object[] QueryValue(this IDbConnection dbConnection, string table, int id)
        {
            object[] result = null;
            try
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    dbCmd.CommandText = string.Format("SELECT * FROM {0} WHERE _rowid_ = {1} ", table, id);
                    using (IDataReader reader = dbCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new object[reader.FieldCount];
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                result[i] = reader.GetValue(i);
                            }
                        }
                    }
                }
                dbConnection.Close();
                return result;
            }
            catch (Exception e)
            {
                dbConnection.Close();
#if UNITY_EDITOR
                Debug.Log(e.ToString());
#endif
                return null;
            }
        }

        public static object[] QueryValue(this IDbConnection dbConnection, string table, int id, params string[] cols)
        {
            object[] result = null;
            try
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    string colsString = "";
                    for (int i = 0; i < cols.Length; i++)
                    {
                        colsString += cols[i] + ((i < cols.Length - 1) ? "," : "");
                    }
                    dbCmd.CommandText = string.Format("SELECT {0} FROM {1} WHERE _rowid_ = {2}", colsString, table, id);
                    using (IDataReader reader = dbCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new object[reader.FieldCount];
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                result[i] = reader.GetValue(i);
                            }
                        }
                    }
                    dbConnection.Close();
                }
                return result;
            }
            catch (Exception e)
            {
                dbConnection.Close();
#if UNITY_EDITOR
                Debug.Log(e.ToString());
#endif
                return null;
            }
        }

        public static object[] QueryValue(this IDbConnection dbConnection, string table, params string[] cols)
        {
            List<object> result = null;
            try
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    string colsString = "";
                    for (int i = 0; i < cols.Length; i++)
                    {
                        colsString += cols[i] + ((i < cols.Length - 1) ? "," : "");
                    }
                    dbCmd.CommandText = string.Format("SELECT {0} FROM {1}", colsString, table);
                    using (IDataReader reader = dbCmd.ExecuteReader())
                    {
                        if (reader.Read())
                            result = new List<object>();
                        do
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                result.Add(reader.GetValue(i));
                            }
                        } while (reader.Read());
                    }
                    dbConnection.Close();
                }
                return result.ToArray();
            }
            catch (Exception e)
            {
#if UNITY_EDITOR
                Debug.Log(e.ToString());
#endif
                dbConnection.Close();
                return null;
            }
        }

        public static object[] QueryValue(this IDbConnection dbConnection, string table, string conditions)
        {
            List<object> result = null;
            try
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    string conditionString = conditions;

                    dbCmd.CommandText = string.Format("SELECT * FROM {0} WHERE {1}", table, conditionString);
                    using (IDataReader reader = dbCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result = new List<object>();
                            do
                            {
                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    result.Add(reader.GetValue(i));
                                }
                            } while (reader.Read());
                        }
                    }
                }
                dbConnection.Close();
                return result.ToArray();
            }
            catch (Exception e)
            {
                dbConnection.Close();
#if UNITY_EDITOR
                Debug.Log(e.ToString());
#endif
                return null;
            }
        }
        #endregion

        #region Update
        public static bool UpdateValue(this IDbConnection dbConnection, string cmd)
        {
            try
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    dbCmd.CommandText = cmd;
                    dbCmd.ExecuteScalar();                    
                }
                dbConnection.Close();
                return true;
            }
            catch (Exception e)
            {
                dbConnection.Close();
#if UNITY_EDITOR
                Debug.Log(e.ToString());
#endif
                return false;
            }
        }
        #endregion

        #region Insert
        public static bool InsertValue(this IDbConnection dbConnection, string cmd)
        {
            try
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    dbCmd.CommandText = cmd;
                    dbCmd.ExecuteScalar();                    
                }
                dbConnection.Close();
                return true;
            }
            catch (Exception e)
            {
                dbConnection.Close();
#if UNITY_EDITOR
                Debug.Log(e.ToString());
#endif
                return false;
            }
        }
        #endregion

        #region Delete
        public static bool Delete(this IDbConnection dbConnection, string cmd)
        {
            try
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    dbCmd.CommandText = cmd;
                    dbCmd.ExecuteScalar();                    
                }
                dbConnection.Close();
                return true;
            }
            catch (Exception e)
            {
                dbConnection.Close();
                Debug.Log(e.ToString());
                return false;
            }
        }
        #endregion

        #region Add column
        public static bool AddColumn(this IDbConnection dbConnection, string cmd)
        {
            try
            {
                dbConnection.Open();
                using (IDbCommand dbCmd = dbConnection.CreateCommand())
                {
                    dbCmd.CommandText = cmd;
                    dbCmd.ExecuteScalar();                    
                }
                dbConnection.Close();
                return true;
            }
            catch (Exception e)
            {
                dbConnection.Close();
                Debug.Log(e.ToString());
                return false;
            }
        }
        #endregion

        public static string GetUpdateCommand(string table, string conditions, string colValuePair)
        {
            return string.Format("UPDATE {0} SET {1} WHERE {2} ", table, colValuePair, conditions);
        }

        public static string GetUpdateCommand(string table, int rowId, string colValuePair)
        {
            return string.Format("UPDATE {0} SET {1} WHERE _rowid_ = {2} ", table, colValuePair, rowId);
        }

        public static string GetInsertCommand(string table, string cols, string values)
        {
            return string.Format("INSERT INTO {0} ({1}) VALUES ({2}) ", table, cols, values);
        }

        public static string GetDeleteCommand(string table, string conditions)
        {
            return string.Format("DELETE FROM {0} WHERE {1}", table, conditions);
        }

        public static string GetSequenceString(string separateSign, params object[] values)
        {
            string result = "";
            for (int i = 0; i < values.Length; i++)
            {
                result += values[i] + ((i < values.Length - 1) ? separateSign : "");
            }
            return result;
        }

        public static string GetGenerateColumnCommand(string table, string colName, string dataType, string defaultValue, bool notNull = false)
        {
            string cmd = string.Format("ALTER TABLE {0} ADD COLUMN {1} {2}", table, colName, dataType);
            if (notNull)
                cmd += @" NOT NULL";
            cmd += @" DEFAULT " + defaultValue;
            return cmd;
        }

        public static string MakeKeyValuePair(string key, object value, string operateSign = "=")
        {
            if (value.GetType() != typeof(string))
                return string.Format("{0} {2} {1}", key, value, operateSign);
            else
                return string.Format("{0} {2} \"{1}\"", key, value, operateSign);
        }

        public static string JsonToString(this string target)
        {
            string[] newS = Regex.Split(target,"\"");
            return newS[1];
        }
    }
}