
using DataTable.Row;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New Version Table", menuName = "DataTable/SQLiteTable/Version", order = 4)]
    public class SQLiteTable_Version : SQLiteTable<VersionRow> { }
}