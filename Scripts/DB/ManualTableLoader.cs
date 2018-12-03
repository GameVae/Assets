using ManualTable.Interface;
using ManualTable.Row;
using ManualTable.SQL;
using UnityEngine;

namespace ManualTable.Loader
{
    public sealed class ManualTableLoader : MonoBehaviour
    {
        public SQLiteManualConnection SQL;
        public TableContainer[] Containers;

        public void Load(RowType ManualRowType, ScriptableObject TableData)
        {
            switch (ManualRowType)
            {
                case RowType.MainBase:
                    SQL.LoadTable(((MainBaseTable)TableData));
                    break;
            }
        }

        public T Cast<T>(ScriptableObject data) where T : ScriptableObject, ITable
        {
            return (T)data;
        }

        private void Start()
        {
            float startTime = Time.realtimeSinceStartup;
            for (int i = 0; i < Containers.Length; i++)
            {
                Load(Containers[i].RowType,Containers[i].Table);
            }
            Debug.Log("Load done: " + (Time.realtimeSinceStartup - startTime));
        }
    }
}