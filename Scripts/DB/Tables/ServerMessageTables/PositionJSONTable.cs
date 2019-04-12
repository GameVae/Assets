using ManualTable.Row;
using UnityEngine;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New Position Table", menuName = "SQLiteTable/Position JSONTable", order = 4)]
    public sealed class PositionJSONTable : JSONTable<PositionRow> { }
}