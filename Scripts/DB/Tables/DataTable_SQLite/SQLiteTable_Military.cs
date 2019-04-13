
using DataTable.Row;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New Military Table", menuName = "DataTable/SQLiteTable/Military", order = 2)]
    public class SQLiteTable_Military : SQLiteTable<MilitaryRow> { }
}
