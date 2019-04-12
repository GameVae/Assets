using System;
using ManualTable.Row;
using UnityEngine;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New BaseDefend Table", menuName = "SQLiteTable/BaseDefend JSONTable", order = 11)]
    public sealed class BaseDefendJSONTable : JSONTable<BaseDefendRow> { }
}
