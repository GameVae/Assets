using ManualTable.Row;
using UnityEngine;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New Unit Table", menuName = "SQLiteTable/Unit JSONTable", order = 12)]
    public sealed class UnitJSONTable : ManualTableJSON<UnitRow> { }
}