using DataTable.Row;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New RSS Table", menuName = "DataTable/JsonTable/JSONTable", order = 7)]
    public sealed class JSONTable_RSSPosition : JSONTable<RSS_PositionRow> { }
}