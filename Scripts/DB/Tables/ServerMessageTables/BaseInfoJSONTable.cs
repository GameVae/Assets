using System;
using ManualTable.Row;
using UnityEngine;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New BaseInfo Table", menuName = "SQLiteTable/BaseInfo JSONTable", order = 9)]
    public sealed class BaseInfoJSONTable : JSONTable<BaseInfoRow>
    {
        public override void LoadRow(string json)
        {
            base.LoadRow(json);
            Rows[Rows.Count - 1].Initalize();
        }
    }
}

