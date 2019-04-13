using DataTable.Row;
using UnityEngine;

namespace DataTable
{
    [CreateAssetMenu(fileName = "New MainBase Table", menuName = "DataTable/SQLiteTable/MainBase", order = 1)]
    public class SQLiteTable_MainBase : SQLiteTable<MainBaseRow> { }
}