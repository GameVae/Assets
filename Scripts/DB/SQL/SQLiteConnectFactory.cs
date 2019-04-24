using DataTable.SQL;
using Generic.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQLiteConnectFactory : MonoSingle<SQLiteConnectFactory>
{
    [System.Serializable]
    public class Link
    {
        public SQLiteLinkType LocalLink;
        public string DBPath;
    }

    public enum SQLiteLinkType
    {
        Task = 1,
        Infantry
    }

    [SerializeField] private SQLiteLocalLink links;

    private Dictionary<SQLiteLinkType, SQLiteManualConnection> connections;
    private Dictionary<SQLiteLinkType, SQLiteManualConnection> Connections
    {
        get { return connections ?? 
                (connections = new Dictionary<SQLiteLinkType, SQLiteManualConnection>()); }
    }

    public SQLiteManualConnection GetConnection(SQLiteLinkType sqlLinkType)
    {
        Connections.TryGetValue(sqlLinkType, out SQLiteManualConnection value);
        if(value == null)
        {
            string sqlPath = links[sqlLinkType];
            if(!string.IsNullOrEmpty(sqlPath))
            {
                value = new SQLiteManualConnection(sqlPath);
                Connections[sqlLinkType] = value;
            }
        }
        return value;
    }

    protected override void OnDestroy()
    {
        Debugger.Log(Connections.Count + " connections");
        foreach (var conn in Connections)
        {
            conn.Value.Dispose();
        }
        Connections.Clear();
        base.OnDestroy();
    }
}
