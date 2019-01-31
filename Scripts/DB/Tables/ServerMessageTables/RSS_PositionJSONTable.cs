using ManualTable.Row;
using UnityEngine;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New RSS Table", menuName = "SQLiteTable/JSONTable", order = 4)]
    public sealed class RSS_PositionJSONTable : ManualTableJSON<RSS_PositionRow> { }
}