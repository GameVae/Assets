using DataTable.Row;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New Position Table", menuName = "DataTable/JsonTable/Position JSONTable", order = 6)]
    public sealed class JSONTable_Position : JSONTable<PositionRow> { }
}