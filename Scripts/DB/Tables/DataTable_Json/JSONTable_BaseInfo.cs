using System;
using DataTable.Row;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New BaseInfo Table", menuName = "DataTable/JsonTable/BaseInfo JSONTable", order = 2)]
    public sealed class JSONTable_BaseInfo : JSONTable<BaseInfoRow>
    {
        public override void LoadRow(string json)
        {
            base.LoadRow(json);
            Rows[Rows.Count - 1].Initalize();
        }
    }
}

