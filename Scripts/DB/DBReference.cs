using EnumCollect;
using Generic.Singleton;
using ManualTable.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace DB
{
    public sealed class DBReference : MonoSingle<DBReference>
    {
        [System.Serializable]
        public class DBKeyValuePair
        {
            public ListUpgrade Key;
            public ScriptableObject Value;
        }

        [System.Serializable]
        public class DBKeyValuePairOther
        {
            public DBType Key;
            public ScriptableObject Value;
        }

        private Dictionary<ListUpgrade, ITable> dbs;
        private Dictionary<DBType, ITable> dbos;

        public ITable this[ListUpgrade dbType]
        {
            get
            {
                try { return dbs[dbType]; }
                catch { return null; }
            }
        }
        public ITable this[DBType dbType]
        {
            get
            {
                try { return dbos[dbType]; }
                catch { return null; }
            }
        }

        public DBKeyValuePair[] InitalizeDB;
        public DBKeyValuePairOther[] InitalizeDBO;

        protected override void Awake()
        {
            base.Awake();
            dbs = new Dictionary<ListUpgrade, ITable>();
            for (int i = 0; i < InitalizeDB?.Length; i++)
            {
                dbs[InitalizeDB[i].Key] = InitalizeDB[i].Value as ITable;
            }

            dbos = new Dictionary<DBType, ITable>();
            for (int i = 0; i < InitalizeDBO?.Length; i++)
            {
                dbos[InitalizeDBO[i].Key] = InitalizeDBO[i].Value as ITable;
            }
        }

    }
}