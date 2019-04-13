using System;
using DataTable.Row;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New BaseDefend Table", menuName = "DataTable/JsonTable/BaseDefend JSONTable", order = 1)]
    public sealed class JSONTable_BaseDefend : JSONTable<BaseDefendRow> { }
}
