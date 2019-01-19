using ManualTable.Row;
using UnityEngine;

namespace ManualTable
{
    [CreateAssetMenu(fileName = "New MainBase Table", menuName = "SQLiteTable/MainBase", order = 2)]
    public class MainBaseTable : GenericUnitInfoTable<MainBaseRow> { }
}